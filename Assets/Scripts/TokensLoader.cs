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
            account = "0xBf00cAeE3f4d0E654b5E1A557914D257f126d055";
        }
        int[] tokenIds = await WalletOfOwner(chain, network, contract, account);

        if (tokenIds.Length == 0)
        {
            _debugView.text = "You don't own any.";
            return;
        }

        _debugView.text = "";

        var urlTemplate = "https://cheekychimps.s3.us-east-2.amazonaws.com/{x}.json";
        var tokens = new List<ERC721Metadata>();
        using var http = new HttpClient();
        foreach (var id in tokenIds)
        {
            var json = await http.GetStringAsync(urlTemplate.Replace("{x}", id.ToString()));
            tokens.Add(JsonConvert.DeserializeObject<ERC721Metadata>(json));
        }

        FindObjectOfType<TokenGridManager>().LoadTokens(tokens.ToArray());

    }
}
