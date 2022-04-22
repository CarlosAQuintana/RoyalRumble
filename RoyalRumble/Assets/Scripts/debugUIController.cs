using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class debugUIController : MonoBehaviour
{
    gameManager gameManager;
    roundManager roundManager;
    [SerializeField] private TextMeshProUGUI roundNumber;
    [SerializeField] private TextMeshProUGUI[] playerScoreTexts;
    void Start()
    {
        gameManager = FindObjectOfType<gameManager>();
        roundManager = FindObjectOfType<roundManager>();
        roundNumber = GameObject.Find("Round Number").GetComponent<TextMeshProUGUI>();
        playerScoreTexts = new TextMeshProUGUI[4];
        for (int s = 0; s < playerScoreTexts.Length; s++)
        {
            playerScoreTexts[s] = GameObject.Find("playerScore" + s).GetComponent<TextMeshProUGUI>();
        }
        playerScoreTexts[0].text = ("");
        playerScoreTexts[1].text = ("");
        playerScoreTexts[2].text = ("");
        playerScoreTexts[3].text = ("");
    }
    void Update()
    {
        roundNumber.text = ("Round - " + roundManager.currentRound);
        if (gameManager.playGame)
        {
            playerScoreTexts[0].text = ("" + roundManager.playerScore[0]);
            playerScoreTexts[1].text = ("" + roundManager.playerScore[1]);
            if (roundManager.playerScore.Length >= 3)
            {
                playerScoreTexts[2].text = ("" + roundManager.playerScore[2]);
                if (roundManager.playerScore.Length == 4)
                {
                    playerScoreTexts[3].text = ("" + roundManager.playerScore[3]);
                }
            }
        }
    }
}
