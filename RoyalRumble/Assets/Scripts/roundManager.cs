using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roundManager : MonoBehaviour
{
    public enum roundState { roundStart, roundPlay, roundEnd } // Each state a round can be in.

    [Header("Round Variables")]
    public roundState currentRoundState;
    public int currentRound; // Current round.
    public int maxRounds; // Set the max number of rounds wanted for gameplay.
    [SerializeField] private float roundTimeElapsed; // Time elapsed from beginning of round.

    [Header("Player Variables")]
    public GameObject playerPrefab;
    public int currentPlayers;
    public int maxPlayerCount;
    public int[] playerScore;
    public bool[] playerIsDead;
    public Transform[] playerSpawns;
    void Start()
    {
        findPlayerSpawns();
    }
    void Update()
    {

    }
    public void roundStateController()
    {
        switch (currentRoundState)
        {
            case roundState.roundStart:
                for (int p = 0; p < playerIsDead.Length; p++)
                {
                    playerIsDead[p] = false;
                }
                break;
            case roundState.roundPlay:
                break;
            case roundState.roundEnd:
                break;
        }
    }
    public void findPlayerSpawns()
    {
        playerSpawns = new Transform[4];
        for (int i = 0; i < playerSpawns.Length; i++)
        {
            playerSpawns[i] = GameObject.Find("spawn" + i).GetComponent<Transform>();
        }
    }
}
