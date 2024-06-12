using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public GameObject itemzone; // 아이템 존 프리팹
    public float dropChance = 0.5f; // 50% 확률로 아이템 존 생성

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        Debug.Log("TakeDamage called with amount: " + amount);
        currentHealth -= amount;
        Debug.Log("Current Health: " + currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Die function called.");
        TryDropItemZone();
        Destroy(gameObject);
    }

    void TryDropItemZone()
    {
        Debug.Log("TryDropItemZone function called.");
        float randomValue = Random.value; // 0.0f에서 1.0f 사이의 값을 반환
        Debug.Log("Random Value: " + randomValue);
        if (randomValue <= dropChance)
        {
            Debug.Log("Item zone instantiated.");
            var obj = Instantiate(itemzone, transform.position, transform.rotation);
            obj.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        // 업데이트 로직이 필요하지 않다면 이 메서드는 비워둡니다.
    }
}
