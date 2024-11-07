using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Data Source SO")]
    public CharacterStatsDataSO characterStatsDataSO;

    [Header("Current Stats")]
    public float currentHp;
    public float currentSkillGauge;

    public float currentAttackDamage;
    public float currentAttackRange;

    public float currentDefensePower;

    private void Start()
    {
        SetCurrentStats();

    }

    private void SetCurrentStats()
    {
        currentHp = characterStatsDataSO.maxHp;
        currentSkillGauge = characterStatsDataSO.maxSkillGauge;
        currentAttackDamage = characterStatsDataSO.attackDamage01;
        currentAttackRange = characterStatsDataSO.attackRange01;
        currentDefensePower = characterStatsDataSO.defensePower;

    }
}
