using UnityEngine;
using DG.Tweening;

public class ArcherArrow : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 1f;
    private float damage;

    [Header("Target CharBattleSystem")]
    public CharacterBattleSystem targetCharBS;

    private Vector3 targetPosition;
    private float targetDistance;

    private Tween moveTween;

    void Start()
    {
        MoveToTarget();

    }

    void Update()
    {
        // 判斷擊中
        targetDistance = Vector3.Distance(transform.position, targetPosition);

        if (targetDistance <= 0.1f)
        {
            if (targetCharBS != null)
            {
                targetCharBS.TakeDamage(damage);
            }

            moveTween?.Kill(); // 停止動畫
            Destroy(gameObject);

            return;

        }

        if (targetCharBS != null)
        {
            targetPosition = targetCharBS.transform.position;
            MoveToTarget(); // 持續重新啟動動畫
        }

    }

    public void SetTarget(CharacterBattleSystem _targetCharBS)
    {
        targetCharBS = _targetCharBS;
        targetPosition = _targetCharBS.transform.position;
    }

    public void SetDamage(float _damage)
    {
        damage = _damage;
    }

    private void MoveToTarget()
    {
        // 檢查並清除舊的動畫，避免重複啟動
        moveTween?.Kill();

        UpdateDirection();

        // 使用 SetSpeedBased 並啟用 OnUpdate 自動檢查目標位置
        moveTween = transform.DOMove(targetPosition, moveSpeed)
                             .SetSpeedBased()
                             .SetEase(Ease.Linear);


    }

    // 計算並朝向目標
    private void UpdateDirection()
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.right = direction;
    }

}
