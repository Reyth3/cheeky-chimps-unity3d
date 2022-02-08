using System.Collections;
using System.Numerics;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TMPro;
using System.Text;
using System.Linq;
using System;
using System.Net.Http;
using UnityEngine.Networking;

public class TokensLoader : MonoBehaviour
{
    public string ABI;
    private TextMeshProUGUI _debugView;

    public async Task<int[]> WalletOfOwner(string _chain, string _network, string _contract, string _account, string _rpc = "")
    {

        string method = "walletOfOwner";
        string[] obj = { _account };
        string args = JsonConvert.SerializeObject(obj);
        string response = await EVM.Call(_chain, _network, _contract, ABI, method, args, _rpc);
        try
        {
            //Debug.Log(response);
            return JsonConvert.DeserializeObject<int[]>(response);
        }
        catch
        {
            Debug.LogError(response);
            throw;
        }
    }


    async void Start()
    {
        _debugView = GameObject.Find("Canvas/Window/TokensDebugView").GetComponent<TextMeshProUGUI>();

        string chain = "ethereum";
        string network = "mainnet";
        string contract = "0xcaA37C3822Cc2e7ABD2Ae2FE7EE85F053220A650";
        string account = PlayerPrefs.GetString("Account");
        if (string.IsNullOrEmpty(account))
        {
            Debug.Log("User not logged in!");
            account = "0x6D68bbE7534eb44058964419E98050a3A042d382";
            PlayerPrefs.SetString("Account", account);
        }
        int[] tokenIds = await WalletOfOwner(chain, network, contract, account);

        if (tokenIds.Length == 0)
        {
            _debugView.text = "You don't own any.";
            return;
        }

        _debugView.text = "Downloading JSON Metadata...";

        var urlTemplate = "https://cheekychimps.s3.us-east-2.amazonaws.com/{x}.json";
        var tokens = new List<ERC721Metadata>();
        var requests = new List<UnityWebRequest>();
        var tasks = new List<Task>();
        foreach (var id in tokenIds)
        {
            try {
            UnityWebRequest req = UnityWebRequest.Get(urlTemplate.Replace("{x}", id.ToString()));
            requests.Add(req);
            tasks.Add(req.SendWebRequest().GetTask());
            } catch {}
        }
        await Task.WhenAll(tasks);
        // Unity sucks when it comes to networking, concurrency and in general WEBGL is missing so much 
        // functionality so it all just has to be crude AF. Forgive me for doing it the way I did it, I'm 
        // not getting paid enough to care.
        
        _debugView.text = "";

        foreach(var req in requests)
        { try {
            var json = req.downloadHandler.text;
            tokens.Add(JsonConvert.DeserializeObject<ERC721Metadata>(json));
        } catch {} // this purely because the AWS server isn't configured properly and I don't wanna throw errors left&right
        }
        FindObjectOfType<TokenGridManager>().LoadTokens(tokens.ToArray());

    }
}