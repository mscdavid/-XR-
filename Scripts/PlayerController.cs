using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerController : MonoBehaviour
{
    public Vector3 targetPosition = new Vector3(0f, 0f, 20f); // ��ǥ ���� ����
    public float moveSpeed = 3f; // �̵� �ӵ�
    public Animator animator; // Animator ������Ʈ
    private Scanner m_scanner;
    private float startTime;
    private bool m_isWalking = false;
    private bool find = false; //���� �������� ��
    private bool isCoroutineRunning = false;

    [SerializeField] private Slider _hpBar;

    // �÷��̾��� HP
    private int _hp;

    public int Hp
    {
        get => _hp;
        // HP�� PlayerController������ ���� �ϵ��� private���� ó��
        // Math.Clamp �Լ��� ����ؼ� hp�� 0���� �Ʒ��� �������� �ʰ� �մϴ�.
        private set => _hp = Math.Clamp(value, 0, _hp);
    }

    private void Awake()
    {
        // �÷��̾��� HP ���� �ʱ� �����մϴ�.
        _hp = 100;
        // MaxValue�� �����ϴ� �Լ��Դϴ�.
        SetMaxHealth(_hp);
    }

    public void SetMaxHealth(int health)
    {
        _hpBar.maxValue = health;
        _hpBar.value = health;
    }

    // �÷��̾ ������� ������ ����� ���� ���� �޾� HP�� �ݿ��մϴ�.
    public void GetDamage(int damage)
    {
        int getDamagedHp = Hp - damage;
        Hp = getDamagedHp;
        _hpBar.value = Hp;
    }

    void Start()
    {
        m_scanner = GetComponent<Scanner>();
        // Animator ������Ʈ �Ҵ�
        animator = GetComponent<Animator>();
        // Ÿ�̸� ���� �ð� ���
        startTime = Time.time;
        // �ʱ� ���� ����
        animator.SetBool("isWalking", false);
        animator.SetBool("find", false);
    }

    void Update()
    {
        // 3�� �Ŀ� �ȱ� ����
        if (!find && !m_isWalking && Time.time - startTime >= 3f)
        {
            m_isWalking = true;
            animator.SetBool("isWalking", true);
        }

        if (m_isWalking)
        {
            // ĳ���Ͱ� ��ǥ ������ ���� �̵�
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }

        if (m_scanner.m_nearestTarget != null)
        {
            if (!isCoroutineRunning)
            {
                StartCoroutine(Coroutine_AnimShoot());
            }
            m_isWalking = false;
            find = true;
        }
    }

    IEnumerator Coroutine_AnimShoot()
    {
        isCoroutineRunning = true;
        animator.SetBool("find", true);
        yield return new WaitForSeconds(1f); // 3�� ���

        // ���� ����� ���� �����ϴ��� Ȯ��
        if (m_scanner.m_nearestTarget != null)
        {
            // Ÿ���� EnemyHealth ������Ʈ�� �����ͼ� ������� �ݴϴ�.
            EnemyHealth enemyHealth = m_scanner.m_nearestTarget.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(10); // ���� ����� �� 10
            }
        }

        animator.SetBool("find", false);
        isCoroutineRunning = false;
    }
}
