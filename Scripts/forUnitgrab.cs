using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class forUnitgrab : MonoBehaviour
{

    public GameObject objectSpawn; 

    private XRGrabInteractable grabInteractable;

    public Rigidbody rb;

    private void Start()
    {
        // XRGrabInteractable ������Ʈ ����
        grabInteractable = GetComponent<XRGrabInteractable>();

        rb = objectSpawn.GetComponent<Rigidbody>();

        // Grab�� �߻����� �� ȣ��� �̺�Ʈ �Լ� ���
        grabInteractable.onSelectEntered.AddListener(OnGrabbedObject);
    }

    // Grab�� �߻����� �� ȣ��� �Լ�
    private void OnGrabbedObject(XRBaseInteractor interactor)
    {
        Debug.Log("jjjjjj");
      

        rb.isKinematic = false;
    }
}
