using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] ScenarioBase scenarioBase;
    [SerializeField] DialogManager DialogManager;
    [SerializeField] PlayerController PlayerController;

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            StartDialog();
        }
    }

    public void StartDialog()
    {
        Debug.Log("isScripting :" + DialogManager.isScripting);
        if(PlayerController.isAction == true && DialogManager.isScripting == false)
        {
            DialogManager.ScenarioBase.Value = scenarioBase;
        }
        return;
    }
}
