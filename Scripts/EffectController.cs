using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    ParticleSystem m_vfx;

    void Awake()
    {
        m_vfx = GetComponent<ParticleSystem>();
    }


    void Update()
    {
        if(m_vfx.isStopped)
        {
            m_vfx.gameObject.SetActive(false);
            EffectManager.Instance.m_effectPool.Set(m_vfx);
        }
    }

}
