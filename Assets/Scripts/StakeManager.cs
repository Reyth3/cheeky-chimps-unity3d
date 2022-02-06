using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StakeManager : MonoBehaviour
{
    public List<TokenDisplay> selectedTokens;
    Button button;
    TextMeshProUGUI _text;
    string[] messages = new string[] {
        "Select Two NFTs",
        "Stake Selected NFTs",
        "Too Many NFTs Selected"
    };

    void Start() {
        TokenDisplay.TokenChecked += TokenChecked;
        TokenDisplay.TokenUnchecked += TokenUnchecked;

        selectedTokens = new List<TokenDisplay>();
        
        _text = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();
        UpdateUI();
    }

    void OnDestroy()
    {
        TokenDisplay.TokenChecked -= TokenChecked;
        TokenDisplay.TokenUnchecked -= TokenUnchecked;        
    }

    private void TokenUnchecked(TokenDisplay obj)
    {
        selectedTokens.Remove(obj);
        UpdateUI();
    }

    private void TokenChecked(TokenDisplay obj)
    {
        selectedTokens.Add(obj);
        UpdateUI();
    }

    void UpdateUI()
    {
        if(selectedTokens.Count < 2)
        {
            _text.text = messages[0];
            button.interactable = false;
        }
        else if (selectedTokens.Count == 2)
        {
            _text.text = messages[1];
            button.interactable = true;
        }
        else {
            _text.text = messages[2];
            button.interactable = false;
        }
    }
}
