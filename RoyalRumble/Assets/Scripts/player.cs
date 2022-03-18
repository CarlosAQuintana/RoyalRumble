using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class player : MonoBehaviour
{
    PlayerInput input;
    roundManager roundManager;
    public int playerIndex;
    public bool isCPU;
    void Start()
    {
        input = GetComponent<PlayerInput>();
        roundManager = FindObjectOfType<roundManager>();
        if (isCPU)
        {

        }
        playerIndex = input.playerIndex;
        gameObject.name = ("Player " + playerIndex);
        transform.position = roundManager.playerSpawns[playerIndex].position;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
