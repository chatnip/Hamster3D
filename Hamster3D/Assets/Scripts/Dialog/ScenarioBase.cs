using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scenario", menuName = "Hamster/Create New Scenario")]

public class ScenarioBase : ScriptableObject
{
    [SerializeField] List<Fragment> fragments;

    public List<Fragment> Fragments
    {
        get { return fragments; }
    }
}

[System.Serializable]
public class Fragment
{
    [SerializeField] string speaker;
    [TextArea]
    [SerializeField] string script;

    public string Speaker
    {
        get { return speaker; }
    }

    public string Script
    {
        get { return script; }
    }
}
