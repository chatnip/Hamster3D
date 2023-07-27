using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New InteractableItemBase", menuName = "Hamster/Create New InteractableItemBase")]
public class InteractableItemBase : ScriptableObject
{
    [Header("*Setting")]
    [SerializeField] string interactName;
    [SerializeField] string interactString;
    [SerializeField] Interact interactType;

    #region Stat Setting
    public Interact InteractType
    {
        get { return interactType; }
    }
    public string InteractName
    {
        get { return interactName; }
    }

    public string InteractString
    {
        get { return interactString; }
    }

    #endregion
}

public enum Interact
{
    NPC,
    CheckPoint,
    SunFlower,
}