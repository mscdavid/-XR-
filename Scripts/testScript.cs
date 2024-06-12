using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class testScript : MonoBehaviour
{
    public XRController controller;
    public GameObject prefabToSpawn;
    public float maxDistance = 12000f;

    private bool isButtonDown = false;

    private void Update()
    {
        if (controller)
        {
            // 컨트롤러의 트리거 버튼을 눌렀을 때
            if (controller.inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerValue) && triggerValue)
            {
                // 버튼이 눌린 상태이고 이전에 버튼이 눌리지 않은 상태일 때만 실행
                if (!isButtonDown)
                {
                    isButtonDown = true;

                    // 컨트롤러의 위치와 방향에서 레이캐스트를 발사
                    RaycastHit hit;
                    if (Physics.Raycast(controller.transform.position, controller.transform.forward, out hit, maxDistance))
                    {
                        // 레이가 적중한 위치에 새로운 오브젝트 생성
                        Instantiate(prefabToSpawn, hit.point, Quaternion.identity);
                    }
                }
            }
            else
            {
                isButtonDown = false; // 버튼이 눌리지 않은 상태로 초기화
            }
        }
    }



}
