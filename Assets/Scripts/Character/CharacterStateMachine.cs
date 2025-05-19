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
    /// 初始化
    /// </summary>
    private void Initialize()
    {
        currentState = idleState;
        currentState.Enter(this);

        agent.updateRotation = false;
        agent.updateUpAxis = false;

    }

    /// <summary>
    /// 狀態切換
    /// </summary>
    /// <param name="state"></param>
    public void ChangeSate(IBaseState state)
    {
        currentState.Exit();
        currentState = state;
        currentState.Enter(this);

    }

    /// <summary>
    /// 找尋最近目標
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
    /// 翻轉角色
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
    /// 偵測死亡狀態
    /// </summary>
    public void BeDead()
    {
        //FIXME: Tag name是否能直接抓取編輯器的tag name，而不是手動輸入
        if (CompareTag("EnemyCharacter"))
        {
            CharacterDieEventSO.Instance.RaiseEvent();
        }

        ChangeSate(deathState);
        return;


    }

}
