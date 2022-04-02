using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class roundManager : MonoBehaviour
{

    public enum roundState { gameBegin, roundStart, roundPlay, roundEnd } // Each state a round can be in.

    [Header("Game Variables")]
    [HideInInspector] public PlayerController[] players;
    [HideInInspector] public combatController[] combatControllers;

    [Header("Round Variables")]
    public roundState currentRoundState;
    public int currentRound = 1; // Current round.
    public int maxRounds; // Set the max number of rounds wanted for gameplay.
    public int numOfPlayersAlive;
    private bool trackRoundTime;
    [SerializeField] private float roundTimeElapsed; // Time elapsed from beginning of round.

    [Header("Player Variables")]

    public GameObject playerPrefab;
    public int playerCount;
    public int maxPlayerCount;
    public int[] playerScore;
    public bool[] playerIsDead;

    // Level spawns added manually via the Unity Editor.
    public Transform[] playerSpawnsRound1;
    public Transform[] playerSpawnsRound2;

    //public Transform[] playerSpawnsRound3;

    //public Transform[] playerSpawnsRound4;

    // Script references.
    public PlayerInputManager playerManager;
    public gameManager manager;

    // Added an animator for the state-driven camera.
    public Animator Cameras;


    void Start()
    {
        // Find controllers and scripts.
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
        // Prevent players from joining
        // while the game is being played.
        if (manager.playGame)
        {
            playerManager.DisableJoining();
        }

        // Counts the length of a single round.
        if (trackRoundTime)
        {
            roundTimeElapsed += Time.deltaTime;
        }
    }

    // Controls what the current state that the round or game is in from beginning to end.
    public void roundStateController()
    {
        switch (currentRoundState)
        {
            // Only call this at the beginning of game.
            case roundState.gameBegin:
                // All players are alive; start a round.
                numOfPlayersAlive = playerCount;
                StartCoroutine("resetRound");
                break;

            // Everytime a round needs to start.
            case roundState.roundStart:
                // Set each player to be alive; start a round.
                numOfPlayersAlive = playerCount;
                for (int p = 0; p < playerIsDead.Length; p++)
                {
                    playerIsDead[p] = false;
                }
                StartCoroutine("resetRound");
                currentRoundState = roundState.roundPlay;
                break;

            // When round is in play.
            case roundState.roundPlay:
                break;

            // When round is over.
            case roundState.roundEnd:
                break;
        }
    }
    public IEnumerator resetRound()
    {
        trackRoundTime = false; // Reset round timer.
        roundTimeElapsed = 0;
        for (int c = 0; c <= combatControllers.Length; c++) // Unequip every weapon.
        {
            if (combatControllers[c] == null)
                break;
            combatControllers[c].unEquipWeapon();
        }
        debugResetAllWeapons(); // For now find each weapon and reset their equipable status.

        if (currentRound == 1)
        {
            // Set camera to face the level 1.
            Cameras.SetBool("isRoundTwo", false);

            // Spawn players into level 1.
            for (int p = 0; p <= players.Length; p++)
            {
                if (players[p] == null)
                    break;
                players[p].transform.position = playerSpawnsRound1[p].position;
            }

            // Wait until spawned before giving players control & tracking time.
            yield return new WaitForSeconds(2f);
            controlAllMovement(true, false);
            freezeControl(false);
            trackRoundTime = true;
        }

        // From here movement is disabled automatically in
        // 'checkForRoundWin()' until the next round starts.
        if (currentRound == 2)
        {
            // Set camera to face the level 2.
            Cameras.SetBool("isRoundTwo", true);

            // Disable movement until game starts.
            controlAllMovement(false, true);
            freezeControl(true);

            // Spawn players into level 2.
            for (int p = 0; p <= players.Length; p++)
            {
                if (players[p] == null)
                    break;
                players[p].transform.position = playerSpawnsRound2[p].position;
            }

            // Wait until spawned before giving players control.
            yield return new WaitForSeconds(0.5f);
            controlAllMovement(true, false);

            // Wait until players drop to start tracking time again.
            yield return new WaitForSeconds(1.5f);
            freezeControl(false);
            trackRoundTime = true;
        }

        if (currentRound == 3)
        {
            // Reset camera to face level 3.
            // Spawn players into level 3 spawn points.
            // Unfreeze players to allow them to drop into the map.
            // Start timer once players land on the map.
        }

        if (currentRound == 4)
        {
            // Reset camera to face level 4.
            // Spawn players into level 4 spawn points.
            // Unfreeze players to allow them to drop into the map.
            // Start timer once players land on the map.
        }
    }

    // Executes when player potentially won a round.
    public void checkForRoundWin()
    {
        // Check for winning player if there is only 1 player left.
        int winnerIndex = 0;
        if (numOfPlayersAlive == 1)
        {
            Debug.Log("One player left!");
            for (int p = 0; p <= playerIsDead.Length; p++)
            {
                if (playerIsDead[p] == false)
                {
                    winnerIndex = p;
                    break;
                }
            }

            // Procede to next round and increase winning player's score.
            Debug.Log("Player " + winnerIndex + " won the round!");
            currentRound++;
            playerScore[winnerIndex]++;

            if (currentRound > 4)
            {
                // Game is over; cut to win screen.
            }

            // Loop back around to level 1.
            if (currentRound > 2)
            {
                currentRound = 1;
            }

            // Begin round and freeze player movement until players
            // spawn in and round starts.
            currentRoundState = roundState.roundStart;
            controlAllMovement(false, true);
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

    // Quick disable all player movement.
    public void controlAllMovement(bool enable, bool disable)
    {
        if (enable)
            for (int i = 0; i <= players.Length; i++)
            {
                if (players[i] == null)
                    break;
                players[i].canMove = true;
            }

        else if (disable)
            for (int i = 0; i <= players.Length; i++)
            {
                if (players[i] == null)
                    break;
                players[i].canMove = false;
            }
    }

    // Quick disable all player control.
    public void freezeControl(bool enable)
    {
        if (enable)
            for (int i = 0; i <= players.Length; i++)
            {
                if (players[i] == null)
                    break;
                players[i].canControl = false;
            }

        else if (!enable)
            for (int i = 0; i <= players.Length; i++)
            {
                if (players[i] == null)
                    break;
                players[i].canControl = true;
            }
    }
    public void debugResetAllWeapons()
    {
        weapon[] allActiveWeapons = FindObjectsOfType<weapon>();
        foreach (weapon userweapon in allActiveWeapons)
        {
            userweapon.isEquippable = true;
            userweapon.enableMesh();
        }
    }
    // I added a function that is called whenever a new player joins.
    void OnPlayerJoined(PlayerInput playerInput)
    {
        // Give each player a unique ID based on the order in 
        // which they joined.
        playerInput.gameObject.GetComponent<PlayerController>().playerID = playerInput.playerIndex;

        // Players spawn in unique spawn points depending on 
        // the order in which they joined.
        playerInput.gameObject.GetComponent<PlayerController>().startPos = playerSpawnsRound1[playerInput.playerIndex].position;

        // Update the current player count based on
        // how many players have joined (+ 1 due to array index starting at 0).
        playerCount = playerInput.playerIndex + 1;

        // Add the PlayerController and combatController of the current player being spawned to a list.
        players[playerInput.playerIndex] = playerInput.gameObject.GetComponent<PlayerController>();
        combatControllers[playerInput.playerIndex] = playerInput.gameObject.GetComponent<combatController>();

        // Name GameObject based on player index.
        playerInput.gameObject.name = ("Player " + playerInput.playerIndex);
    }
}
