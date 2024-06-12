using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPack : MonoBehaviour
{

    void Start()
    {
        Invoke("SetItemTimer", 8f);
    }


    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Debug.Log("Heal");
        var player = other.GetComponent<Player>();
        var isHealed = player.Heal();
        if (!isHealed) return;

        gameObject.SetActive(false);
        GameManager.Instance.HealPackRegen();

    }
}
