using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//UI���� class�� ����
using UnityEngine.UI;

public class UItimer1 : MonoBehaviour
{
    //TextŸ���� timerText����
    public Text m_timerText;
    private float m_totalTime = 10 * 60;
    //Ÿ�̸Ӱ� ���۵� ���� ����
    private float m_startTime;
    void Start()
    {
        m_startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        // ��� �ð� ���
        float elapsed = Time.time - m_startTime;
        // ���� �ð� ���
        float remainingTime = m_totalTime - elapsed;

        // ���� �ð��� 0���� ������ 0���� ���� (Ÿ�̸Ӱ� ����)
        if (remainingTime < 0)
        {
            remainingTime = 0;
        }

        // ���� �ð��� �а� �ʷ� ��ȯ
        string minutes = ((int)remainingTime / 60).ToString("00");
        string seconds = (remainingTime % 60).ToString("00");

        // Ÿ�̸� �ؽ�Ʈ ������Ʈ
        m_timerText.text = "PlayTime : " + minutes + ":" + seconds;
    }
}
