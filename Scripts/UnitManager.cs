using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType
{
    None = -1,
    Ally,
    Enemy,
    Max
}

public enum AllyType
{
    None = -1,
    AllySmall,
    AllyBig,
    Max,
}

public enum EnemyType
{
    None,
    Golem = 2,
    Troll,
    Zombie,
    Max
}
public class UnitManager : SingletonMonoBehaviour<UnitManager>
{
    public GameObject[] m_units;
    public ObjectPool<Enemy> m_enemyPool = null;
    public ObjectPool<Ally> m_allySmallPool = null;
    public ObjectPool<Ally> m_allyBigPool = null;
    Transform[] m_spawnPoints;

    float m_period = 3f;

    WaitForSeconds m_wait = null;

    int m_count;

    protected override void OnAwake()
    {
        m_count = 0;
        m_wait = new WaitForSeconds(m_period);
        m_spawnPoints = GameManager.Instance.m_spawnPoint.GetComponentsInChildren<Transform>();
        
        m_enemyPool = new ObjectPool<Enemy>(9, () => {

            var obj = Instantiate(m_units[Random.Range((int)EnemyType.Golem, (int)EnemyType.Max)]);
            obj.SetActive(false);
            obj.transform.parent = this.transform;
            var enemy = obj.GetComponent<Enemy>();
            enemy.InitEnemy();
            
            return enemy;
        });

        m_allySmallPool = new ObjectPool<Ally>(5, () =>
        {
            var obj = Instantiate(m_units[(int)AllyType.AllySmall]);
            obj.SetActive(false);
            obj.transform.parent = transform;
            var ally = obj.GetComponent<Ally>();
            ally.InitAlly();
            return ally;    
        });
        m_allyBigPool = new ObjectPool<Ally>(5, () =>
        {
            var obj = Instantiate(m_units[(int)AllyType.AllyBig]);
            obj.SetActive(false);
            obj.transform.parent = transform;
            var ally = obj.GetComponent<Ally>();
            ally.InitAlly();
            return ally;
        });
    }

    protected override void OnStart()
    {
        StartCoroutine(Coroutine_Spawn());
    }

    IEnumerator Coroutine_Spawn()
    {
        yield return new WaitForSeconds(30f);

        while(true)
        {

            for (int i = 1; i < m_spawnPoints.Length; i++)
            {
                var enemy = m_enemyPool.Get();
                enemy.transform.position = m_spawnPoints[i].position;
                enemy.InitEnemy();
                enemy.gameObject.SetActive(true);
                m_count++;
            }

            if (m_count != 9)
            {
                yield return m_wait;
            }
            else
            {
                m_count = 0;
                yield return new WaitForSeconds(10f);
            }
            
        }
    }

    public Ally GetAllySmall()
    {
        var allySmall = m_allySmallPool.Get();
        allySmall.InitAlly();
        return allySmall;
    }

    public Ally GetAllyBig()
    {
        var allyBig = m_allyBigPool.Get();
        allyBig.InitAlly();
        return allyBig;
    }

    public void RemoveEnemy(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        m_enemyPool.Set(enemy);
    }

    public void RemoveAlly(Ally ally)
    {
        ally.gameObject.SetActive(false);
        m_allySmallPool.Set(ally);
    }
}
