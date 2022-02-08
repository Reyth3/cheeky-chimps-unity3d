using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StakeManager : MonoBehaviour
{
    public enum StakingBehavior { Staking = 0, Unstaking = 1 }

    public StakingBehavior behavior;

    public List<TokenDisplay> selectedTokens;
    Button button;
    TextMeshProUGUI _text;
    string[] stakingMessages = new string[] {
        "Select Two NFTs",
        "Stake Selected NFTs",
        "Too Many NFTs Selected"
    };

    string[] unstakingMessages = new string[] {
        "Select your Stake",
        "Confirm Unstake"
    };

    void Start()
    {
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
        if (behavior == StakingBehavior.Staking)
        {

            if (selectedTokens.Count < 2)
            {
                _text.text = stakingMessages[0];
                button.interactable = false;
            }
            else if (selectedTokens.Count == 2)
            {
                _text.text = stakingMessages[1];
                button.interactable = true;
            }
            else
            {
                _text.text = stakingMessages[2];
                button.interactable = false;
            }
        }
        else {
            if (selectedTokens.Count < 1)
            {
                _text.text = unstakingMessages[0];
                button.interactable = false;
            }
            else if (selectedTokens.Count == 2)
            {
                _text.text = unstakingMessages[1];
                button.interactable = true;
            }
        }
    }

    public void OnStakeTokensClick()
    {
        // TODO: Someone needs to connect it with the AWS server 
        Debug.LogError("Staking implementation goes here.");
    }

    public void SwitchSceneButtonClick()
    {
        var currentState = (int)behavior;
        var targetState = (StakingBehavior)(currentState == 0 ? 1 : 0);
        StopAllCoroutines();
        SceneManager.LoadScene(targetState.ToString(), LoadSceneMode.Single);
    }
}
