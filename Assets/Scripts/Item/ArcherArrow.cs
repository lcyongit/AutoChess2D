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
        // �P�_����
        targetDistance = Vector3.Distance(transform.position, targetPosition);

        if (targetDistance <= 0.1f)
        {
            if (targetCharBS != null)
            {
                targetCharBS.TakeDamage(damage);
            }

            moveTween?.Kill(); // ����ʵe
            Destroy(gameObject);

            return;

        }

        if (targetCharBS != null)
        {
            targetPosition = targetCharBS.transform.position;
            MoveToTarget(); // ���򭫷s�Ұʰʵe
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
        // �ˬd�òM���ª��ʵe�A�קK���ƱҰ�
        moveTween?.Kill();

        UpdateDirection();

        // �ϥ� SetSpeedBased �ñҥ� OnUpdate �۰��ˬd�ؼЦ�m
        moveTween = transform.DOMove(targetPosition, moveSpeed)
                             .SetSpeedBased()
                             .SetEase(Ease.Linear);


    }

    // �p��ô¦V�ؼ�
    private void UpdateDirection()
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.right = direction;
    }

}
