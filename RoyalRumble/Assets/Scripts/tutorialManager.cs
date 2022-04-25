using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class tutorialManager : MonoBehaviour
{
    public Transform spawnPoint;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnPlayerJoined(PlayerInput playerInput) // Called whenever a new player joins.
    {
        PlayerController spawnedPLayer = playerInput.gameObject.GetComponent<PlayerController>();
        spawnedPLayer.gameObject.transform.position = spawnPoint.transform.position;
        spawnedPLayer.playerID = playerInput.playerIndex;
        spawnedPLayer.canMove = true;
        spawnedPLayer.canControl = true;
        playerInput.gameObject.name = ("Player " + playerInput.playerIndex);
    }

}
