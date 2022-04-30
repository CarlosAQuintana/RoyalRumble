using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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

    public float vertblinkResetTimer;
    public float vertblinkTimer;
    public float vertblinkCounter;
    public float vertfireAgain;

    public bool arrowBlinking;
    public bool vertarrowBlinking;

    public float arrowPosSet;
    public Transform arrowPos1;
    public Transform arrowPos2;
    public Transform arrowPos3;
    public Transform arrowPos4;
    public Transform firePos1;
    public Transform firePos2;
    public Transform firePos3;
    public Transform firePos4;

    [Header("Ice Variables")]
    public GameObject iceLane;
    public GameObject iceLaneVert;
    public GameObject icicleSpike;
    public GameObject vertIcicleSpike;

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

    public float vertIceLaneSet;
    public Transform iceLane1v;
    public Transform iceLane2v;
    public Transform iceLane3v;
    public Transform iceLane4v;
    public Transform iceLane5v;

    public Transform iceLaunch1v;
    public Transform iceLaunch2v;
    public Transform iceLaunch3v;
    public Transform iceLaunch4v;
    public Transform iceLaunch5v;

    public Transform iceLaunch1;
    public Transform iceLaunch2;
    public Transform iceLaunch3;
    public Transform iceLaunch4;
    public Transform iceLaunch5;
    public Transform iceLaunch6;
    public Transform iceLaunch7;
    public Transform iceLaunch8;
    public Transform iceLaunch9;

    [Header("Sand Variables")]
    public GameObject sandHazard;
    public Transform sandRise1;
    public Transform sandRise2;
    public Transform sandRise3;
    public Transform sandRise4;
    public float shakeTimer;
    public float reshakeTimer;
    private bool cancamshake;

    [Header("Fire Variable")]
    public float lavaHere;
    public Transform lavaSpot1;
    public Transform lavaSpot2;
    public Transform lavaSpot3;
    public Transform lavaSpot4;
    public Transform lavaSpot5;

    public Transform lavaFire1;
    public Transform lavaFire2;
    public Transform lavaFire3;
    public Transform lavaFire4;
    public Transform lavaFire5;

    public GameObject showLava;
    public GameObject lavaBall;

    [Header("UI Stuff")]
    public GameObject exclam1;
    public GameObject exclam2;
    public GameObject exclam3;
    public Transform hazardnotifPos;


    // Start is called before the first frame update
    void Start()
    {
        fireArrow.SetActive(false);
        arrowBlinking = true;
        blinkTimer = 0.25f;
        blinkResetTimer = 0.25f;
        fireAgain = 5f;
        shakeTimer = 4f;
        reshakeTimer = 5f;

        vertblinkTimer = 0.25f;
        vertblinkResetTimer = 0.25f;
        vertfireAgain = 5f;
        vertarrowBlinking = true;

        iceLane.SetActive(false);
        iceLaneVert.SetActive(false);
        showLava.SetActive(false);

        GenerateFireBreathPos();
        GenerateIceLanePos();
        GenerateVertIceLanePos();
        GenerateLavaSpot();

        exclam1.SetActive(false);
        exclam2.SetActive(false);
        exclam3.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        GameObject gameManager = GameObject.FindWithTag("gameManager");  //Gets the hazard and round manager scripts
        hazardManager hM = gameManager.GetComponent<hazardManager>();
        roundManager rM = gameManager.GetComponent<roundManager>();

        if (rM.RoundTimeElasped > 20)  //Gets the time since the round started, starts hazard if longer than 20 seconds
        {
            if (rM.currentLevel == roundManager.level.castle)
            {
                //FireBlast();
            }
            if (rM.currentLevel == roundManager.level.ice)
            {
                IcicleShot();
            }
            if (rM.currentLevel == roundManager.level.jungle)
            {
                SandRising();
            }
            if (rM.currentLevel == roundManager.level.fire)
            {
                LavaHazard();
            }
        }

        if (rM.RoundTimeElasped > 60)
        {
            if (rM.currentLevel == roundManager.level.ice)
            {
                VerticalIcicleShot();
            }
        }

        hazardTimer += Time.deltaTime;
        if (hazardTimer > 20)
        {
            IcicleShot();
        }
        if (hazardTimer > 60)
        {
            VerticalIcicleShot();
        }

        if (rM.RoundTimeElasped > 16)
        {
            exclam1.SetActive(true);
        }
        if (rM.RoundTimeElasped > 19)
        {
            exclam1.SetActive(false);
        }

        if (rM.RoundTimeElasped > 36)
        {
            exclam2.SetActive(true);
        }
        if (rM.RoundTimeElasped > 39)
        {
            exclam2.SetActive(false);
        }

        if (rM.RoundTimeElasped > 56)
        {
            exclam3.SetActive(true);
        }
        if (rM.RoundTimeElasped > 59)
        {
            exclam3.SetActive(false);
        }


    }
    void FireBlast()  //Function that makes the arrow indicator before the fire blast blink
    {
        GameObject gameManager = GameObject.FindWithTag("gameManager");  //Gets the hazard and round manager scripts
        hazardManager hM = gameManager.GetComponent<hazardManager>();
        roundManager rM = gameManager.GetComponent<roundManager>();
        if (arrowBlinking == true)  //Begins the loop for the arrow blinking
        {
            if (rM.RoundTimeElasped > 40)
            {
                fireAgain = 2.5f;
            }
            if (rM.RoundTimeElasped > 60)
            {
                fireAgain = 1f;
            }

            if (blinkTimer < 0.55f)  //Makes arrow active
            {
                fireArrow.SetActive(true);  //Sets arrow to active
                blinkTimer -= Time.deltaTime;  //Begins countdown to inactive
            }

            if (blinkTimer < 0) //Makes arrow inactive
            {
                fireArrow.SetActive(false);  //Sets arrow to inactive
                blinkResetTimer -= Time.deltaTime;  //Begins countdown to active

                if (blinkResetTimer < 0)  //Resets both timers to make arrow active again
                {
                    blinkTimer = 0.5f;
                    blinkResetTimer = 0.5f;
                    blinkCounter += 1;  //Adds one to the amount of times the arrow has blinked
                }
            }

            if (blinkCounter == 5)  //Tracks amount of times arrow has blinked
            {
                if (arrowPosSet == 1)   //Orients the hitbox to the direction of the arrow, and plays the fire particle effect
                {
                    Instantiate(breathEffect, firePos3.position, firePos3.rotation);
                    Instantiate(fireHitbox, arrowPos1.position, arrowPos1.rotation);
                }
                if (arrowPosSet == 2)
                {
                    Instantiate(breathEffect, firePos4.position, firePos4.rotation);
                    Instantiate(fireHitbox, arrowPos2.position, arrowPos2.rotation);
                }
                if (arrowPosSet == 3)
                {
                    Instantiate(breathEffect, firePos1.position, firePos1.rotation);
                    Instantiate(fireHitbox, arrowPos3.position, arrowPos3.rotation);
                }
                if (arrowPosSet == 4)
                {
                    Instantiate(breathEffect, firePos2.position, firePos2.rotation);
                    Instantiate(fireHitbox, arrowPos4.position, arrowPos4.rotation);
                }

                blinkTimer = 0.6f;  //Stops the arrow loop from happening
                if (blinkTimer == 0.6f)
                {
                    fireArrow.SetActive(false);  //Keeps the arrow inactive until needed again
                    arrowBlinking = false;
                }
            }
        }

        if (arrowBlinking == false)  //Gets all the timers and counters ready for the next blast
        {
            blinkTimer = 0.5f;
            blinkResetTimer = 0.5f;
            blinkCounter = 0;
            fireAgain -= Time.deltaTime;  //Begins countdown for the break inbetween blasts

            if (fireAgain < 0)
            {
                GenerateFireBreathPos();  //Sets new arrow position
                arrowBlinking = true;  //Begins loop cycle again
            }
        }
    }

    void IcicleShot()  //Fires the icicle from a random point on the Ice Stage
    {
        GameObject gameManager = GameObject.FindWithTag("gameManager");  //Gets the hazard and round manager scripts
        hazardManager hM = gameManager.GetComponent<hazardManager>();
        roundManager rM = gameManager.GetComponent<roundManager>();
        if (arrowBlinking == true)  //Begins the loop for the lane blinking
        {
            if (hazardTimer > 40)
            {
                fireAgain = 3f;
            }
            if (hazardTimer > 60)
            {
                fireAgain = 1f;
            }

            if (blinkTimer < 0.26f)  //Makes lane active
            {
                iceLane.SetActive(true);  //Sets lane to active
                blinkTimer -= Time.deltaTime;  //Begins countdown to inactive
            }

            if (blinkTimer < 0) //Makes arrow inactive
            {
                iceLane.SetActive(false);  //Sets lane to inactive
                blinkResetTimer -= Time.deltaTime;  //Begins countdown to active

                if (blinkResetTimer < 0)  //Resets both timers to make arrow active again
                {
                    blinkTimer = 0.25f;
                    blinkResetTimer = 0.25f;
                    blinkCounter += 1;  //Adds one to the amount of times the lane has blinked
                }
            }

            if (blinkCounter == 5)  //Tracks amount of times lane has blinked
            {
                if (iceLaneSet == 1)  //Fires an icicle at the appropriate position
                {
                    Instantiate(icicleSpike, iceLaunch1.position, iceLaunch1.rotation);
                }
                if (iceLaneSet == 2)
                {
                    Instantiate(icicleSpike, iceLaunch2.position, iceLaunch2.rotation);
                }
                if (iceLaneSet == 3)
                {
                    Instantiate(icicleSpike, iceLaunch3.position, iceLaunch3.rotation);
                }
                if (iceLaneSet == 4)
                {
                    Instantiate(icicleSpike, iceLaunch4.position, iceLaunch4.rotation);
                }
                if (iceLaneSet == 5)
                {
                    Instantiate(icicleSpike, iceLaunch5.position, iceLaunch5.rotation);
                }
                if (iceLaneSet == 6)
                {
                    Instantiate(icicleSpike, iceLaunch6.position, iceLaunch6.rotation);
                }
                if (iceLaneSet == 7)
                {
                    Instantiate(icicleSpike, iceLaunch7.position, iceLaunch7.rotation);
                }
                if (iceLaneSet == 8)
                {
                    Instantiate(icicleSpike, iceLaunch8.position, iceLaunch8.rotation);
                }
                if (iceLaneSet == 9)
                {
                    Instantiate(icicleSpike, iceLaunch9.position, iceLaunch9.rotation);
                }

                blinkTimer = 0.6f;  //Stops the arrow loop from happening
                if (blinkTimer == 0.6f)
                {
                    iceLane.SetActive(false);  //Keeps the arrow inactive until needed again
                    arrowBlinking = false;
                }
            }
        }

        if (arrowBlinking == false)  //Gets all the timers and counters ready for the next blast
        {
            blinkTimer = 0.25f;
            blinkResetTimer = 0.25f;
            blinkCounter = 0;
            fireAgain -= Time.deltaTime;  //Begins countdown for the break inbetween blasts

            if (fireAgain < 0)
            {
                GenerateIceLanePos();  //Sets new arrow position
                arrowBlinking = true;  //Begins loop cycle again
            }
        }
    }

    void VerticalIcicleShot()
    {
        GameObject gameManager = GameObject.FindWithTag("gameManager");  //Gets the hazard and round manager scripts
        hazardManager hM = gameManager.GetComponent<hazardManager>();
        roundManager rM = gameManager.GetComponent<roundManager>();
        if (vertarrowBlinking == true)  //Begins the loop for the lane blinking
        {

            if (hazardTimer > 40)
            {
                vertfireAgain = 3f;
            }
            if (hazardTimer > 60)
            {
                vertfireAgain = 1f;
            }


            if (vertblinkTimer < 0.26f)  //Makes lane active
            {
                iceLaneVert.SetActive(true);  //Sets lane to active
                vertblinkTimer -= Time.deltaTime;  //Begins countdown to inactive
            }

            if (vertblinkTimer < 0) //Makes arrow inactive
            {
                iceLaneVert.SetActive(false);  //Sets lane to inactive
                vertblinkResetTimer -= Time.deltaTime;  //Begins countdown to active

                if (vertblinkResetTimer < 0)  //Resets both timers to make arrow active again
                {
                    vertblinkTimer = 0.25f;
                    vertblinkResetTimer = 0.25f;
                    vertblinkCounter += 1;  //Adds one to the amount of times the lane has blinked
                }
            }

            if (vertblinkCounter == 5)  //Tracks amount of times lane has blinked
            {
                if (vertIceLaneSet == 1)
                {
                    Instantiate(vertIcicleSpike, iceLaunch1v.position, iceLaunch1v.rotation);
                }
                if (vertIceLaneSet == 2)
                {
                    Instantiate(vertIcicleSpike, iceLaunch2v.position, iceLaunch2v.rotation);
                }
                if (vertIceLaneSet == 3)
                {
                    Instantiate(vertIcicleSpike, iceLaunch3v.position, iceLaunch3v.rotation);
                }
                if (vertIceLaneSet == 4)
                {
                    Instantiate(vertIcicleSpike, iceLaunch4v.position, iceLaunch4v.rotation);
                }
                if (vertIceLaneSet == 5)
                {
                    Instantiate(vertIcicleSpike, iceLaunch5v.position, iceLaunch5v.rotation);
                }


                vertblinkTimer = 0.6f;  //Stops the arrow loop from happening
                if (vertblinkTimer == 0.6f)
                {
                    iceLaneVert.SetActive(false);  //Keeps the arrow inactive until needed again
                    vertarrowBlinking = false;
                }
            }
        }

        if (vertarrowBlinking == false)  //Gets all the timers and counters ready for the next blast
        {
            vertblinkTimer = 0.25f;
            vertblinkResetTimer = 0.25f;
            vertblinkCounter = 0;
            vertfireAgain -= Time.deltaTime;  //Begins countdown for the break inbetween blasts

            if (vertfireAgain < 0)
            {
                GenerateVertIceLanePos();  //Sets new arrow position
                vertarrowBlinking = true;  //Begins loop cycle again
            }
        }
    }

    void SandRising()
    {
        GameObject gameManager = GameObject.FindWithTag("gameManager");  //Gets the hazard and round manager scripts
        hazardManager hM = gameManager.GetComponent<hazardManager>();
        roundManager rM = gameManager.GetComponent<roundManager>();

        GameObject DesertCam = GameObject.Find("Level 3 Camera");
        CinemachineVirtualCamera CMVC = DesertCam.GetComponent<CinemachineVirtualCamera>();
        CinemachineBasicMultiChannelPerlin CMBMCP = CMVC.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        if (rM.RoundTimeElasped > 20)
        {
            CMBMCP.m_AmplitudeGain = 1f;
        }
        if (rM.RoundTimeElasped > 24)
        {
            CMBMCP.m_AmplitudeGain = 0f;
        }

        if (rM.RoundTimeElasped > 25)
        {
            sandHazard.transform.position = sandRise4.transform.position;
        }

        if (rM.RoundTimeElasped > 40)
        {
            CMBMCP.m_AmplitudeGain = 1f;
        }
        if (rM.RoundTimeElasped > 44)
        {
            CMBMCP.m_AmplitudeGain = 0f;
        }

        if (rM.RoundTimeElasped > 45)
        {
            sandHazard.transform.position = sandRise3.transform.position;
        }

        if (rM.RoundTimeElasped > 50)
        {
            CMBMCP.m_AmplitudeGain = 1f;
        }
        if (rM.RoundTimeElasped > 54)
        {
            CMBMCP.m_AmplitudeGain = 0f;
        }

        if (rM.RoundTimeElasped > 55)
        {
            sandHazard.transform.position = sandRise2.transform.position;
        }

        if (rM.RoundTimeElasped > 60)
        {
            CMBMCP.m_AmplitudeGain = 1f;
        }
        if (rM.RoundTimeElasped > 64)
        {
            CMBMCP.m_AmplitudeGain = 0f;
        }

        if (rM.RoundTimeElasped > 65)
        {
            sandHazard.transform.position = sandRise1.transform.position;
        }
    }

    void LavaHazard()
    {
        GameObject gameManager = GameObject.FindWithTag("gameManager");  //Gets the hazard and round manager scripts
        hazardManager hM = gameManager.GetComponent<hazardManager>();
        roundManager rM = gameManager.GetComponent<roundManager>();
        if (arrowBlinking == true)  //Begins the loop for the arrow blinking
        {
            if (rM.RoundTimeElasped > 40)
            {
                fireAgain = 3f;
            }
            if (rM.RoundTimeElasped > 60)
            {
                fireAgain = 1f;
            }

            if (blinkTimer < 0.55f)  //Makes arrow active
            {
                showLava.SetActive(true);  //Sets arrow to active
                blinkTimer -= Time.deltaTime;  //Begins countdown to inactive
            }

            if (blinkTimer < 0) //Makes arrow inactive
            {
                showLava.SetActive(false);  //Sets arrow to inactive
                blinkResetTimer -= Time.deltaTime;  //Begins countdown to active

                if (blinkResetTimer < 0)  //Resets both timers to make arrow active again
                {
                    blinkTimer = 0.5f;
                    blinkResetTimer = 0.5f;
                    blinkCounter += 1;  //Adds one to the amount of times the arrow has blinked
                }
            }

            if (blinkCounter == 5)  //Tracks amount of times arrow has blinked
            {
                if (lavaHere == 1)
                {
                    Instantiate(lavaBall, lavaFire1.position, lavaFire1.rotation);
                }
                if (lavaHere == 2)
                {
                    Instantiate(lavaBall, lavaFire2.position, lavaFire2.rotation);
                }
                if (lavaHere == 3)
                {
                    Instantiate(lavaBall, lavaFire3.position, lavaFire3.rotation);
                }
                if (lavaHere == 4)
                {
                    Instantiate(lavaBall, lavaFire4.position, lavaFire4.rotation);
                }
                if (lavaHere == 5)
                {
                    Instantiate(lavaBall, lavaFire5.position, lavaFire5.rotation);
                }

                blinkTimer = 0.6f;  //Stops the arrow loop from happening
                if (blinkTimer == 0.6f)
                {
                    iceLane.SetActive(false);  //Keeps the arrow inactive until needed again
                    arrowBlinking = false;
                }
            }
        }

        if (arrowBlinking == false)  //Gets all the timers and counters ready for the next blast
        {
            blinkTimer = 0.5f;
            blinkResetTimer = 0.5f;
            blinkCounter = 0;
            fireAgain -= Time.deltaTime;  //Begins countdown for the break inbetween blasts

            if (fireAgain < 0)
            {
                GenerateLavaSpot();  //Sets new arrow position
                arrowBlinking = true;  //Begins loop cycle again
            }
        }

        if (rM.RoundTimeElasped > 20)
        {
            GameObject floor1 = GameObject.Find("moveGround1");
            Rigidbody f1rb = floor1.GetComponent<Rigidbody>();
            f1rb.velocity = new Vector3(0, -0.5f, 0);
        }

        if (rM.RoundTimeElasped > 40)
        {
            GameObject floor2 = GameObject.Find("moveGround2");
            Rigidbody f2rb = floor2.GetComponent<Rigidbody>();
            f2rb.velocity = new Vector3(0, -0.5f, 0);
        }

        if (rM.RoundTimeElasped > 60)
        {
            GameObject floor3 = GameObject.Find("moveGround3");
            Rigidbody f3rb = floor3.GetComponent<Rigidbody>();
            f3rb.velocity = new Vector3(0, -0.5f, 0);
        }
    }





    void GenerateFireBreathPos()  //Function that randomly generates the arrow position randomly
    {
        arrowPosSet = Random.Range(1, 5);

        if (arrowPosSet == 1)
        {
            fireArrow.transform.position = arrowPos1.transform.position;
            fireArrow.transform.rotation = arrowPos1.transform.rotation;
        }

        if (arrowPosSet == 2)
        {
            fireArrow.transform.position = arrowPos2.transform.position;
            fireArrow.transform.rotation = arrowPos2.transform.rotation;
        }

        if (arrowPosSet == 3)
        {
            fireArrow.transform.position = arrowPos3.transform.position;
            fireArrow.transform.rotation = arrowPos3.transform.rotation;
        }

        if (arrowPosSet == 4)
        {
            fireArrow.transform.position = arrowPos4.transform.position;
            fireArrow.transform.rotation = arrowPos4.transform.rotation;
        }

        Debug.Log("arrow is at position" + arrowPosSet);
    }

    void GenerateIceLanePos() //Functions that generates and sets where the next icicle is going to come from
    {
        iceLaneSet = Random.Range(1, 10);

        if (iceLaneSet == 1)
        {
            iceLane.transform.position = iceLane1.transform.position;
        }
        if (iceLaneSet == 2)
        {
            iceLane.transform.position = iceLane2.transform.position;
        }
        if (iceLaneSet == 3)
        {
            iceLane.transform.position = iceLane3.transform.position;
        }
        if (iceLaneSet == 4)
        {
            iceLane.transform.position = iceLane4.transform.position;
        }
        if (iceLaneSet == 5)
        {
            iceLane.transform.position = iceLane5.transform.position;
        }
        if (iceLaneSet == 6)
        {
            iceLane.transform.position = iceLane6.transform.position;
        }
        if (iceLaneSet == 7)
        {
            iceLane.transform.position = iceLane7.transform.position;
        }
        if (iceLaneSet == 8)
        {
            iceLane.transform.position = iceLane8.transform.position;
        }
        if (iceLaneSet == 9)
        {
            iceLane.transform.position = iceLane9.transform.position;
        }
        Debug.Log("icicle will come from pos" + iceLaneSet);
    }

    void GenerateVertIceLanePos()
    {
        vertIceLaneSet = Random.Range(1, 6);

        if (vertIceLaneSet == 1)
        {
            iceLaneVert.transform.position = iceLane1v.transform.position;
        }
        if (vertIceLaneSet == 2)
        {
            iceLaneVert.transform.position = iceLane2v.transform.position;
        }
        if (vertIceLaneSet == 3)
        {
            iceLaneVert.transform.position = iceLane3v.transform.position;
        }
        if (vertIceLaneSet == 4)
        {
            iceLaneVert.transform.position = iceLane4v.transform.position;
        }
        if (vertIceLaneSet == 5)
        {
            iceLaneVert.transform.position = iceLane5v.transform.position;
        }

    }


    void GenerateLavaSpot()
    {
        lavaHere = Random.Range(1, 6);

        if (lavaHere == 1)
        {
            showLava.transform.position = lavaSpot1.transform.position;
        }
        if (lavaHere == 2)
        {
            showLava.transform.position = lavaSpot2.transform.position;
        }
        if (lavaHere == 3)
        {
            showLava.transform.position = lavaSpot3.transform.position;
        }
        if (lavaHere == 4)
        {
            showLava.transform.position = lavaSpot4.transform.position;
        }
        if (lavaHere == 5)
        {
            showLava.transform.position = lavaSpot5.transform.position;
        }
    }
}
