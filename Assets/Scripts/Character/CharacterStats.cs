using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Character Name")]
    public CharacterName characterName;

    [Header("Current Stats")]
    public float currentHp;
    public float currentSkillGauge;
    public float currentMoveSpeed;

    public float currentAttackDamage;
    public float currentAttackInterval;
    public float currentAttackRange;

    public float currentDefensePower;

    [Header("Setup Stats")]
    public float maxHp;
    public float maxSkillGauge;

    //TODO: 加入星級or等級


    private void Awake()
    {
        

    }

    private void Start()
    {
        SetCurrentStats();
    }


    private void SetCurrentStats()
    {
        var charDetails = GameDataManager.Instance.characterDataListSO.GetCharacterDetails(characterName);

        currentHp = charDetails.maxHp;
        currentSkillGauge = charDetails.maxSkillGauge;
        currentMoveSpeed = charDetails.moveSpeed;

        currentAttackDamage = charDetails.attackDamage01;
        currentAttackInterval = charDetails.attackInterval01;
        currentAttackRange = charDetails.attackRange01;

        currentDefensePower = charDetails.defensePower;

        maxHp = charDetails.maxHp;
        maxSkillGauge = charDetails.maxSkillGauge;


    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, currentAttackRange);

    }


}
