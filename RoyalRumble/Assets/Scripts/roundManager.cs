using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(AudioSource))]
public class roundManager : MonoBehaviour
{
    public enum roundState { gameBegin, roundStart, roundPlay, roundEnd } // Each state a round can be in.
    public enum level { castle, ice, jungle, fire }

    [Header("Compoonent and Script Variables")]
    public PlayerInputManager playerManager;
    public gameManager manager;
    public PlayerController[] players;
    public combatController[] combatControllers;
    public playerSwagController[] swagControllers;

    [Header("Sound Effects")]
    public AudioSource source;
    public AudioClip spawnSound;
    public AudioClip startSound;
    public AudioClip[] roundWinSound;
    public AudioClip[] gameWinSound;

    [Header("Round Variables")]
    public roundState currentRoundState;
    public level currentLevel;
    public int currentRound = 1; // Current round.
    public int maxRounds; // Set the max number of rounds wanted for gameplay.
    public int scoreToWin = 3;
    public int numOfPlayersAlive;
    public bool gameOver;
    private bool trackRoundTime;
    public bool TrackRound { get => trackRoundTime; }
    private bool roundInPlay;
    public bool RoundIsInPlay { get => roundInPlay; }
    [SerializeField] private float roundTransitionDelay = 5f;
    [SerializeField] private float _roundTimeElapsed; // Time elapsed from beginning of round.
    public float RoundTimeElasped { get => _roundTimeElapsed; }

    [Header("Player Variables")]
    public GameObject playerPrefab;
    public int playerCount;
    public int maxPlayerCount;
    public int[] playerScore;
    public bool[] playerIsDead;

    // Level spawns added manually via the Unity Editor.
    public Transform[] playerSpawnsRound1;
    public Transform[] playerSpawnsRound2;
    public Transform[] playerSpawnsRound3;
    public Transform[] playerSpawnsRound4;

    public Animator Cameras; // Animator for the state-driven camera.

    void Start()
    {
        // Find controllers and scripts.
        manager = FindObjectOfType<gameManager>();
        //players = new PlayerController[4];
        //combatControllers = new combatController[4];
        gameOver = false;
        playSound(startSound);
    }
    void Update()
    {
        if (manager.playGame)
        {
            playerManager.DisableJoining(); // Prevent players from joining while the game is being played.
        }
        if (trackRoundTime)
        {
            _roundTimeElapsed += Time.deltaTime; // Counts the length of a single round.
        }
    }
    // Controls what the current state that the round or game is in from beginning to end.
    public void roundStateController()
    {
        switch (currentRoundState)
        {
            case roundState.gameBegin: // Only call this at the beginning of game.
                //currentRound = 1;
                numOfPlayersAlive = playerCount;
                StartCoroutine("resetRound");

                break;
            case roundState.roundStart: // Set each player to be alive then start the round.
                numOfPlayersAlive = playerCount;
                for (int p = 0; p < playerIsDead.Length; p++)
                {
                    playerIsDead[p] = false;
                }
                StartCoroutine("resetRound");
                currentRoundState = roundState.roundPlay;
                break;
            case roundState.roundPlay: // When round is in play.
                break;

            case roundState.roundEnd: // When round is over.
                break;
        }
    }
    public IEnumerator resetRound()
    {
        roundInPlay = false;
        foreach (combatController controller in combatControllers)
        {
            controller.StopAllCoroutines();
        }
        yield return new WaitForSeconds(.25f);
        controlAllMovement(false, true);
        freezeControl(true);
        trackRoundTime = false; // Reset round timer.
        _roundTimeElapsed = 0;
        for (int c = 0; c < combatControllers.Length; c++) // Unequip every weapon.
        {
            combatControllers[c].unEquipWeapon();
        }
        if (currentRound == 1 || currentRound == 5)
        {
            changeYValue(0f);
            currentLevel = level.castle;
            debugResetAllWeapons();
            Cameras.Play("Level 1 Camera");
            for (int p = 0; p < players.Length; p++)
            {
                if (players[p] == null)
                    break;
                players[p].transform.position = playerSpawnsRound1[p].position;
            }
            yield return new WaitForSeconds(roundTransitionDelay);
            controlAllMovement(true, false);
            freezeControl(false);
            trackRoundTime = true;
        }
        else if (currentRound == 2 || currentRound == 6)
        {
            changeYValue(-65f);
            slipperyControl(true);
            currentLevel = level.ice;
            debugResetAllWeapons();
            Cameras.Play("Level 2 Camera");
            for (int p = 0; p < players.Length; p++)
            {
                if (players[p] == null)
                    break;
                players[p].transform.position = playerSpawnsRound2[p].position;
            }
            yield return new WaitForSeconds(roundTransitionDelay);
            controlAllMovement(true, false);
            freezeControl(false);
            trackRoundTime = true;
        }
        else if (currentRound == 3 || currentRound == 7)
        {
            changeYValue(-120f);
            slipperyControl(false);
            currentLevel = level.jungle;
            debugResetAllWeapons();
            Cameras.Play("Level 3 Camera");
            for (int p = 0; p < players.Length; p++)
            {
                if (players[p] == null)
                    break;
                players[p].transform.position = playerSpawnsRound3[p].position;
            }
            yield return new WaitForSeconds(roundTransitionDelay);
            controlAllMovement(true, false);
            freezeControl(false);
            trackRoundTime = true;
        }
        else if (currentRound == 4 || currentRound == 8)
        {
            changeYValue(-180f);
            currentLevel = level.fire;
            debugResetAllWeapons();
            Cameras.Play("Level 4 Camera");
            for (int p = 0; p < players.Length; p++)
            {
                if (players[p] == null)
                    break;
                players[p].transform.position = playerSpawnsRound4[p].position;
            }
            yield return new WaitForSeconds(roundTransitionDelay);
            controlAllMovement(true, false);
            freezeControl(false);
            trackRoundTime = true;
        }
        roundInPlay = true;
        foreach (combatController controller in combatControllers)
        {
            controller.canAttack = true;
        }
    }
    public void checkForRoundWin() // Executes when player potentially won a round.
    {
        int winnerIndex = 0;
        if (numOfPlayersAlive == 1) // Check for winning player if there is only 1 player left.
        {
            for (int p = 0; p < playerIsDead.Length; p++)
            {
                if (playerIsDead[p] == false)
                {
                    winnerIndex = p;
                    break;
                }
            }
            combatController[] combatants = FindObjectsOfType<combatController>();
            foreach (combatController combat in combatants)
            {
                combat.isDead = false;
            }
            playerScore[winnerIndex]++;
            checkGameWin();
            // Begin round and freeze player movement until players
            // spawn in and round starts.
            if (!gameOver)
            {
                currentRound++;
                currentRoundState = roundState.roundStart;
                playWinSound(roundWinSound[winnerIndex]);
                controlAllMovement(false, true);
                roundStateController();
            }
        }
    }
    public void checkGameWin()
    {
        int winnerIndex = 0;
        if (playerScore[0] == scoreToWin)
        {
            winnerIndex = 0;
            Debug.Log("Player " + winnerIndex + " won the game!");
            gameOver = true;
            swagControllers[winnerIndex].equipCrown();
            manager.enablePlayButton();
            playWinSound(gameWinSound[winnerIndex]);
        }
        if (playerScore[1] == scoreToWin)
        {
            winnerIndex = 1;
            Debug.Log("Player " + winnerIndex + " won the game!");
            gameOver = true;
            swagControllers[winnerIndex].equipCrown();
            manager.enablePlayButton();
            playWinSound(gameWinSound[winnerIndex]);
        }
        if (playerScore.Length == 2)
            return;
        if (playerScore[2] == scoreToWin)
        {
            winnerIndex = 2;
            Debug.Log("Player " + winnerIndex + " won the game!");
            gameOver = true;
            swagControllers[winnerIndex].equipCrown();
            manager.enablePlayButton();
            playWinSound(gameWinSound[winnerIndex]);
        }
        if (playerScore.Length == 3)
            return;
        if (playerScore[3] == scoreToWin)
        {
            winnerIndex = 3;
            Debug.Log("Player " + winnerIndex + " won the game!");
            gameOver = true;
            swagControllers[winnerIndex].equipCrown();
            manager.enablePlayButton();
            playWinSound(gameWinSound[winnerIndex]);
        }
    }
    // Changes Y-Value in player controller script so that look rotation works.
    public void changeYValue(float yValue)
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].yValue = yValue;
        }
    }
    public void controlAllMovement(bool enable, bool disable) // Quick disable all player movement.
    {
        if (enable)
            for (int i = 0; i < players.Length; i++)
            {
                players[i].canMove = true;
            }

        else if (disable)
            for (int i = 0; i < players.Length; i++)
            {
                players[i].canMove = false;
            }
    }
    public void freezeControl(bool enable) // Quick disable all player control.
    {
        if (enable)
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] == null)
                    break;
                players[i].canControl = false;
            }
        else if (!enable)
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] == null)
                    break;
                players[i].canControl = true;
            }
    }
    public void slipperyControl(bool enable) // Quick disable all player control.
    {
        if (enable)
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] == null)
                    break;
                players[i].smoothInputSpeed = 0.75f;
            }
        else if (!enable)
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] == null)
                    break;
                players[i].smoothInputSpeed = 0.2f;
            }
    }
    public void debugResetAllWeapons()
    {
        weapon[] allActiveWeapons = FindObjectsOfType<weapon>();
        foreach (weapon userweapon in allActiveWeapons)
        {
            userweapon.isEquippable = true;
            userweapon.enableMesh();
            if (userweapon.itemParticle.isStopped)
            {
                userweapon.itemParticle.Play();
            }
        }
    }
    public void replayGame()
    {
        if (gameOver)
        {
            manager.disablePlayButton();
            gameOver = false;
            for (int score = 0; score < playerScore.Length; score++)
            {
                playerScore[score] = 0;
            }
            foreach (playerSwagController swag in swagControllers)
            {
                swag.crown.SetActive(false);
            }
            currentRound = 1;
            StopCoroutine("resetRound");
            currentRoundState = roundState.gameBegin;
            roundStateController();
        }
    }
    void OnPlayerJoined(PlayerInput playerInput)
    {
        // Give each player a unique ID based on the order in which they joined.
        playerInput.gameObject.GetComponent<PlayerController>().playerID = playerInput.playerIndex;
        // Players spawn in unique spawn points depending on the order in which they joined.
        playerInput.gameObject.GetComponent<PlayerController>().startPos = playerSpawnsRound1[playerInput.playerIndex].position;
        // Update the current player count based on how many players have joined (+ 1 due to array index starting at 0).
        playerCount = playerInput.playerIndex + 1;
        playerInput.gameObject.name = ("Player " + playerInput.playerIndex);  // Name GameObject based on player index.
        playWinSound(spawnSound);
    }
    public void addDummy()
    {
        if (playerCount < 4)
        {
            GameObject dummyPlayer = Instantiate(playerPrefab);
            PlayerInput pInput = dummyPlayer.GetComponent<PlayerInput>();
            PlayerController pController = dummyPlayer.GetComponent<PlayerController>();
            pController.playerID = pInput.playerIndex;
            pController.startPos = playerSpawnsRound1[pInput.playerIndex].position;
            playerCount = pInput.playerIndex + 1;
        }
    }
    public void playWinSound(AudioClip clipSound)
    {
        source.Stop();
        source.clip = clipSound;
        source.Play();
    }
    public void playSound(AudioClip clipSound)
    {
        source.Stop();
        source.clip = clipSound;
        source.Play();
    }
}
