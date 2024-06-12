using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//UI관련 class에 접근
using UnityEngine.UI;

public class UItimer1 : MonoBehaviour
{
    //Text타입의 timerText선언
    public Text m_timerText;
    private float m_totalTime = 10 * 60;
    //타이머가 시작된 시점 선언
    private float m_startTime;
    void Start()
    {
        m_startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        // 경과 시간 계산
        float elapsed = Time.time - m_startTime;
        // 남은 시간 계산
        float remainingTime = m_totalTime - elapsed;

        // 남은 시간이 0보다 작으면 0으로 설정 (타이머가 멈춤)
        if (remainingTime < 0)
        {
            remainingTime = 0;
        }

        // 남은 시간을 분과 초로 변환
        string minutes = ((int)remainingTime / 60).ToString("00");
        string seconds = (remainingTime % 60).ToString("00");

        // 타이머 텍스트 업데이트
        m_timerText.text = "PlayTime : " + minutes + ":" + seconds;
    }
}
