using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    None = -1,
    Effect1,
    Effect2,
    Effect3,
    Max
}
public class EffectManager : SingletonMonoBehaviour<EffectManager>
{
    [SerializeField]
    ParticleSystem[] m_effect;

    public ObjectPool<ParticleSystem> m_effectPool = null;


    protected override void OnAwake()
    {
        m_effectPool = new ObjectPool<ParticleSystem>(9, () =>
        {
            var obj = Instantiate(m_effect[Random.Range((int)EffectType.Effect1, (int)EffectType.Max)].gameObject);
            obj.SetActive(false);
            obj.transform.parent = transform;
            var vfx = obj.GetComponent<ParticleSystem>();
            vfx.Stop();

            return vfx;
        });  
    }
}
