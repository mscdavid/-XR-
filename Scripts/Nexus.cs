using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nexus : MonoBehaviour
{
    [SerializeField]
    UnitStat m_unitStat;
    [SerializeField]
    HealthBar m_healthBar;
    int m_hp;

    void Awake()
    {
        InitStat();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;

        other.gameObject.SetActive(false);
        var damage = other.GetComponent<Enemy>().GetDamage();
        m_hp -= damage;
        m_healthBar.SetHealth(m_hp, m_unitStat.m_hp);

        if(m_hp <= 0)
        {
            GameManager.Instance.GameOver();
        }
    }

    void InitStat()
    {
        m_hp = m_unitStat.m_hp;
        m_healthBar.SetTarget(transform);
        m_healthBar.SetHealth(m_hp, m_unitStat.m_hp);
    }
}
