using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameManager : Manager<GameManager>
{
    [HideInInspector] public IntReactiveProperty goldenSeedValue = new IntReactiveProperty(0);

    // json으로 저장할 것
    // 1. 각 맵의 황금 해바라기씨 얻음 여부
    // 2. 총 모은 황금 해바라기씨 갯수

    /*
    [Header("*Database")]
    [SerializeField] public UserDatabase userDatabase;
    public UserDatabase UserDatabase
    {
        get
        {
            if (userDatabase == null)
            {
                LoadData();
                SaveData();
            }
            return userDatabase;
        }
    }

    private void LoadData()
    {
        string filePath = Application.persistentDataPath + UserDatabase.GameDataPath;
        if (File.Exists(filePath))
        {
            print("불러오기 성공");
            string FromJsonData = File.ReadAllText(filePath);
            userDatabase = JsonUtility.FromJson<UserDatabase>(FromJsonData);
        }
        else
        {
            print("새로운 파일 생성");
            userDatabase = new UserDatabase();
        }
    }

    public void SaveData()
    {
        string ToJsonData = JsonUtility.ToJson(userDatabase);
        string filePath = Application.persistentDataPath + UserDatabase.GameDataPath;

        File.WriteAllText(filePath, ToJsonData);
        Debug.Log(ToJsonData);
        print("저장완료");
    }
    */
}
