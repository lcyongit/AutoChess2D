using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStatsDataSO", menuName = "Scriptable Objects/Data/CharacterStatsDataSO")]
public class CharacterStatsDataSO : ScriptableObject
{
    [Header("Stats")]
    public float maxHp;
    public float maxSkillGauge;

    public float attackDamage01;
    public float attackRange01;
    
    public float attackDamage02;
    public float attackRange02;
    
    public float attackDamage03;
    public float attackRange03;

    public float defensePower;

}
