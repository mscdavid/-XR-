using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerController : MonoBehaviour
{
    public Vector3 targetPosition = new Vector3(0f, 0f, 20f); // 목표 지점 설정
    public float moveSpeed = 3f; // 이동 속도
    public Animator animator; // Animator 컴포넌트
    private Scanner m_scanner;
    private float startTime;
    private bool m_isWalking = false;
    private bool find = false; //적을 마주쳤을 때
    private bool isCoroutineRunning = false;

    [SerializeField] private Slider _hpBar;

    // 플레이어의 HP
    private int _hp;

    public int Hp
    {
        get => _hp;
        // HP는 PlayerController에서만 수정 하도록 private으로 처리
        // Math.Clamp 함수를 사용해서 hp가 0보다 아래로 떨어지지 않게 합니다.
        private set => _hp = Math.Clamp(value, 0, _hp);
    }

    private void Awake()
    {
        // 플레이어의 HP 값을 초기 세팅합니다.
        _hp = 100;
        // MaxValue를 세팅하는 함수입니다.
        SetMaxHealth(_hp);
    }

    public void SetMaxHealth(int health)
    {
        _hpBar.maxValue = health;
        _hpBar.value = health;
    }

    // 플레이어가 대미지를 받으면 대미지 값을 전달 받아 HP에 반영합니다.
    public void GetDamage(int damage)
    {
        int getDamagedHp = Hp - damage;
        Hp = getDamagedHp;
        _hpBar.value = Hp;
    }

    void Start()
    {
        m_scanner = GetComponent<Scanner>();
        // Animator 컴포넌트 할당
        animator = GetComponent<Animator>();
        // 타이머 시작 시간 기록
        startTime = Time.time;
        // 초기 상태 설정
        animator.SetBool("isWalking", false);
        animator.SetBool("find", false);
    }

    void Update()
    {
        // 3초 후에 걷기 시작
        if (!find && !m_isWalking && Time.time - startTime >= 3f)
        {
            m_isWalking = true;
            animator.SetBool("isWalking", true);
        }

        if (m_isWalking)
        {
            // 캐릭터가 목표 지점을 향해 이동
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
        yield return new WaitForSeconds(1f); // 3초 대기

        // 공격 대상이 아직 존재하는지 확인
        if (m_scanner.m_nearestTarget != null)
        {
            // 타겟의 EnemyHealth 컴포넌트를 가져와서 대미지를 줍니다.
            EnemyHealth enemyHealth = m_scanner.m_nearestTarget.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(10); // 예제 대미지 값 10
            }
        }

        animator.SetBool("find", false);
        isCoroutineRunning = false;
    }
}
