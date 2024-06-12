using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class Ally : Unit
{
    [SerializeField]
    GameObject m_target;

    [SerializeField]
    Rigidbody m_rigid;

    [SerializeField]
    Collider m_collider;

    [SerializeField]
    Scanner m_scanner;

    [SerializeField]
    UnitStat m_unitStat;

    [SerializeField]
    HealthBar m_healthBar;

    XRGrabInteractable m_grabInteractable;

    Animator m_animator;

    [SerializeField]
    StateType m_stateType;

    [SerializeField]
    AllyType m_allyType;

    bool m_isPatrol;
    bool m_isWalking;
    bool m_isDelay;
    bool m_isAttack;
    bool m_isDigging;
    [SerializeField]
    bool m_isGrabbed;

    float m_speed;
    float m_attackRange;

    Vector3 m_previousPosition;
    Vector3 m_velocity;

    [SerializeField]
    int m_hp;
    [SerializeField]
    int m_damage;
    
    public bool IsGrabbed
    {
        get { return m_isGrabbed; }
        set { m_isGrabbed = value; }
    }

    public AllyType Type
    {
        get { return m_allyType; }
        private set { }
    }

    void Awake()
    {
        m_isWalking = false;
        m_isDelay = false;
        m_isAttack = false;
        m_isPatrol = false;
        m_isDigging = false;
        m_isGrabbed = true;
    }
    
    public void InitAlly()
    {
        m_speed = m_unitStat.m_moveSpeed;
        m_hp = m_unitStat.m_hp;
        m_damage = m_unitStat.m_damage;
        m_attackRange = m_unitStat.m_attackRange;

        m_rigid = GetComponent<Rigidbody>();
        m_collider = GetComponent<BoxCollider>();
        m_scanner = GetComponent<Scanner>();
        m_animator = GetComponent<Animator>();
        m_healthBar = GetComponentInChildren<HealthBar>();
        m_grabInteractable = GetComponent<XRGrabInteractable>();

        m_grabInteractable.selectEntered.AddListener(OnSelectEntered);
        m_grabInteractable.selectExited.AddListener(OnSelectExited);
        m_healthBar.SetTarget(transform);
        m_healthBar.SetHealth(m_hp, m_unitStat.m_hp);

        m_isWalking = false;
        m_isDelay = false;
        m_isAttack = false;
        m_isPatrol = false;
        m_isDigging = false;
        m_isGrabbed = true;

        m_animator.SetBool("Die", false);
        m_animator.SetBool("Walk", false);
        m_stateType = StateType.Live;
        m_collider.enabled = true;
        m_animator.enabled = true;
    }

    void Update()
    {
        if (m_grabInteractable.isSelected)
        {
            m_velocity = (transform.position - m_previousPosition) / Time.deltaTime;
            m_previousPosition = transform.position;
        }
    }
    void FixedUpdate()
    {
        if (m_isGrabbed || !GameManager.Instance.m_isLive || m_stateType != StateType.Live ) return;

        if (m_target != GameManager.Instance.m_mineral && m_scanner.m_nearestTarget == null && m_stateType != StateType.Patrol)
        {
            m_stateType = StateType.Patrol;
            m_animator.SetBool("Walk", false);
            m_isWalking = m_animator.GetBool("Walk");
            m_animator.Play("Idle");
            StartCoroutine(Coroutine_SearchTarget(300));
            
        }
        else if (m_scanner.m_nearestTarget != null && m_target != m_scanner.m_nearestTarget)
        {
            m_target = m_scanner.m_nearestTarget.gameObject;
        }

        if(m_target != null)
        {
            var LookPos = new Vector3(m_target.transform.position.x, 0f, m_target.transform.position.z);

            transform.LookAt(LookPos);

            Vector3 dir = m_target.transform.position - m_rigid.position;
            dir.y = 0;
            Vector3 nextVec = dir.normalized * m_speed * Time.fixedDeltaTime;
            AnimatorClipInfo[] currentClips = m_animator.GetCurrentAnimatorClipInfo(0);
            m_isWalking = m_animator.GetBool("Walk");

            if (currentClips.Length > 0)
            {
                if (m_isWalking)
                {
                    m_rigid.MovePosition(m_rigid.position + nextVec);
                }
                else if (m_isAttack)
                {
                    m_rigid.MovePosition(m_rigid.position + Vector3.zero);
                }
            }
        }
    }

    void LateUpdate()
    {
        
        if (m_isGrabbed || !GameManager.Instance.m_isLive || m_stateType != StateType.Live || m_target == null) return;

        if (!m_isWalking && m_attackRange < Vector3.Distance(transform.position, m_target.transform.position))
        {
            m_animator.SetBool("Walk", true);
            m_isWalking = m_animator.GetBool("Walk");
        }
        else if (!m_isWalking && m_attackRange >= Vector3.Distance(transform.position, m_target.transform.position))
        {
            m_animator.SetTrigger("Attack");
            if (!m_isDigging)
            {
                StartCoroutine(Coroutine_Digging());
            }
        }
        if (m_scanner.m_nearestTarget != null && m_target != GameManager.Instance.m_mineral)
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
        else if(m_scanner.m_nearestTarget == null && m_target == GameManager.Instance.m_mineral)
        {
            AnimatorClipInfo[] currentClips = m_animator.GetCurrentAnimatorClipInfo(0);
            if (currentClips[0].clip.name != "Die" && currentClips[0].clip.name != "Attack" &&
                5f >= Vector3.Distance(transform.position, m_target.transform.position))
            {
                
                m_animator.SetBool("Walk", false);
                m_animator.SetTrigger("Attack");
                m_isWalking = false;

                if(!m_isDigging)
                {
                    StartCoroutine(Coroutine_Digging());
                }

            }
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

    public void Anim_RemoveAlly()
    {
        m_animator.enabled = false;
        m_grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
        m_grabInteractable.selectExited.RemoveListener(OnSelectExited);
        UnitManager.Instance.RemoveAlly(this);
    }

    public void Anim_Attack()
    {
        if(m_target != GameManager.Instance.m_mineral)
        {
            var enemy = m_target.GetComponent<Unit>();
            if (enemy != null)
            {
                enemy.Hit(m_damage);
            }
        }
        
    }

    IEnumerator Coroutine_Die()
    {

        m_stateType = StateType.Die;

        m_collider.enabled = false;
        
        m_animator.SetBool("Walk", false);
        m_animator.ResetTrigger("Hit");
        m_animator.Play("Die", 0);
        yield return null;
        yield return null;
        yield return null;
        AnimatorStateInfo stateInfo = m_animator.GetCurrentAnimatorStateInfo(0);

        float animationLength = GetAnimationClipLength(stateInfo);

        yield return new WaitForSeconds(animationLength);

        Anim_RemoveAlly();
    }

    IEnumerator Coroutine_SearchTarget(int frame)
    {

        for (int i = 0; i < frame; i++)
        {
            if (m_scanner.m_nearestTarget != null)
            {
                m_isPatrol = false;
                break;
            }
            else
            {
                m_isPatrol = true;
                yield return null;
            }
        }
            
        if(m_isPatrol)
        {
            m_isPatrol = false;
            m_target = GameManager.Instance.m_mineral;
            m_stateType = StateType.Live;
        }
        else if(!m_isPatrol)
        {
            m_target = m_scanner.m_nearestTarget.gameObject;
            m_stateType = StateType.Live;
        }
        Debug.Log("Completed Patrol");
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

    IEnumerator Coroutine_Digging()
    {
        m_isDigging = true;
        yield return new WaitForSeconds(2f);
        GameManager.Instance.m_resources += 10;
        GameManager.Instance.UpdateResources();
        m_isDigging = false;    
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        m_isGrabbed = true;
        m_rigid.isKinematic = true;
        m_animator.SetBool("Walk", false);
        m_rigid.velocity = m_velocity * 5f;

        
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        m_isGrabbed = false;
        m_rigid.isKinematic = false;
        m_rigid.velocity = m_velocity * 5f;
    }
    
}