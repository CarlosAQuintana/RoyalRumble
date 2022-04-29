using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameHazardManager : MonoBehaviour
{
    public roundManager round;
    public gameManager game;

    [Header("Castle Hazards")]
    public bool sweeperSpawned;
    public GameObject leftFireSweeper;
    public GameObject rightFireSweeper;
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
        if (round.RoundTimeElasped >= 5)
        {
            if (!sweeperSpawned)
            {
                sweeperSpawned = true;
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
            sweeperSpawned = false;
        }
    }
}
