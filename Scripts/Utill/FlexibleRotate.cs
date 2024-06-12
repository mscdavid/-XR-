using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlexibleRotate : MonoBehaviour
{
    [SerializeField]
    float m_rotateSpeed;
    [Range(-1,1)]
    public float m_dir;

    void Update()
    {
        transform.Rotate(0f, 0f, m_dir * m_rotateSpeed * Time.deltaTime, Space.Self);
    }
}
