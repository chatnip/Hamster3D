using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class QMark : MonoBehaviour
{
    [SerializeField] GameObject speechBubble;

    private void Awake()
    {
        speechBubble.SetActive(false);
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            speechBubble.SetActive(true);
            transform.DOScale(1, 1).SetEase(Ease.OutBounce);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transform.DOScale(0.5f, 1).SetEase(Ease.OutBounce);
            speechBubble.SetActive(false);
        }
    }
}
