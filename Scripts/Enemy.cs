using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    [SerializeField]
    GameObject m_target;

    [SerializeField]
    GameObject m_attackAreaObj;

    [SerializeField]
    Rigidbody m_rigid;

    [SerializeField]
    Collider m_collider;

    [SerializeField]
    Scanner m_scanner;

    [SerializeField]
    UnitStat m_unitStat;

    [SerializeField]
    Collider m_attackAreaCollider;

    [SerializeField]
    HealthBar m_healthBar;

    Animator m_animator;
    AttackAreaUnitFind m_attackArea;

    AudioSource m_audio;

    [SerializeField]
    StateType m_stateType;

    bool m_isWalking;
    bool m_isDelay;
    bool m_isAttack;
    float m_speed;
    float m_attackRange;
    [SerializeField]
    int m_hp;
    [SerializeField]
    int m_damage;



    void Awake()
    {

    }

    public void InitEnemy()
    {
        m_speed = m_unitStat.m_moveSpeed;
        m_hp = m_unitStat.m_hp;
        m_damage = m_unitStat.m_damage;
        m_attackRange = m_unitStat.m_attackRange;
        m_target = GameManager.Instance.m_nexus;
        m_scanner = GetComponent<Scanner>();
        m_rigid = GetComponent<Rigidbody>();
        m_collider = GetComponent<BoxCollider>();
        m_animator = GetComponent<Animator>();
        m_healthBar = GetComponentInChildren<HealthBar>();
        m_audio = GetComponent<AudioSource>();

        m_attackAreaObj = transform.GetChild(2).gameObject;
        m_attackArea = m_attackAreaObj.GetComponentInChildren<AttackAreaUnitFind>();
        m_attackAreaCollider = m_attackAreaObj.GetComponentInChildren<BoxCollider>();

        m_healthBar.SetTarget(transform);
        m_healthBar.SetHealth(m_hp, m_unitStat.m_hp);

        m_isWalking = false;
        m_isDelay = false;
        m_isAttack = false;
        m_animator.SetBool("Die", false);
        m_stateType = StateType.Live;
        m_collider.enabled = true;
        m_attackAreaCollider.enabled = true;
        m_animator.enabled = true;
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.m_isLive || m_stateType != StateType.Live) return;

        if (m_target != GameManager.Instance.m_nexus && m_scanner.m_nearestTarget == null)
        {
            m_target = GameManager.Instance.m_nexus;
            m_animator.SetBool("Walk", true);
        }
        else if(m_scanner.m_nearestTarget != null && m_scanner.m_nearestTarget != m_target)
        {
            m_target = m_scanner.m_nearestTarget.gameObject;
        }
        var LookPos = new Vector3(m_target.transform.position.x, 0f, m_target.transform.position.z);
        transform.LookAt(LookPos);

        Vector3 dir = m_target.transform.position - m_rigid.position;
        dir.y = 0;
        Vector3 nextVec = dir.normalized * m_speed * Time.fixedDeltaTime;

        AnimatorClipInfo[] currentClips = m_animator.GetCurrentAnimatorClipInfo(0);

        m_isWalking = m_animator.GetBool("Walk");

        if (currentClips.Length > 0)
        {
            if(m_isWalking)
            {
                m_rigid.MovePosition(m_rigid.position + nextVec);
            }
            else if(m_isAttack)
            {
                m_rigid.MovePosition(m_rigid.position + Vector3.zero);
            }
        }
    }

    void LateUpdate()
    {
        if (m_stateType != StateType.Live) return;

        if(!m_isWalking && m_attackRange < Vector3.Distance(transform.position, m_target.transform.position))
        {
            m_animator.SetBool("Walk", true);
            m_isWalking = true;
        }
        if(m_scanner.m_nearestTarget != null)
        {
            AnimatorClipInfo[] currentClips = m_animator.GetCurrentAnimatorClipInfo(0);
            if (currentClips[0].clip.name != "Die" && currentClips[0].clip.name != "Attack" && 
                m_attackRange >= Vector3.Distance(transform.position, m_target.transform.position))
            {
                m_isAttack = true;
                m_animator.SetBool("Walk", false);
                m_animator.SetTrigger("Attack");
                m_isWalking = false;
            }
            else
            {
                m_isAttack = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Nexus")) return;

        Anim_RemoveEnemy();
        
    }

    public void Anim_Attack()
    {
        foreach (var unit in m_attackArea.m_unitList)
        {
            unit.GetComponent<Unit>().Hit(m_damage);
        }
    }

    public override void Hit(int damage)
    {
        if (m_stateType != StateType.Live) return;

        m_hp -= damage;
        m_healthBar.SetHealth(m_hp, m_unitStat.m_hp);
        if (m_hp <= 0)
        {
            StartCoroutine(Coroutine_Die());
        }
        else
        {
            if (m_isDelay) return;
            StartCoroutine(Coroutine_Delay());
        }
    }

    IEnumerator Coroutine_Delay()
    {
        m_isDelay = true;
        m_animator.SetTrigger("Hit");

        yield return null;
        yield return null;

        AnimatorStateInfo stateInfo = m_animator.GetCurrentAnimatorStateInfo(0);
        float animationLength = GetAnimationClipLength(stateInfo);

        // 애니메이션이 끝날 때까지 기다립니다.
        yield return new WaitForSeconds(animationLength);

        // 애니메이션이 끝난 후에 딜레이 시간을 기다립니다.
        yield return new WaitForSeconds(1.5f);
        m_isDelay = false;
    }

    IEnumerator Coroutine_Die()
    {

        m_stateType = StateType.Die;

        m_collider.enabled = false;
        m_attackAreaCollider.enabled = false;
        m_attackArea.ClearUnitList();
        
        m_animator.SetBool("Walk", false);
        m_animator.Play("Die", 0);
        m_audio.Play();
        var vfx = EffectManager.Instance.m_effectPool.Get();
        vfx.transform.position = transform.position;
        vfx.transform.position += Vector3.up * 10;

        vfx.gameObject.SetActive(true);
        vfx.Play();
        yield return null;
        yield return null;
        yield return null;
        AnimatorStateInfo stateInfo = m_animator.GetCurrentAnimatorStateInfo(0);
        
        float animationLength = GetAnimationClipLength(stateInfo);
        
        yield return new WaitForSeconds(animationLength);
        
        Anim_RemoveEnemy();
    }
    float GetAnimationClipLength(AnimatorStateInfo stateInfo)
    {
        
        AnimationClip[] clips = m_animator.runtimeAnimatorController.animationClips;

        foreach (AnimationClip clip in clips)
        {
            if (stateInfo.IsName(clip.name))
            {
                return clip.length;
            }
        }
        
        return 0f;
    }
    public void Anim_RemoveEnemy()
    {
        m_animator.enabled = false;
        ItemManager.Instance.DropItem(this);
        UnitManager.Instance.RemoveEnemy(this);
    }

    public int GetDamage()
    {
        return m_damage;
    }

}