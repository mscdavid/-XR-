using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ally")) return;
        other.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
