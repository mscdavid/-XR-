using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAreaUnitFind : MonoBehaviour
{
    public List<GameObject> m_unitList = new List<GameObject>();

    public GameObject GetNearestUnit(Transform target)
    {
        if (m_unitList.Count == 0) return null;

        var parent = target;
        int index = 0;
        float minDist = (m_unitList[0].transform.position - parent.transform.position).sqrMagnitude;
        for (int i = 1; i < m_unitList.Count; i++)
        {
            var dist = (m_unitList[i].transform.position - parent.transform.position).sqrMagnitude;
            if(dist < minDist)
            {
                minDist = dist;
                index = i;
            }
        }

        return m_unitList[index];
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ally") || other.CompareTag("Player"))
        {
            m_unitList.Add(other.gameObject);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ally") || other.CompareTag("Player"))
        {
            m_unitList.Remove(other.gameObject);
        }
    }

    public void ClearUnitList()
    {
        m_unitList.Clear();
    }

    void Start()
    {
        ClearUnitList();
    }

}
