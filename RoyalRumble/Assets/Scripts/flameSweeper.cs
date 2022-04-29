using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flameSweeper : MonoBehaviour
{
    public Transform[] travelPoints;
    public LayerMask playerLayer;
    public float lerpAlpha;
    public float timeSinceLife;
    public bool goLeft;
    void Start()
    {
        travelPoints = new Transform[3];
        travelPoints[0] = GameObject.Find("Transform Spot One").transform;
        travelPoints[1] = GameObject.Find("Transform Spot Two").transform;
        travelPoints[2] = GameObject.Find("Transform Spot Three").transform;

        if (goLeft)
            FindObjectOfType<gameHazardManager>().warningMarkerLeft.SetActive(false);
        if (!goLeft)
            FindObjectOfType<gameHazardManager>().warningMarkerRight.SetActive(false);
    }
    void Update()
    {
        lerpAlpha += Time.deltaTime * 2f;
        move(goLeft);
        //sprayFire(goLeft);
        if (Vector3.Distance(transform.position, travelPoints[0].position) > 15f)
        {
            Destroy(this.gameObject);
        }
    }
    public void move(bool goLeft)
    {
        if (!goLeft)
        {
            transform.position = Vector3.Lerp(travelPoints[0].position, travelPoints[2].position, lerpAlpha);
        }
        else if (goLeft)
        {
            transform.position = Vector3.Lerp(travelPoints[0].position, travelPoints[1].position, lerpAlpha);
        }
    }
    public void sprayFire(bool goLeft)
    {
        RaycastHit ray;
        if (!goLeft)
        {
            if (Physics.BoxCast(transform.position, new Vector3(.75f, 2f, 30f), transform.right, out ray, Quaternion.identity, 2f, playerLayer))
            {
                Debug.Log("Hit Player");
                combatController enemyCombat = ray.collider.GetComponent<combatController>(); // Fetch the enemy's combatController,
                PlayerController enemyControl = ray.collider.GetComponent<PlayerController>(); // enemy's PlayerController,
                roundManager rManager = FindObjectOfType<roundManager>(); // and the roundManager.
                enemyCombat.killPlayer(enemyCombat, enemyControl);
                rManager.checkForRoundWin();
                Destroy(this.gameObject);
            }
        }
        else if (goLeft)
        {
            if (Physics.BoxCast(transform.position, new Vector3(.75f, 2f, 30f), -transform.right, out ray, Quaternion.identity, 2f, playerLayer))
            {
                Debug.Log("Hit Player");
                combatController enemyCombat = ray.collider.GetComponent<combatController>(); // Fetch the enemy's combatController,
                PlayerController enemyControl = ray.collider.GetComponent<PlayerController>(); // enemy's PlayerController,
                roundManager rManager = FindObjectOfType<roundManager>(); // and the roundManager.
                enemyCombat.killPlayer(enemyCombat, enemyControl);
                rManager.checkForRoundWin();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            combatController enemyCombat = other.GetComponent<combatController>(); // Fetch the enemy's combatController,
            PlayerController enemyControl = other.GetComponent<PlayerController>(); // enemy's PlayerController,
            roundManager rManager = FindObjectOfType<roundManager>(); // and the roundManager.
            enemyCombat.killPlayer(enemyCombat, enemyControl);
            rManager.checkForRoundWin();
        }
    }
}
