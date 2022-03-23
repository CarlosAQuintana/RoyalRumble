using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class roundManager : MonoBehaviour
{

    public enum roundState { gameBegin, roundStart, roundPlay, roundEnd } // Each state a round can be in.
    [Header("Game Variables")]
    public PlayerController[] players;
    public combatController[] combatControllers;

    [Header("Round Variables")]
    public roundState currentRoundState;
    public int currentRound = 1; // Current round.
    public int maxRounds; // Set the max number of rounds wanted for gameplay.
    public int numOfPlayersAlive;
    public bool trackRoundTime;
    [SerializeField] private float roundTimeElapsed; // Time elapsed from beginning of round.

    [Header("Player Variables")]

    public GameObject playerPrefab;
    public int playerCount;
    public int maxPlayerCount;
    public int[] playerScore;
    public bool[] playerIsDead;

    // I took the loop away from player spawn because it
    // was causing some issues with debugging; Spawns
    // are now added manually via the Unity Editor.
    public Transform[] playerSpawns;

    // I added a few script references.
    public PlayerInputManager playerManager;
    public gameManager manager;

    void Start()
    {
        // I referenced the Game Manager script.
        manager = FindObjectOfType<gameManager>();
        players = new PlayerController[4];
        combatControllers = new combatController[4];

        // If a specific round number isn't set, set game length to 3 rounds.
        if (maxRounds < 3)
            maxRounds = 3;
        currentRound = 1;
    }

    void Update()
    {
        // I added the ability to prevent players from joining
        // while the game is being played.
        if (manager.playGame)
        {
            playerManager.DisableJoining();
        }
        if (trackRoundTime)
        {
            roundTimeElapsed += Time.deltaTime;
        }
    }

    public void roundStateController()
    {
        switch (currentRoundState)
        {
            case roundState.gameBegin: // Only call this at the beginning of game.
                numOfPlayersAlive = playerCount;
                StartCoroutine("resetRound");
                break;
            case roundState.roundStart: // Everytime a round needs to start.
                numOfPlayersAlive = playerCount;
                for (int p = 0; p < playerIsDead.Length; p++)
                {
                    playerIsDead[p] = false;
                }
                for (int p = 0; p < playerCount; p++)
                {
                    players[p].transform.position = playerSpawns[p].position;
                    if (players[p] == null)
                        break;
                }
                StartCoroutine("resetRound");
                FindObjectOfType<weapon>().isEquippable = true;
                currentRoundState = roundState.roundPlay;
                break;
            case roundState.roundPlay:
                break;
            case roundState.roundEnd:
                break;
        }
    }
    public IEnumerator resetRound()
    {
        trackRoundTime = false;
        roundTimeElapsed = 0;
        for (int c = 0; c < combatControllers.Length; c++) // Uenequip every weapon.
        {
            if (combatControllers[c] == null)
                break;
            combatControllers[c].unEquipWeapon();
        }
        controlAllMovement(false, true);
        yield return new WaitForSeconds(2f);
        controlAllMovement(true, false);
        trackRoundTime = true;
        //currentRoundState = roundState.roundPlay;
    }
    public void checkForRoundWin() // If one player is left, find their index, give them a point and advance the round.
    {
        int winnerIndex = 0;
        if (numOfPlayersAlive == 1)
        {
            Debug.Log("One player left!");
            for (int p = 0; p < playerIsDead.Length; p++)
            {
                if (playerIsDead[p] == false)
                {
                    winnerIndex = p;
                    break;
                }
            }
            Debug.Log("Player " + winnerIndex + " won the round!");
            currentRound++;
            playerScore[winnerIndex]++;
            currentRoundState = roundState.roundStart;
            roundStateController();
        }
    }
    public void checkGameWin() // Needs major reworking don't use for now!
    {
        int winnerIndex = 0;
        if (currentRound == maxRounds)
        {
            int s1 = playerScore[0];
            int s2 = playerScore[1];
            int s3 = playerScore[2];
            int s4 = playerScore[3];
            if (s1 > s2 && s1 > s3 && s1 > s4)
            {
                winnerIndex = 0;
                Debug.Log("Player " + winnerIndex + " won the game!");
            }
            else if (s2 > s1 && s2 > s3 && s2 > s4)
            {
                winnerIndex = 1;
                Debug.Log("Player " + winnerIndex + " won the game!");
            }
            else if (s3 > s1 && s3 > s2 && s3 > s4)
            {
                winnerIndex = 2;
                Debug.Log("Player " + winnerIndex + " won the game!");
            }
            else if (s4 > s1 && s4 > s2 && s4 > s3)
            {
                winnerIndex = 3;
                Debug.Log("Player " + winnerIndex + " won the game!");
            }

        }
    }
    public void controlAllMovement(bool enable, bool disable) // Quick disable all player movement.
    {
        if (enable)
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] == null)
                    break;
                players[i].canMove = true;
            }
        else if (disable)
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] == null)
                    break;
                players[i].canMove = false;
            }
    }
    // I added a function that is called whenever a new player joins.
    void OnPlayerJoined(PlayerInput playerInput)
    {
        // I added the ability to give each player a unique ID based on the order in 
        // which they joined.
        playerInput.gameObject.GetComponent<PlayerController>().playerID = playerInput.playerIndex;

        // I added the ability for players to spawn in unique spawn points depending on 
        // the order in which they joined.
        playerInput.gameObject.GetComponent<PlayerController>().startPos = playerSpawns[playerInput.playerIndex].position;

        // I added the ability to update the current player count based on
        // how many players have joined (+ 1 due to array index starting at 0).
        playerCount = playerInput.playerIndex + 1;

        // Add the PlayerController and combatController of the current player being spawned to a list.
        players[playerInput.playerIndex] = playerInput.gameObject.GetComponent<PlayerController>();
        combatControllers[playerInput.playerIndex] = playerInput.gameObject.GetComponent<combatController>();

        // Name GameObject based on player index.
        playerInput.gameObject.name = ("Player " + playerInput.playerIndex);
    }
}
