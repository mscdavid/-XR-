using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crystalnum : MonoBehaviour
{
    public Text numText; // 숫자를 표시할 Text 컴포넌트
    private int currentNumber = 0; // 현재 숫자 값

    void Start()
    {
        // 초기 숫자 값 설정
        if (numText == null)
        {
            Debug.LogError("numText is not assigned!");
            return;
        }
        numText.text = ": " + currentNumber.ToString();
        // 코루틴 시작
        StartCoroutine(IncreaseNumberEveryFiveSeconds());
    }

    IEnumerator IncreaseNumberEveryFiveSeconds()
    {
        while (true)
        {
            // 5초 대기
            yield return new WaitForSeconds(8f);
            // 숫자 증가
            currentNumber += 100;
            // Text 컴포넌트에 새로운 숫자 값 설정
            numText.text = ": " + currentNumber.ToString();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
