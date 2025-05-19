using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class CharacterStateMachine : MonoBehaviour
{
    [Header("Components")]
    public CharacterBattleSystem charBS;
    public CharacterStats charStats;
    public NavMeshAgent agent;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    [Header("Target Info")]
    public Transform target;

    [Header("State")]
    public IBaseState currentState;
    public IBaseState idleState = new IdleState();
    public IBaseState chaseState = new ChaseState();
    public IBaseState attackState = new AttackState();
    public IBaseState deathState = new DeathState();

    [Header("State Check")]
    public bool isIdle;
    public bool isChase;
    public bool isAttack;
    public bool isHurt;
    public bool isDead;



    private void Awake()
    {
        charBS = GetComponent<CharacterBattleSystem>();
        charStats = GetComponent<CharacterStats>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    private void Start()
    {
        Initialize();

    }

    private void Update()
    {
        if (charBS.battleController.isBattlePreparation)
            return;

        currentState.LogicUpdate();

    }

    private void FixedUpdate()
    {
        currentState.FixedUpdate();
    }

    //private void OnEnable()
    //{
        
    //}

    //private void OnDisable()
    //{
        
    //}

    

    /// <summary>
    /// ��l��
    /// </summary>
    private void Initialize()
    {
        currentState = idleState;
        currentState.Enter(this);

        agent.updateRotation = false;
        agent.updateUpAxis = false;

    }

    /// <summary>
    /// ���A����
    /// </summary>
    /// <param name="state"></param>
    public void ChangeSate(IBaseState state)
    {
        currentState.Exit();
        currentState = state;
        currentState.Enter(this);

    }

    /// <summary>
    /// ��M�̪�ؼ�
    /// </summary>
    /// <returns></returns>
    public bool SearchTarget()
    {
        if (!isDead)
        {
            CharacterBattleSystem targetCharBS = charBS.battleController.SearchTarget(charBS);
            if (targetCharBS != null && !targetCharBS.charSM.isDead)
                target = targetCharBS.transform;
            else
                target = null;
        }

        return target != null;

    }

    /// <summary>
    /// ½�ਤ��
    /// </summary>
    public void FlipCharacter()
    {
        if (target != null)
        {
            float targetDirX = target.position.x - transform.position.x;

            if (targetDirX >= 0f)
                spriteRenderer.flipX = false;
            else 
                spriteRenderer.flipX = true;

        }

    }

    /// <summary>
    /// �������`���A
    /// </summary>
    public void BeDead()
    {
        //FIXME: Tag name�O�_�ઽ������s�边��tag name�A�Ӥ��O��ʿ�J
        if (CompareTag("EnemyCharacter"))
        {
            CharacterDieEventSO.Instance.RaiseEvent();
        }

        ChangeSate(deathState);
        return;


    }

}
