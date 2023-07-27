using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    [SerializeField] List<Bullet> bulletPrefabs = new List<Bullet>();
    [SerializeField] Queue<Bullet> bulletObjectesQueue = new Queue<Bullet>();
    [SerializeField] List<Enemy> enemyPrefabs = new List<Enemy>();
    [SerializeField] Queue<Enemy> enemyObjectesQueue = new Queue<Enemy>();

    private void Awake()
    {
        SetupQueue();
    }

    private void SetupQueue()
    {
        for (int i = 0; i < bulletPrefabs.Count; i++)
        {
            bulletObjectesQueue.Enqueue(bulletPrefabs[i]);
            bulletPrefabs[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            enemyObjectesQueue.Enqueue(enemyPrefabs[i]);
            enemyPrefabs[i].gameObject.SetActive(false);
        }
    }

    #region Bullet
    public Bullet BulletObjectPool()
    {
        var bulletObject = bulletObjectesQueue.Dequeue();
        bulletObject.gameObject.SetActive(true);
        return bulletObject;
    }
    public void BulletObjectPick(Bullet bulletObject)
    {
        bulletObjectesQueue.Enqueue(bulletObject);
        bulletObject.gameObject.SetActive(false);
    }
    #endregion

    #region Enemy
    public Enemy EnemyObjectPool()
    {
        var enemyObject = enemyObjectesQueue.Dequeue();
        return enemyObject;
    }
    public void EnemyObjectPick(Enemy enemyObject)
    {
        enemyObjectesQueue.Enqueue(enemyObject);
        enemyObject.gameObject.SetActive(false);
    }
    #endregion
}
