using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class UIClickSpawnUnit : MonoBehaviour
{
    public XRController controller; 

    public Button uiBigButton; 
    public Button uiSmallButton;

    public Transform spawnLocation;

    private GameObject spawnedBigObject;
    private GameObject spawnedSmallObject;

    private bool isBigObjectSpawned = false; 
    private bool isSmallObjectSpawned = false;

    private XRGrabInteractable grabInteractableBig;
    private XRGrabInteractable grabInteractableSmall;

    private Rigidbody m_rigidBig;
    private Rigidbody m_rigidSmall;

    private Vector3 previousPosition;
    private Vector3 velocity;



    private void Start()
    {
        uiBigButton.onClick.AddListener(BigAllySpawn);
        uiSmallButton.onClick.AddListener(SmallAllySpawn);
    }
    
    private void BigAllySpawn()
    {
        if (GameManager.Instance.BigCost())
        {
            var allyBig = UnitManager.Instance.GetAllyBig();
            if (allyBig != null)
            {
                allyBig.gameObject.SetActive(true);
                allyBig.transform.position = spawnLocation.position;
                allyBig.transform.rotation = spawnLocation.rotation;
                allyBig.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
        


    }

    private void SmallAllySpawn()
    {
        if (GameManager.Instance.SmallCost())
        {
            var allySmall = UnitManager.Instance.GetAllySmall();
            if (allySmall != null)
            {
                allySmall.gameObject.SetActive(true);
                allySmall.transform.position = spawnLocation.position;
                allySmall.transform.rotation = spawnLocation.rotation;
                allySmall.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }
}
