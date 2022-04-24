using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    [Header("References")]
    public PlayerController owner;
    public Rigidbody rb;
    public Transform hitPoint;
    public Transform hitPointTwo;
    public Transform hitPointThree;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;

    [Header("Variables")]
    private float lifetime = 10f;
    public float elaspedLife;
    public float hitRadius = 1;
    public float speed;
    public bool isDanger;
    public bool canBounce;
    public bool delayHitCheck;
    [SerializeField] private int maxBounces;
    [SerializeField] private int bouncesLeft;
    void Start()
    {
        isDanger = true;
        bouncesLeft = maxBounces;
        rb.velocity = (transform.forward * Time.fixedDeltaTime * speed * 1.5f);
    }
    void Update()
    {
        liveLife();
        lookForHit();
    }
    private void FixedUpdate()
    {

    }
    public void lookForHit()
    {
        if (isDanger)
        {
            RaycastHit ray;
            if (Physics.BoxCast(hitPoint.position, new Vector3(.25f, .25f, .25f), transform.forward, out ray, transform.rotation, hitRadius + .25f, playerLayer))
            {
                isDanger = false;
                combatController enemyCombat = ray.collider.GetComponent<combatController>(); // Fetch the enemy's combatController,
                PlayerController enemyControl = ray.collider.GetComponent<PlayerController>(); // enemy's PlayerController,
                enemyCombat.killPlayer(enemyCombat, enemyControl);
                Destroy(this.gameObject);
            }
            else if (Physics.BoxCast(hitPoint.position, new Vector3(.25f, .25f, .25f), transform.forward, out ray, transform.rotation, hitRadius + .25f, obstacleLayer))
            {
                Debug.Log("hit wall");
                if (!canBounce)
                {
                    isDanger = false;
                    rb.velocity = (Vector3.zero);
                    rb.isKinematic = true;
                }
                else if (canBounce && !delayHitCheck)
                {
                    delayHitCheck = true;
                    StartCoroutine("delayCheck");
                    if (bouncesLeft == 0)
                    {
                        rb.velocity = (Vector3.zero);
                        isDanger = false;
                        rb.isKinematic = true;
                    }
                    bouncesLeft = Mathf.Clamp(bouncesLeft -= 1, 0, maxBounces);
                }
            }
            if (Physics.Raycast(hitPoint.position, transform.forward, out ray, hitRadius, playerLayer))
            {
                /*isDanger = false;
                combatController enemyCombat = ray.collider.GetComponent<combatController>(); // Fetch the enemy's combatController,
                PlayerController enemyControl = ray.collider.GetComponent<PlayerController>(); // enemy's PlayerController,
                enemyCombat.killPlayer(enemyCombat, enemyControl);
                Destroy(this.gameObject);*/
            }
            else if (Physics.Raycast(hitPoint.position, transform.forward, out ray, hitRadius, obstacleLayer))
            {
                /*if (!canBounce)
                {
                    isDanger = false;
                    rb.velocity = (Vector3.zero);
                    rb.isKinematic = true;
                }
                else if (canBounce && !delayHitCheck)
                {
                    delayHitCheck = true;
                    StartCoroutine("delayCheck");
                    if (bouncesLeft == 0)
                    {
                        rb.velocity = (Vector3.zero);
                        isDanger = false;
                        rb.isKinematic = true;
                    }
                    bouncesLeft = Mathf.Clamp(bouncesLeft -= 1, 0, maxBounces);
                }*/
            }
        }
    }
    public IEnumerator delayCheck()
    {
        yield return new WaitForSeconds(.1f);
        delayHitCheck = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Environment" && !canBounce)
        {
            /*
            isDanger = false;
            rb.velocity = (Vector3.zero);
            rb.isKinematic = true;*/
        }

    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Environment") && canBounce)
        {
            /* if (bouncesLeft == 0)
             {
                 rb.velocity = (Vector3.zero);
                 isDanger = false;
                 rb.isKinematic = true;
             }
             bouncesLeft = Mathf.Clamp(bouncesLeft -= 1, 0, maxBounces);*/
        }
    }
    public void liveLife()
    {
        elaspedLife += Time.deltaTime;
        if (elaspedLife > lifetime)
            Destroy(this.gameObject);
    }
}
