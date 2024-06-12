using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using System.Collections;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [Header(" # Game Control ")]
    public bool m_isLive;

    [Header("# Game Object")]
    public GameObject m_nexus;
    public GameObject m_spawnPoint;
    public GameObject m_mineral;
    public GameObject m_healPack;
    public GameObject m_gameOver;
    public GameObject m_gameClear;
    public UITimer m_timer;
    public TextMeshProUGUI m_textMesh;
    public Camera m_camera;
    public int m_resources;
    public int m_smallCost;
    public int m_bigCost;

    protected override void OnAwake()
    {
        m_isLive = true;
        m_camera = Camera.main;
        m_gameClear.SetActive(false);
        m_gameOver.SetActive(false);
        UpdateResources();
    }

    public void UpdateResources()
    {
        m_textMesh.text = m_resources.ToString();
    }

    public void Haptic(XRBaseController controller)
    {
        controller.SendHapticImpulse(1.0f, 0.3f);

    }

    public bool SmallCost()
    {
        if((m_resources -= m_smallCost) < 0)
        {
            m_resources += m_smallCost;
            return false;
        }
        else
        {
            UpdateResources();
            return true;
        }


    }

    public bool BigCost()
    {
        if ((m_resources -= m_bigCost) < 0)
        {
            m_resources += m_bigCost;
            return false;
        }
        else
        {
            UpdateResources();
            return true;
        }
    }

    public void HealPackRegen()
    {
        StartCoroutine(Coroutine_HealPackRegen());
    }

    public void GameOver()
    {
        //m_gameOver.transform.position = Camera.main.transform.forward * 20f;
        m_gameOver.SetActive(true);
        m_isLive = false;
        Invoke("Quit", 5f);
    }

    public void GameClear()
    {
        //m_gameOver.transform.position = Camera.main.transform.forward * 20f;
        m_gameClear.SetActive(true);
        m_isLive = false;
        Invoke("Quit", 5f);
    }


    IEnumerator Coroutine_HealPackRegen()
    {
        yield return new WaitForSeconds(30f);
        m_healPack.SetActive(true);
    }

    void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 빌드된 애플리케이션에서 실행 중인 경우
        Application.Quit();
#endif
    }
}