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
    public Transform[] playerSpawnsRound1;
    public Transform[] playerSpawnsRound2;

    // I added a few script references.
    public PlayerInputManager playerManager;
    public gameManager manager;

    // Added boolean values which determine whether it is the first round, second round,
    // or if we need to reset the game.
    private bool isRoundTwo = false;
    private bool firstRound = true;
    private bool resetGame = false;

    // Added an animator for the state-driven camera.
    public Animator Cameras;

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
                // I removed the loop which respawns players at the first spawn
                // locations, as there is now a new round with new spawn locations.
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
        for (int c = 0; c < combatControllers.Length; c++) // Unequip every weapon.
        {
            if (combatControllers[c] == null)
                break;
            combatControllers[c].unEquipWeapon();
        }

        // If it is time for the second round, move the camera to face the
        // second level, prevent movement to teleport players to new spawn
        // points, teleport players to that point, then give players control
        // again. Next, allow for reseting the game when round 2 is over.
        // (Later we will have more rounds and no need to reset, but for now we have 
        // to switch back and forth between the first and second).
        if (isRoundTwo)
        {
            Cameras.SetBool("isRoundTwo", true);

            //controlAllMovement(false, true);
            for (int p = 0; p < players.Length + 1; p++)
            {
                if (players[p] == null)
                    break;
                players[p].transform.position = playerSpawnsRound2[p].position;
            }
            yield return new WaitForSeconds(3f);
            controlAllMovement(true, false);

            yield return new WaitForSeconds(2f);
            trackRoundTime = true;

            isRoundTwo = false;
            resetGame = true;
        }

        // If it is only the first round, set camera to face the first level, 
        // prevent movement to move players to level 1 spawn points,
        // move them to those points, then give them their control back.
        // Next, allow for round 2 to begin when the first round is over.
        else if (firstRound)
        {
            Cameras.SetBool("isRoundTwo", false);
            controlAllMovement(false, true);
            for (int p = 0; p < players.Length + 1; p++)
            {
                if (players[p] == null)
                    break;
                players[p].transform.position = playerSpawnsRound1[p].position;
            }

            yield return new WaitForSeconds(2f);
            controlAllMovement(true, false);
            trackRoundTime = true;

            firstRound = false;
            isRoundTwo = true;
        }
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

            // If it is time to loop back around to the first level
            // (which is what happens until we can incorporate more than 2)
            // we will allow for allow for level 1 to load when the current
            // round is over.
            if (resetGame)
            {
                firstRound = true;
                resetGame = false;
            }

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
        playerInput.gameObject.GetComponent<PlayerController>().startPos = playerSpawnsRound1[playerInput.playerIndex].position;

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
