using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UniRx;
using UniRx.Triggers;

public class Enemy : MonoBehaviour
{
    [Header("*Components")]
    [SerializeField] PlayerController PlayerController;
    [SerializeField] ObjectManager ObjectManager;

    [Header("*Enemy")]
    [SerializeField] NavMeshAgent nav;
    [SerializeField] float trackingValue;
    [SerializeField] GameObject health;
    FloatReactiveProperty sqrLen = new FloatReactiveProperty();

    [Header("*Status")]
    [SerializeField] ReactiveProperty<EnemyBase> enemyBase = new ReactiveProperty<EnemyBase>();
    Dictionary<Stat, float> EnemyStatPerValue = new Dictionary<Stat, float>();
    IntReactiveProperty CurrentHP = new IntReactiveProperty();

    public int GetMaxHP { get { return (int)GetStat(Stat.MaxHp); } }
    public float GetMoveSpeed { get { return GetStat(Stat.MoveSpeed); } }


    private void Awake()
    {
        ResetStat();

        this.UpdateAsObservable()
            .Subscribe(x =>
            {
                PlayerDistance();
                nav.SetDestination(PlayerController.transform.position);
            });

        sqrLen
            .Subscribe(x =>
            {
                if(x > trackingValue)
                {
                    nav.isStopped = true;
                }
                else
                {
                    nav.isStopped = false;
                }
            });

        CurrentHP
            .ThrottleFirst(System.TimeSpan.FromSeconds(0.5f))
            .Subscribe(hp =>
            {
                if (hp > 0)
                {
                    SetHPbar(hp);
                    return;
                }
                SetHPbar(0);
                EnemyDie();
            });
    }

    private void ResetStat()
    {
        EnemyStatPerValue.Clear();
        EnemyStatPerValue.Add(Stat.MaxHp, enemyBase.Value.MaxHp);
        EnemyStatPerValue.Add(Stat.MoveSpeed, enemyBase.Value.MoveSpeed);

        CurrentHP.Value = enemyBase.Value.MaxHp;
    }

    private float GetStat(Stat stat)
    {
        if (enemyBase.Value == null) return 0;
        float statValue = EnemyStatPerValue[stat];
        return statValue;
    }
    protected virtual void SetHPbar(int currentHP)
    {
        float HpBar_X_Scale = (float)currentHP / GetMaxHP;
        HpBar_X_Scale = Mathf.Clamp(HpBar_X_Scale, 0, 1);
        health.transform.localScale = new Vector3(HpBar_X_Scale, 1f, 1f);
    }

    private void PlayerDistance()
    {
        Vector3 offset = PlayerController.transform.position - gameObject.transform.position;
        sqrLen.Value = offset.sqrMagnitude;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PlayerBullet"))
        {
            int damage = PlayerController.damage.Value;
            CurrentHP.Value -= damage;
        }
    }
    public void EnemyDie()
    {
        nav.isStopped = true;
        ObjectManager.EnemyObjectPick(this);
    }
}
