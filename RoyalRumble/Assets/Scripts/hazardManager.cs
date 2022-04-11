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
    public Transform firePos1;
    public Transform firePos2;
    public Transform firePos3;
    public Transform firePos4;


    // Start is called before the first frame update
    void Start()
    {
        fireArrow.SetActive(false);
        arrowBlinking = true;
        blinkTimer = 0.5f;
        blinkResetTimer = 0.5f;
        fireAgain = 5f;
        
        GenerateArrowPos();
    }

    // Update is called once per frame
    void Update()
    {
        hazardTimer += Time.deltaTime;

        if(hazardTimer > 10)
        {
           ArrowBlink();
        }
    }

    
    
    void ArrowBlink()  //Function that makes the arrow indicator before the fire blast blink
    {
        if(arrowBlinking == true)  //Begins the loop for the arrow blinking
        {
            fireAgain = 5f;  //Sets timer for the break in between blasts
            
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
                    Instantiate(breathEffect, firePos3.position, firePos3.rotation);
                    Instantiate(fireHitbox, arrowPos1.position, arrowPos1.rotation);
                }
                if(arrowPosSet == 2)
                {
                    Instantiate(breathEffect, firePos4.position, firePos4.rotation);
                    Instantiate(fireHitbox, arrowPos2.position, arrowPos2.rotation);
                }
                if(arrowPosSet == 3)
                {
                    Instantiate(breathEffect, firePos1.position, firePos1.rotation);
                    Instantiate(fireHitbox, arrowPos3.position, arrowPos3.rotation);
                }
                if(arrowPosSet == 4)
                {
                    Instantiate(breathEffect, firePos2.position, firePos2.rotation);
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
                GenerateArrowPos();  //Sets new arrow position
                arrowBlinking = true;  //Begins loop cycle again
            }
        }
    }





    void GenerateArrowPos()  //Function that randomly generates the arrow position randomly
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
}
