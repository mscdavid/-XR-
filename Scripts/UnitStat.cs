using UnityEngine;

[CreateAssetMenu(fileName = "Unit", menuName = "Scriptable Object/UnitData")]
public class UnitStat : ScriptableObject
{
    public int m_hp;
    public int m_damage;
    public float m_moveSpeed;
    public float m_attackRange;
}

public enum StateType
{
    None = -1,
    Die,
    Live,
    Patrol,
    Digging,
}