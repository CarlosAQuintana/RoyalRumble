using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public roundManager roundManager;
    void Start()
    {

    }
    void Update()
    {

    }
    public void beginPlay()
    {
        GameObject playerPre = roundManager.playerPrefab;
        for (int i = roundManager.currentPlayers; i < roundManager.maxPlayerCount; i++)
        {
            Instantiate(playerPre);
            playerPre.GetComponent<player>().isCPU = true;
        }
    }
    public void OnPlayerJoin()
    {
        roundManager.currentPlayers++;
    }
}
