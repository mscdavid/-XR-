using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ItemManager : SingletonMonoBehaviour<ItemManager>
{
    [SerializeField]
    GameObject m_itemZone;

    [SerializeField]
    [Range(0f, 1f)]
    float m_dropRatio;

    public ObjectPool<ItemZone> m_itemPool = null;

    protected override void OnAwake()
    {
        m_itemPool = new ObjectPool<ItemZone>(3, () =>
        {
            var obj = Instantiate(m_itemZone);
            obj.transform.parent = transform;
            obj.SetActive(false);
            var item = obj.GetComponent<ItemZone>();
            return item;
        });
    }

    public void DropItem(Enemy enemy)
    {
        var ratio = Random.value;

        if(ratio <= m_dropRatio)
        {
            var item = m_itemPool.Get(); 
            item.transform.position = enemy.transform.position;
            item.gameObject.SetActive(true);
            
        }
    }
}
