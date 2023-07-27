using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UniRx;
using UnityEngine.InputSystem;

[System.Serializable]
public struct DialogData
{
    public Text nameText;
    public Text dialogText;
}

public class DialogManager : MonoBehaviour
{
    [Header("*Component")]
    [SerializeField] PlayerController PlayerController;

    [Header("*Dialog")]
    [SerializeField] GameObject dialog;
    [SerializeField] DialogData DialogData;
    [SerializeField] public ReactiveProperty<ScenarioBase> ScenarioBase = new ReactiveProperty<ScenarioBase>();
    [SerializeField] public bool isScripting;

    private void Awake()
    {
        isScripting = false;

        ScenarioBase
            .Where(Base => Base != null)
            .Subscribe(texting =>
            {
                StartCoroutine(DialogTexting(texting));
            });
    }

    private void DialogSetup(Fragment Scenario_Fragment)
    {
        dialog.SetActive(true);
        DialogData.nameText.text = Scenario_Fragment.Speaker;
        isScripting = true;
    }

    public IEnumerator DialogTexting(ScenarioBase scenarioBase)
    {
        DialogData.dialogText.text = null;
        DialogSetup(scenarioBase.Fragments[0]);

        for (int i = 0; i < scenarioBase.Fragments.Count; i++)
        {
            int temp = i;
            var sequence = DOTween.Sequence();
            DialogData.dialogText.text = null;
            DialogSetup(scenarioBase.Fragments[temp]);
            Fragment newFragment = scenarioBase.Fragments[temp];
          
            sequence.Append(DialogData.dialogText.DOText(newFragment.Script, newFragment.Script.Length / 10)
                    .SetEase(Ease.Linear)
                    .OnUpdate(() =>
                    {
                        if (PlayerController.isAction)
                        {
                            sequence.Complete();
                        }
                    }));

            yield return new WaitUntil(() =>
            {
                if (DialogData.dialogText.text == newFragment.Script)
                {
                    return true;
                }
                return false;
            });

            yield return new WaitForSeconds(0.2f);

            yield return new WaitUntil(() =>
            {
                if (PlayerController.isAction)
                {
                    return true;
                }
                return false;
            });
            continue;
        }
        dialog.SetActive(false);
        isScripting = false;
    }
}
