using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TokenDisplay : MonoBehaviour
{
    public bool isChecked;
    ERC721Metadata loadedToken;
    TextMeshProUGUI _title;
    Image _thumbnail;
    Image _checkmark;

    void Awake()
    {
        _title = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        _thumbnail = transform.GetChild(0).GetComponent<Image>();
        _checkmark = transform.GetChild(2).GetComponent<Image>();
    }

    public void UpdateTokenMetadata(ERC721Metadata tokenMetadata)
    {
        loadedToken = tokenMetadata;
        StartCoroutine("LoadThumbnail");
    }

    private IEnumerator LoadThumbnail()
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(loadedToken.Image);

        yield return request.SendWebRequest();
        if(request.result != UnityWebRequest.Result.Success) 
            Debug.Log(request.error);
        else
        {
            Texture2D tex = ((DownloadHandlerTexture) request.downloadHandler).texture;
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
            _thumbnail.overrideSprite = sprite;
        } 
    }

    void FixedUpdate() {
        _title.text = loadedToken?.Name ?? "HTTP Load Error";
    }
    public void SetChecked(bool c)
    {
        isChecked = c;
        _checkmark.gameObject.SetActive(c);
        if(c)
            TokenChecked?.Invoke(this);
        else TokenUnchecked?.Invoke(this);
    }

    public void ToggleChecked()
    {
        print("Check toggled");
        SetChecked(!isChecked);
    }

    public static event Action<TokenDisplay> TokenChecked;
    public static event Action<TokenDisplay> TokenUnchecked;
}
