using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;

public class TitleManager : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] Button exitButton;

    private void Awake()
    {
        startButton
            .OnClickAsObservable()
            .Subscribe(x =>
            {
                SceneManager.LoadScene("Main");
            });

        exitButton
            .OnClickAsObservable()
            .Subscribe(x =>
            {
                Application.Quit();
            });
    }
}
