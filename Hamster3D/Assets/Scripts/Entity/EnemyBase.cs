using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Hamster/Create New Enemy")]
public class EnemyBase : ScriptableObject
{
    [Header("*Stat Setting")]
    [SerializeField] int maxHp;
    [SerializeField] float moveSpeed;

    #region Stat Setting
    public int MaxHp
    {
        get { return maxHp; }
    }

    public float MoveSpeed
    {
        get { return moveSpeed; }
    }

    #endregion
}

public enum Stat
{
    MaxHp,
    MoveSpeed,
}