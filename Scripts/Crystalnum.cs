using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crystalnum : MonoBehaviour
{
    public Text numText; // ���ڸ� ǥ���� Text ������Ʈ
    private int currentNumber = 0; // ���� ���� ��

    void Start()
    {
        // �ʱ� ���� �� ����
        if (numText == null)
        {
            Debug.LogError("numText is not assigned!");
            return;
        }
        numText.text = ": " + currentNumber.ToString();
        // �ڷ�ƾ ����
        StartCoroutine(IncreaseNumberEveryFiveSeconds());
    }

    IEnumerator IncreaseNumberEveryFiveSeconds()
    {
        while (true)
        {
            // 5�� ���
            yield return new WaitForSeconds(8f);
            // ���� ����
            currentNumber += 100;
            // Text ������Ʈ�� ���ο� ���� �� ����
            numText.text = ": " + currentNumber.ToString();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
