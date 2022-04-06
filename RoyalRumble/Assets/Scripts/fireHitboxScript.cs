using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireHitboxScript : MonoBehaviour
{
    Rigidbody rb;
    public float timeActive;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        timeActive = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject gameManager = GameObject.FindWithTag("gameManager");
        hazardManager hM = gameManager.GetComponent<hazardManager>();
        roundManager rM = gameManager.GetComponent<roundManager>();

        if(hM.arrowPosSet == 1)
        {
            rb.velocity = new Vector3 (5, 0, 0);
        }
        if(hM.arrowPosSet == 2)
        {
            rb.velocity = new Vector3 (0, 0, -5);
        }
        if(hM.arrowPosSet == 3)
        {
            rb.velocity = new Vector3 (-5, 0, 0);
        }
        if(hM.arrowPosSet == 4)
        {
            rb.velocity = new Vector3 (0, 0, 5);
        }

        timeActive -= Time.deltaTime;
        if(timeActive < 0)
        {
            Destroy(gameObject);
        }
    }
}
