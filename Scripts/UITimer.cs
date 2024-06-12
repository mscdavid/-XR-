using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//UI���� class�� ����
using UnityEngine.UI;
using TMPro;

public class UITimer : MonoBehaviour
{
    //TextŸ���� timerText����
    public TextMeshProUGUI m_timerText;
    
    float m_totalTime = 5 * 60;
    //Ÿ�̸Ӱ� ���۵� ���� ����
    float m_startTime;

    bool m_isEnd;

    public float m_remainingTime = 0f;
    void Start()
    {
        m_isEnd = false;
        m_startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        // ��� �ð� ���
        float elapsed = Time.time - m_startTime;
        // ���� �ð� ���
        m_remainingTime = m_totalTime - elapsed;

        // ���� �ð��� 0���� ������ 0���� ���� (Ÿ�̸Ӱ� ����)
        if (m_remainingTime < 0)
        {
            m_remainingTime = 0;
            if(!m_isEnd)
            {
                m_isEnd = true;
                GameManager.Instance.GameClear();
            }
        }

        // ���� �ð��� �а� �ʷ� ��ȯ
        string minutes = ((int)m_remainingTime / 60).ToString("00");
        string seconds = (m_remainingTime % 60).ToString("00");

        // Ÿ�̸� �ؽ�Ʈ ������Ʈ
        m_timerText.text = "PlayTime : " + minutes + ":" + seconds;
    }
}