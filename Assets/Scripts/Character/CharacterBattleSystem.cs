using UnityEngine;

public class CharacterBattleSystem : MonoBehaviour
{
    [Header("Components")]
    public CharacterStateMachine charSM;
    public CharacterStats charStats;
    public CharacterUIBar charUIBar;

    public BattleController battleController;

    [Header("Prefab")]
    public GameObject arrowPrefab;


    private void Awake()
    {
        battleController = FindAnyObjectByType<BattleController>();

        charSM = GetComponent<CharacterStateMachine>();
        charStats = GetComponent<CharacterStats>();
        charUIBar = GetComponent<CharacterUIBar>();

    }

    private void Start()
    {
        RegisterCharBS();

    }

    private void OnEnable()
    {
        

    }

    private void OnDisable()
    {
        UnRegisterCharBS();
    }

    /// <summary>
    /// ��ԧ��� �ʵe�ƥ�
    /// </summary>
    public void Attack()
    {
        //TODO: ���ީR���P�_��o�ޯ��q & �K�[UI Bar Update

        if (charSM.isDead)
            return;

        if (charSM.target != null)
        {
            if (Vector3.Distance(transform.position, charSM.target.position) <= charStats.currentAttackRange)
            {
                var targetCharBS = charSM.target.GetComponent<CharacterBattleSystem>();
                targetCharBS.TakeDamage(targetCharBS.charStats.currentAttackDamage);

            }
        }
    }

    /// <summary>
    /// �}�b����� �ʵe�ƥ�
    /// </summary>
    public void AttackArcher()
    {
        //TODO: ���ީR���P�_��o�ޯ��q & �K�[UI Bar Update

        if (charSM.isDead)
            return;

        if (charSM.target != null)
        {
            var fixedPos = new Vector3(0.4f, 0.345f, 0f);
            GameObject arrow = Instantiate(arrowPrefab, transform.position + fixedPos, Quaternion.identity);
            arrow.GetComponent<ArcherArrow>().SetTarget(charSM.target.GetComponent<CharacterBattleSystem>());
            arrow.GetComponent<ArcherArrow>().SetDamage(charStats.currentAttackDamage);

        }
    }

    public void TakeDamage(float damageSource)
    {
        if (charSM.isDead) return;

        if (!charSM.isDead)
            charStats.currentHp -= damageSource;

        if (charStats.currentHp <= 0)
        {
            charStats.currentHp = 0;
            charSM.BeDead();

        }

        //UI Bar�ܤ�
        charUIBar.UpdateUIBar(charStats.maxHp, charStats.currentHp, charStats.maxSkillGauge, charStats.currentSkillGauge);

    }

    public void Death()
    {


    }

    private void RegisterCharBS()
    {
        battleController.RegisterCharBS(this);
    }

    public void UnRegisterCharBS()
    {
        battleController.UnRegisterCharBS(this);
    }

}
