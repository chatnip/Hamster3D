using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class InteractableItem : MonoBehaviour
{
    [SerializeField] GameSystem GameSystem;
    [SerializeField] ReactiveProperty<InteractableItemBase> interactableItemBase = new ReactiveProperty<InteractableItemBase>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameSystem.OpenMessagePanel(interactableItemBase.Value.InteractString);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameSystem.CloseMessagePanel();
        }
    }
}
