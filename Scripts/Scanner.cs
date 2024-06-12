using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    
    public float m_scanRange;
    public LayerMask m_targetLayer;
    public Collider[] m_targets;
    public Transform m_nearestTarget;

    private void FixedUpdate()
    {
        m_targets = Physics.OverlapSphere(transform.position, m_scanRange, m_targetLayer);

        m_nearestTarget = GetNearest();
    }

    private Transform GetNearest()
    {
        Transform result = null;
        float diff = 100;

        foreach (Collider target in m_targets)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float curDiff = Vector3.Distance(myPos, targetPos);

            if (curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;

            }
        }

        return result;
    }
    

    

}
