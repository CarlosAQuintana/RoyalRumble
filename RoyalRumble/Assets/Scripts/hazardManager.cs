using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hazardManager : MonoBehaviour
{
    public GameObject fireArrow;
    public GameObject breathEffect;
    public GameObject fireHitbox;

    public float hazardTimer;
    public float blinkResetTimer;
    public float blinkTimer;
    public float blinkCounter;
    public float fireAgain;

    public bool arrowBlinking;

    public float arrowPosSet;
    public Transform arrowPos1;
    public Transform arrowPos2;
    public Transform arrowPos3;
    public Transform arrowPos4;

    public GameObject iceLane;
    public GameObject icicleSpike;

    public float iceLaneSet;
    public Transform iceLane1;
    public Transform iceLane2;
    public Transform iceLane3;
    public Transform iceLane4;
    public Transform iceLane5;
    public Transform iceLane6;
    public Transform iceLane7;
    public Transform iceLane8;
    public Transform iceLane9;
    public Transform iceLane10;

    public Transform iceLaunch1;
    public Transform iceLaunch2;
    public Transform iceLaunch3;
    public Transform iceLaunch4;
    public Transform iceLaunch5;
    public Transform iceLaunch6;
    public Transform iceLaunch7;
    public Transform iceLaunch8;
    public Transform iceLaunch9;
    public Transform iceLaunch10;

    public GameObject sandHazard;
    public Transform sandRise1;
    public Transform sandRise2;
    public Transform sandRise3;
    public Transform sandRise4;


    // Start is called before the first frame update
    void Start()
    {
        fireArrow.SetActive(false);
        arrowBlinking = true;
        blinkTimer = 0.25f;
        blinkResetTimer = 0.25f;
        fireAgain = 5f;

        iceLane.SetActive(false);
        
        GenerateFireBreathPos();
        GenerateIceLanePos();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject gameManager = GameObject.FindWithTag("gameManager");  //Gets the hazard and round manager scripts
        hazardManager hM = gameManager.GetComponent<hazardManager>();
        roundManager rM = gameManager.GetComponent<roundManager>();

        if(rM.RoundTimeElasped > 30)  //Gets the time since the round started, starts hazard if longer than 30 seconds
        {
            FireBlast();
        }
        
        hazardTimer += Time.deltaTime;  //Uses internal hazard timer, switch to rM timer eventually
        if(hazardTimer > 5)
        {  
           IcicleShot();
           SandRising();
        }
    }

    
    
    void FireBlast()  //Function that makes the arrow indicator before the fire blast blink
    {
        if(arrowBlinking == true)  //Begins the loop for the arrow blinking
        {
           if(hazardTimer < 25)
            {
                fireAgain = 5f;
            }
            else if (hazardTimer < 35)
            {
                fireAgain = 2.5f;
            }
            else if (hazardTimer < 45)
            {
                fireAgain = 1.15f;
            }
            else
            {
                fireAgain = 1;
            }

            if(blinkTimer < 0.55f)  //Makes arrow active
            {
                fireArrow.SetActive(true);  //Sets arrow to active
                blinkTimer -= Time.deltaTime;  //Begins countdown to inactive
            }

            if(blinkTimer < 0) //Makes arrow inactive
            {
                fireArrow.SetActive(false);  //Sets arrow to inactive
                blinkResetTimer -= Time.deltaTime;  //Begins countdown to active

                if(blinkResetTimer < 0)  //Resets both timers to make arrow active again
                {
                    blinkTimer = 0.5f; 
                    blinkResetTimer = 0.5f;
                    blinkCounter += 1;  //Adds one to the amount of times the arrow has blinked
                }
            }

            if(blinkCounter == 5)  //Tracks amount of times arrow has blinked
            {
                if(arrowPosSet == 1)   //Orients the hitbox to the direction of the arrow, and plays the fire particle effect
                {
                    Instantiate(breathEffect, arrowPos1.position, arrowPos1.rotation);
                    Instantiate(fireHitbox, arrowPos1.position, arrowPos1.rotation);
                }
                if(arrowPosSet == 2)
                {
                    Instantiate(breathEffect, arrowPos2.position, arrowPos2.rotation);
                    Instantiate(fireHitbox, arrowPos2.position, arrowPos2.rotation);
                }
                if(arrowPosSet == 3)
                {
                    Instantiate(breathEffect, arrowPos3.position, arrowPos3.rotation);
                    Instantiate(fireHitbox, arrowPos3.position, arrowPos3.rotation);
                }
                if(arrowPosSet == 4)
                {
                    Instantiate(breathEffect, arrowPos4.position, arrowPos4.rotation);
                    Instantiate(fireHitbox, arrowPos4.position, arrowPos4.rotation);
                }

                blinkTimer = 0.6f;  //Stops the arrow loop from happening
                if(blinkTimer == 0.6f)
                {
                    fireArrow.SetActive(false);  //Keeps the arrow inactive until needed again
                    arrowBlinking = false;
                }
            }
        }

        if(arrowBlinking == false)  //Gets all the timers and counters ready for the next blast
        {
            blinkTimer = 0.5f;
            blinkResetTimer = 0.5f;
            blinkCounter = 0;
            fireAgain -= Time.deltaTime;  //Begins countdown for the break inbetween blasts

            if(fireAgain < 0)
            {
                GenerateFireBreathPos();  //Sets new arrow position
                arrowBlinking = true;  //Begins loop cycle again
            }
        }
    }

    void IcicleShot()  //Fires the icicle from a random point on the Ice Stage
    {
        if(arrowBlinking == true)  //Begins the loop for the lane blinking
        {
            if(hazardTimer < 25)  //Sets the speed of the hazard based on how long the round has gone on for
            {
                fireAgain = 5f;
            }
            else if (hazardTimer < 35)
            {
                fireAgain = 2.5f;
            }
            else if (hazardTimer < 45)
            {
                fireAgain = 1.15f;
            }
            else
            {
                fireAgain = 0.5f;  //After 45 seconds, the speed of the hazard caps out and just keeps firing
            }
            
            if(blinkTimer < 0.26f)  //Makes lane active
            {
                iceLane.SetActive(true);  //Sets lane to active
                blinkTimer -= Time.deltaTime;  //Begins countdown to inactive
            }

            if(blinkTimer < 0) //Makes arrow inactive
            {
                iceLane.SetActive(false);  //Sets lane to inactive
                blinkResetTimer -= Time.deltaTime;  //Begins countdown to active

                if(blinkResetTimer < 0)  //Resets both timers to make arrow active again
                {
                    blinkTimer = 0.25f; 
                    blinkResetTimer = 0.25f;
                    blinkCounter += 1;  //Adds one to the amount of times the lane has blinked
                }
            }

            if(blinkCounter == 5)  //Tracks amount of times lane has blinked
            { 
                if(iceLaneSet == 1)  //Fires an icicle at the appropriate position
                {
                    Instantiate(icicleSpike, iceLaunch1.position, iceLaunch1.rotation);
                }
                if(iceLaneSet == 2)
                {
                    Instantiate(icicleSpike, iceLaunch2.position, iceLaunch2.rotation);
                }
                if(iceLaneSet == 3)
                {
                    Instantiate(icicleSpike, iceLaunch3.position, iceLaunch3.rotation);
                }
                if(iceLaneSet == 4)
                {
                    Instantiate(icicleSpike, iceLaunch4.position, iceLaunch4.rotation);
                }
                if(iceLaneSet == 5)
                {
                    Instantiate(icicleSpike, iceLaunch5.position, iceLaunch5.rotation);
                }
                if(iceLaneSet == 6)
                {
                    Instantiate(icicleSpike, iceLaunch6.position, iceLaunch6.rotation);
                }
                if(iceLaneSet == 7)
                {
                    Instantiate(icicleSpike, iceLaunch7.position, iceLaunch7.rotation);
                }
                if(iceLaneSet == 8)
                {
                    Instantiate(icicleSpike, iceLaunch8.position, iceLaunch8.rotation);
                }
                if(iceLaneSet == 9)
                {
                    Instantiate(icicleSpike, iceLaunch9.position, iceLaunch9.rotation);
                }
                if(iceLaneSet == 10)
                {
                    Instantiate(icicleSpike, iceLaunch10.position, iceLaunch10.rotation);
                }
               

                blinkTimer = 0.6f;  //Stops the arrow loop from happening
                if(blinkTimer == 0.6f)
                {
                    iceLane.SetActive(false);  //Keeps the arrow inactive until needed again
                    arrowBlinking = false;
                }
            }
        }

        if(arrowBlinking == false)  //Gets all the timers and counters ready for the next blast
        {
            blinkTimer = 0.25f;
            blinkResetTimer = 0.25f;
            blinkCounter = 0;
            fireAgain -= Time.deltaTime;  //Begins countdown for the break inbetween blasts

            if(fireAgain < 0)
            {
                GenerateIceLanePos();  //Sets new arrow position
                arrowBlinking = true;  //Begins loop cycle again
            }
        }
    }

    void SandRising()
    {
        if(hazardTimer > 15)
        {
            sandHazard.transform.position = sandRise4.transform.position;
        }
        if(hazardTimer > 30)
        {
            sandHazard.transform.position = sandRise3.transform.position;
        }
        if(hazardTimer > 40)
        {
            sandHazard.transform.position = sandRise2.transform.position;
        }
        if(hazardTimer > 50)
        {
            sandHazard.transform.position = sandRise1.transform.position;
        }
    }





    void GenerateFireBreathPos()  //Function that randomly generates the arrow position randomly
    {
        arrowPosSet = Random.Range(1, 5);

        if(arrowPosSet == 1)
        {
            fireArrow.transform.position = arrowPos1.transform.position;
        }

        if(arrowPosSet == 2)
        {
            fireArrow.transform.position = arrowPos2.transform.position;
        }

        if(arrowPosSet == 3)
        {
            fireArrow.transform.position = arrowPos3.transform.position;
        }

        if(arrowPosSet == 4)
        {
            fireArrow.transform.position = arrowPos4.transform.position;
        }

        Debug.Log("arrow is at position" + arrowPosSet);
    }

    void GenerateIceLanePos() //Functions that generates and sets where the next icicle is going to come from
    {
        iceLaneSet = Random.Range(1, 11);

        if(iceLaneSet == 1)
        {
            iceLane.transform.position = iceLane1.transform.position;
        }
        if(iceLaneSet == 2)
        {
            iceLane.transform.position = iceLane2.transform.position;
        }
        if(iceLaneSet == 3)
        {
            iceLane.transform.position = iceLane3.transform.position;
        }
        if(iceLaneSet == 4)
        {
            iceLane.transform.position = iceLane4.transform.position;
        }
        if(iceLaneSet == 5)
        {
            iceLane.transform.position = iceLane5.transform.position;
        }
        if(iceLaneSet == 6)
        {
            iceLane.transform.position = iceLane6.transform.position;
        }
        if(iceLaneSet == 7)
        {
            iceLane.transform.position = iceLane7.transform.position;
        }
        if(iceLaneSet == 8)
        {
            iceLane.transform.position = iceLane8.transform.position;
        }
        if(iceLaneSet == 9)
        {
            iceLane.transform.position = iceLane9.transform.position;
        }
        if(iceLaneSet == 10)
        {
            iceLane.transform.position = iceLane10.transform.position;
        }
        Debug.Log("icicle will come from pos" + iceLaneSet);
    }
}
