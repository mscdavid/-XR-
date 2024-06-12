using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    AudioSource m_audio;
    bool m_isDelay;

    [SerializeField]
    XRBaseController m_controller;

    void Awake()
    {
        m_audio = GetComponent<AudioSource>();
        m_isDelay = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Pickaxe")) return;

        if(!m_isDelay)
        {
            StartCoroutine(Coroutine_Delay());
            m_audio.Play();
            GameManager.Instance.Haptic(m_controller);
            GameManager.Instance.m_resources += 10;
            GameManager.Instance.UpdateResources();
        }
        
        
    }

    IEnumerator Coroutine_Delay()
    {
        m_isDelay = true;
        yield return new WaitForSeconds(3f);
        m_isDelay = false;
    }
}
