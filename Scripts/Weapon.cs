using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
public class Weapon : MonoBehaviour
{
    [SerializeField]
    Player m_player;

    [SerializeField]
    XRBaseController m_controller;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;
        m_player.Attack(other.GetComponent<Enemy>());
        GameManager.Instance.Haptic(m_controller);

    }
}
