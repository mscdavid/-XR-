using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    [SerializeField]
    UnitStat m_unitStat;

    [SerializeField]
    Weapon m_weapon;

    [SerializeField]
    HealthBar m_healthBar;

    int m_hp;
    int m_damage;
    bool m_isDelay;

    void Awake()
    {
        m_isDelay = false;
        m_hp = m_unitStat.m_hp;
        m_damage = m_unitStat.m_damage;
    }

    public void Attack(Enemy enemy)
    {
        if (m_isDelay) return;

        if(!m_isDelay)
        {
            enemy.Hit(m_damage);
            StartCoroutine(Coroutine_Delay());
        }
    }

    public override void Hit(int damage)
    {
        m_hp -= damage;
        m_healthBar.SetHealth(m_hp, m_unitStat.m_hp);
        if(m_hp <= 0)
        {
            GameManager.Instance.GameOver();
            
        }
    }

    IEnumerator Coroutine_Delay()
    {
        m_isDelay = true;
        yield return new WaitForSeconds(1.5f);
        m_isDelay = false;
    }

    public bool Heal()
    {
        if (m_hp + 10 > m_unitStat.m_hp) return false;

        m_hp += 10;
        m_healthBar.SetHealth(m_hp, m_unitStat.m_hp);
        return true;
    }


}
