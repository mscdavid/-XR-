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
            // ��Ʈ�ѷ��� Ʈ���� ��ư�� ������ ��
            if (controller.inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerValue) && triggerValue)
            {
                // ��ư�� ���� �����̰� ������ ��ư�� ������ ���� ������ ���� ����
                if (!isButtonDown)
                {
                    isButtonDown = true;

                    // ��Ʈ�ѷ��� ��ġ�� ���⿡�� ����ĳ��Ʈ�� �߻�
                    RaycastHit hit;
                    if (Physics.Raycast(controller.transform.position, controller.transform.forward, out hit, maxDistance))
                    {
                        // ���̰� ������ ��ġ�� ���ο� ������Ʈ ����
                        Instantiate(prefabToSpawn, hit.point, Quaternion.identity);
                    }
                }
            }
            else
            {
                isButtonDown = false; // ��ư�� ������ ���� ���·� �ʱ�ȭ
            }
        }
    }



}
