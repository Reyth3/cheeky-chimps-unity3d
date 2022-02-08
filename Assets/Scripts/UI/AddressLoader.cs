using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddressLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void FixedUpdate()
    {
        var addr = PlayerPrefs.GetString("Account");
        if(addr == "")
            return;
        var formatted = $"{addr.Substring(0, 6)}...{addr.Substring(addr.Length - 4)}";

        GetComponent<TextMeshProUGUI>()
            .text = formatted;
    }
}
