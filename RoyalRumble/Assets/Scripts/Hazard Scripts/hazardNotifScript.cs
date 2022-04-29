using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hazardNotifScript : MonoBehaviour
{

    private float timeActive;


    // Start is called before the first frame update
    void Start()
    {
        timeActive = 4f;
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        timeActive -= Time.deltaTime;
        {
            if (timeActive < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
