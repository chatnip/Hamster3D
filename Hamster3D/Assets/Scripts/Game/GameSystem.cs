using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UniRx;

public class GameSystem : MonoBehaviour
{
    [Header("*Components")]
    [SerializeField] GameManager GameManager;
    [SerializeField] PlayerController PlayerController;

    [Header("*HUD")]
    [SerializeField] public GameObject health;
    [SerializeField] Text bulletText;
    [SerializeField] GameObject messagePanel;
    [SerializeField] Text messageText;

    [Header("*CheckPoint")]
    [SerializeField] public GameObject[] checkPoint; // ChapterBase의 체크포인트를 가져오게 할 것.
    [SerializeField] public bool[] checking;

    [Header("*GameOver")]
    [SerializeField] GameObject gameOverWindow;
    [HideInInspector] public BoolReactiveProperty isGameOver = new BoolReactiveProperty();
    [SerializeField] Button overRestartButton;
    [SerializeField] Button overLobbyButton;

    [Header("*GameClear")]
    [SerializeField] GameObject gameClearWindow;
    [SerializeField] Text goldenSeedValueText;
    [SerializeField] Button clearLobbyButton;
    [HideInInspector] public BoolReactiveProperty isGameClear = new BoolReactiveProperty();

    public void OpenMessagePanel(string text)
    {
        messagePanel.SetActive(true);
        messageText.text = text;
    }

    public void CloseMessagePanel()
    {
        messagePanel.SetActive(false);
    }
    private void Awake()
    {
        PlayerController.seedValue
            .Subscribe(x =>
            {
                bulletText.text = "Seed : " + x.ToString();
            });

        GameManager.goldenSeedValue
            .Subscribe(x =>
            {
                goldenSeedValueText.text = "총 " + x + "개를 모았다!";
            });

        #region Game Window
        isGameClear
            .Where(_ => _ == true)
            .Subscribe(x =>
            {
                gameClearWindow.SetActive(true);
            });

        isGameOver
            .Where(_ => _ == true)
            .Subscribe(x =>
            {
                gameOverWindow.SetActive(true);
            });

        overRestartButton
            .OnClickAsObservable()
            .Subscribe(x =>
            {
                SceneManager.LoadScene("Main");
            });

        overLobbyButton
            .OnClickAsObservable()
            .Subscribe(x =>
            {
                // 로비 만들면 로비로 이동되도록 변경할 것
                SceneManager.LoadScene("Title");
            });

        clearLobbyButton
            .OnClickAsObservable()
            .Subscribe(x =>
            {
                // 위와 동일
                SceneManager.LoadScene("Title");
            });
        #endregion
    }

    public void CheckPoint(GameObject target)
    {
        if(checking[0] == false)
        {
            // 처음 체크포인트도 지나지 못했다면
            // ChapterManager에서 플레이어 시작좌표 가져오기
            target.transform.position = new Vector3(-1.5f, 4f, 13f);
        }
        else if(checking[checking.Length - 1] == true)
        {
            target.transform.position = checkPoint[checking.Length - 1].transform.position;
        }
        else
        {
            for(int i = 1; i < checking.Length; i++)
            {
                if(checking[i] == false)
                {
                    target.transform.position = checkPoint[i - 1].transform.position;
                }
            }
        }
    }
}
