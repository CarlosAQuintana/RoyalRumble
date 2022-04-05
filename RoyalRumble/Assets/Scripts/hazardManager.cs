using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hazardManager : MonoBehaviour
{
    public GameObject fireArrow;
    public GameObject breathEffect;

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

    // Start is called before the first frame update
    void Start()
    {
        fireArrow.SetActive(false);
        arrowBlinking = true;
        blinkTimer = 0.5f;
        blinkResetTimer = 0.5f;
        fireAgain = 2f;
        
        GenerateArrowPos();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject roundManager = GameObject.FindWithTag("gameManager");
        roundManager rManager = roundManager.GetComponent<roundManager>();

        hazardTimer += Time.deltaTime;

        if(hazardTimer > 3)
        {
           ArrowBlink();
        }
    }

    
    
    void ArrowBlink()  //Function that makes the arrow indicator before the fire blast blink
    {
        if(arrowBlinking == true)
        {
            fireAgain = 2f;
            
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
                if(arrowPosSet == 1)
                {
                    Instantiate(breathEffect, arrowPos1.position, arrowPos1.rotation);
                }
                if(arrowPosSet == 2)
                {
                    Instantiate(breathEffect, arrowPos2.position, arrowPos2.rotation);
                }
                if(arrowPosSet == 3)
                {
                    Instantiate(breathEffect, arrowPos3.position, arrowPos3.rotation);
                }
                if(arrowPosSet == 4)
                {
                    Instantiate(breathEffect, arrowPos4.position, arrowPos4.rotation);
                }

                blinkTimer = 0.6f;  //Stops the arrow loop from happening
                if(blinkTimer == 0.6f)
                {
                    fireArrow.SetActive(false);  //Keeps the arrow inactive until needed again
                    arrowBlinking = false;
                }
            }
        }

        if(arrowBlinking == false)
        {
            GenerateArrowPos();
            blinkTimer = 0.5f;
            blinkResetTimer = 0.5f;
            blinkCounter = 0;
            fireAgain -= Time.deltaTime;

            if(fireAgain < 0)
            {
                arrowBlinking = true;
            }
        }
    }





    void GenerateArrowPos()  //Function that randomly generates the arrow position randomly
    {
        arrowPosSet = Random.Range(0, 5);

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
