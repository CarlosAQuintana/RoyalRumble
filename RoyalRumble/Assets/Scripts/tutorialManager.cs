using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class tutorialManager : MonoBehaviour
{
    public Transform spawnPoint;
    PlayerInputManager InputManager;
    PlayerController tutorialPlayer;
    void Start()
    {
        InputManager = GetComponent<PlayerInputManager>();
    }
    void Update()
    {

    }
    void OnPlayerJoined(PlayerInput playerInput)
    {
        InputManager.DisableJoining();
        playerInput.gameObject.GetComponent<PlayerController>().startPos = spawnPoint.transform.position;
        combatController spawnedCombatant = playerInput.gameObject.GetComponent<combatController>();
        spawnedCombatant.inTutorial = true;
        PlayerController spawnedPLayer = playerInput.gameObject.GetComponent<PlayerController>();
        spawnedPLayer.playerID = playerInput.playerIndex;
        tutorialPlayer = spawnedPLayer;
        playerInput.gameObject.name = ("Player " + playerInput.playerIndex);
        StartCoroutine("quickEnableControl");
    }
    public IEnumerator quickEnableControl()
    {
        yield return new WaitForSeconds(0.25f);
        tutorialPlayer.canMove = true;
        tutorialPlayer.canControl = true;
    }
}
