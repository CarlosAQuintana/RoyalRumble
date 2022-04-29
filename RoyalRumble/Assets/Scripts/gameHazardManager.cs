using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameHazardManager : MonoBehaviour
{
    public roundManager round;
    public gameManager game;

    [Header("Castle Hazards")]
    public GameObject warningMarkerLeft;
    public GameObject warningMarkerRight;
    public bool warningLeftActivated;
    public bool warningRightActivated;
    public bool doubleActivation;
    public bool sweeperOneSpawned;
    public bool sweeperTwoSpawned;
    public bool sweeperThreeSpawned;
    public GameObject leftFireSweeper;
    public GameObject rightFireSweeper;

    [Header("Jungle Hazards")]
    public GameObject sand;
    public Transform sandEndPoint;
    public bool rise;

    void Start()
    {
        game = GetComponent<gameManager>();
        round = GetComponent<roundManager>();
    }
    void Update()
    {
        switch (round.currentLevel)
        {
            case roundManager.level.castle:
                castleHazard();
                break;
            case roundManager.level.ice:
                iceHazard();
                break;
            case roundManager.level.jungle:
                jungleHazard();
                break;
            case roundManager.level.fire:
                fireHazard();
                break;
        }
        resetHazards();
    }
    public void castleHazard()
    {
        if (round.RoundTimeElasped == 0)
            return;
        if (round.RoundTimeElasped >= 15)
        {
            if (!warningLeftActivated)
            {
                warningLeftActivated = true;
                warningMarkerLeft.SetActive(true);
            }
        }
        if (round.RoundTimeElasped >= 20)
        {
            if (!sweeperOneSpawned)
            {
                sweeperOneSpawned = true;
                GameObject sweeper = Instantiate(leftFireSweeper, Vector3.zero, Quaternion.identity);
            }
        }
        if (round.RoundTimeElasped >= 35)
        {
            if (!warningRightActivated)
            {
                warningRightActivated = true;
                warningMarkerRight.SetActive(true);
            }
        }
        if (round.RoundTimeElasped >= 40)
        {
            if (!sweeperOneSpawned)
            {
                sweeperOneSpawned = true;
                GameObject sweeper = Instantiate(rightFireSweeper, Vector3.zero, Quaternion.identity);
            }
        }

        if (round.RoundTimeElasped >= 55)
        {
            if (!doubleActivation)
            {
                doubleActivation = true;
                warningMarkerLeft.SetActive(true);
                warningMarkerRight.SetActive(true);
            }
        }
        if (round.RoundTimeElasped >= 60)
        {
            if (!sweeperOneSpawned)
            {
                sweeperOneSpawned = true;
                GameObject sweeper = Instantiate(leftFireSweeper, Vector3.zero, Quaternion.identity);
            }
        }
    }
    public void iceHazard()
    {
        if (round.RoundTimeElasped == 0)
            return;
    }
    public void jungleHazard()
    {
        if (round.RoundTimeElasped == 0)
            return;
    }
    public void fireHazard()
    {
        if (round.RoundTimeElasped == 0)
            return;
    }
    public void resetHazards()
    {
        if (!round.TrackRound)
        {
            // Reset Castle Hazards.
            sweeperOneSpawned = false;
            sweeperTwoSpawned = false;
            sweeperThreeSpawned = false;
            warningLeftActivated = false;
            warningRightActivated = false;
            doubleActivation = false;
            warningMarkerLeft.SetActive(false);
            warningMarkerRight.SetActive(false);
            // Reset Ice Hazards.
            // Reset Jungle Hazards.
            // Reset Fire Hazards.
        }
    }
}
