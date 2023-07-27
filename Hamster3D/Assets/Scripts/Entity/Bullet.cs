using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;

public class Bullet : MonoBehaviour
{
    [Header("*Component")]
    [SerializeField] ObjectManager ObjectManager;
    [SerializeField] PlayerController PlayerController;

    [Header("*Bullet")]
    [SerializeField] float bulleSpeed;
    [SerializeField] Rigidbody bulletRigidbody;

    void OnEnable()
    {
        transform.position = PlayerController.gameObject.transform.position;
        transform.LookAt(PlayerController.playerLookVector);
        bulletRigidbody.velocity = transform.forward * bulleSpeed;
        StartCoroutine(BulletDestroy());
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("Enemy"))
        {
            ObjectManager.BulletObjectPick(this);
        }
    }

    private IEnumerator BulletDestroy()
    {
        yield return new WaitForSeconds(1f);
        ObjectManager.BulletObjectPick(this);
        yield break;
    }
}
