using UnityEngine;
using UnityEngine.AI;

public class CharacterStateMachine : MonoBehaviour
{
    [Header("Components")]
    public NavMeshAgent agent;
    public Animator animator;
    public CharacterStats stats;

    [Header("Target Info")]
    public Transform target;

    [Header("State")]
    public IBaseState currentState;
    public IBaseState idleState = new IdleState();
    public IBaseState chaseState = new ChaseState();
    public IBaseState attackState = new AttackState();

    [Header("State Check")]
    public bool isIdle;
    public bool isChase;
    public bool isAttack;
    public bool isHurt;
    public bool isDeath;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        stats = GetComponent<CharacterStats>();

    }

    private void Start()
    {
        RegisterInBattleManager();

        Initialize();

        if (target != null)
        {
            agent.destination = target.position;
        }

    }

    private void Update()
    {
        currentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdate();
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        UnRegisterFromBattleManager();
    }

    private void RegisterInBattleManager()
    {
        BattleManager.Instance.RegisterInList(this);
    }

    private void UnRegisterFromBattleManager()
    {
        BattleManager.Instance.UnRegisterFromList(this);
    }

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
        target = BattleManager.Instance.SearchTarget(this).transform;

        return target != null;

    }

}
