using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//UI관련 class에 접근
using UnityEngine.UI;
using TMPro;

public class UITimer : MonoBehaviour
{
    //Text타입의 timerText선언
    public TextMeshProUGUI m_timerText;
    
    float m_totalTime = 5 * 60;
    //타이머가 시작된 시점 선언
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
        // 경과 시간 계산
        float elapsed = Time.time - m_startTime;
        // 남은 시간 계산
        m_remainingTime = m_totalTime - elapsed;

        // 남은 시간이 0보다 작으면 0으로 설정 (타이머가 멈춤)
        if (m_remainingTime < 0)
        {
            m_remainingTime = 0;
            if(!m_isEnd)
            {
                m_isEnd = true;
                GameManager.Instance.GameClear();
            }
        }

        // 남은 시간을 분과 초로 변환
        string minutes = ((int)m_remainingTime / 60).ToString("00");
        string seconds = (m_remainingTime % 60).ToString("00");

        // 타이머 텍스트 업데이트
        m_timerText.text = "PlayTime : " + minutes + ":" + seconds;
    }
}