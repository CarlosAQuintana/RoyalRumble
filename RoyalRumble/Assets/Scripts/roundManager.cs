using UnityEngine;
using UnityEngine.InputSystem;

public class roundManager : MonoBehaviour
{

    public enum roundState { roundStart, roundPlay, roundEnd } // Each state a round can be in.
    [Header("Game Variables")]
    public PlayerController[] players;

    [Header("Round Variables")]
    public roundState currentRoundState;
    public int currentRound; // Current round.
    public int maxRounds; // Set the max number of rounds wanted for gameplay.
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
    }

    void Update()
    {
        // I added the ability to prevent players from joining
        // while the game is being played.
        if (manager.playGame)
        {
            playerManager.DisableJoining();
        }
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

        players[playerInput.playerIndex] = playerInput.gameObject.GetComponent<PlayerController>();
    }
}
