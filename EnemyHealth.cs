using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public GameObject itemzone; // ������ �� ������
    public float dropChance = 0.5f; // 50% Ȯ���� ������ �� ����

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
        float randomValue = Random.value; // 0.0f���� 1.0f ������ ���� ��ȯ
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
        // ������Ʈ ������ �ʿ����� �ʴٸ� �� �޼���� ����Ӵϴ�.
    }
}
