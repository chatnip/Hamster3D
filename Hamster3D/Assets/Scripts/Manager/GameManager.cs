using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameManager : Manager<GameManager>
{
    [HideInInspector] public IntReactiveProperty goldenSeedValue = new IntReactiveProperty(0);

    // json���� ������ ��
    // 1. �� ���� Ȳ�� �عٶ�⾾ ���� ����
    // 2. �� ���� Ȳ�� �عٶ�⾾ ����

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
            print("�ҷ����� ����");
            string FromJsonData = File.ReadAllText(filePath);
            userDatabase = JsonUtility.FromJson<UserDatabase>(FromJsonData);
        }
        else
        {
            print("���ο� ���� ����");
            userDatabase = new UserDatabase();
        }
    }

    public void SaveData()
    {
        string ToJsonData = JsonUtility.ToJson(userDatabase);
        string filePath = Application.persistentDataPath + UserDatabase.GameDataPath;

        File.WriteAllText(filePath, ToJsonData);
        Debug.Log(ToJsonData);
        print("����Ϸ�");
    }
    */
}
