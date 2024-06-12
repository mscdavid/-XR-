using UnityEngine;

public class ItemZone : MonoBehaviour
{
    public GameObject itemPrefab; // ����� ������ ������

    void Start()
    {
        Invoke("SetItemTimer", 8f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ally")) return;

        var ally = other.GetComponent<Ally>();

        if(ally.Type == AllyType.AllyBig)
        {
            var allyBig = UnitManager.Instance.m_allyBigPool.Get();
            allyBig.transform.position = other.transform.position + new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            allyBig.InitAlly();
            allyBig.IsGrabbed = false;
            allyBig.gameObject.SetActive(true);

        }
        else if(ally.Type == AllyType.AllySmall)
        {
            var allySmall = UnitManager.Instance.m_allySmallPool.Get();
            allySmall.transform.position = other.transform.position + new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            allySmall.InitAlly();
            allySmall.IsGrabbed = false;
            allySmall.gameObject.SetActive(true);
        }

        // ���� ������Ʈ ��Ȱ��ȭ
        gameObject.SetActive(false);
    }

    void SetItemTimer()
    {
        if(gameObject.activeSelf)
            gameObject.SetActive(false);
    }
}
