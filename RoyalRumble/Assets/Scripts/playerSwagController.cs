using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSwagController : MonoBehaviour
{
    public PlayerController controller;
    public SkinnedMeshRenderer pRenderer;
    public MeshRenderer ringRenderer;

    [Header("Materials")]
    public Material[] charMaterials;
    public Material[] playerRingMaterials;

    [Header("Accessories")]
    public GameObject itemZero;
    public GameObject itemOne;
    public GameObject itemTwo;
    public GameObject itemThree;
    public GameObject crown;

    [Header("Misc")]
    public GameObject ring;
    public int pIndex;
    [SerializeField] private Vector3 ringRotation;
    private float rotGain;

    void Start()
    {
        Invoke("dressUp", .3f);
    }

    void Update()
    {
        rotGain = rotGain += Time.deltaTime * 25f;
        ringRotation = new Vector3(-90, 0, rotGain);
        ring.transform.rotation = Quaternion.Euler(ringRotation);
    }
    public void dressUp()
    {
        pIndex = controller.playerID;
        switch (pIndex)
        {
            case 0:

                pRenderer.material = charMaterials[0];
                ringRenderer.material = playerRingMaterials[0];
                itemZero.SetActive(true);
                break;
            case 1:

                pRenderer.material = charMaterials[1];
                ringRenderer.material = playerRingMaterials[1];
                itemOne.SetActive(true);
                break;
            case 2:
                pRenderer.material = charMaterials[2];
                ringRenderer.material = playerRingMaterials[2];
                itemTwo.SetActive(true);
                break;

            case 3:
                /*pRenderer.material = charMaterials[3];
                ringRenderer.material = playerRingMaterials[3];*/
                itemThree.SetActive(true);
                break;
        }
    }
    public void equipCrown()
    {
        crown.SetActive(true);
    }
}
