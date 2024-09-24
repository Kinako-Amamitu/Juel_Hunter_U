using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour
{

    private static NetworkManager instance;

    const string API_BASE_URL = "https://api-juelhunter.japaneast.cloudapp.azure.com/api/";
    private int userID = 0;  //自分のユーザーID
    private string userName = ""; //自分のユーザー名

    private int stageClearNumber=0; //ステージクリア状況

    public int stageCullentClear
    {
        get { return stageClearNumber; }
    }

    public static NetworkManager Instance
    {
        get
        {
            if(instance==null)
            {
                GameObject gameObj = new GameObject("NetworkManager");
                instance = gameObj.AddComponent<NetworkManager>();
                DontDestroyOnLoad(gameObj);
            }
            return instance;
        }
    }

    //ユーザー登録処理
    public IEnumerator RegistUser(string name, Action<bool> result)
    {
        //サーバーに送信するオブジェクトを作成
        RegistUserRequest request_data = new RegistUserRequest();
        request_data.Name = name;

        //サーバーに送信するオブジェクトをJSONに変換
        string json = JsonConvert.SerializeObject(request_data);

        //送信
        UnityWebRequest request = UnityWebRequest.Post(
            API_BASE_URL + "users/store", json, "application/json");

        yield return request.SendWebRequest();
        bool isSuccess = false;
        if(request.result==UnityWebRequest.Result.Success&& request.responseCode == 200)
        {
            //通信が成功したとき、返ってきたJSONをオブジェクトに変換
            string resultJson = request.downloadHandler.text;
            RegistUserResponse response = JsonConvert.DeserializeObject<RegistUserResponse>(resultJson);

            //ファイルにユーザーIDを保存
            this.userName = name;
            this.userID = response.UserID;
            SaveUserData();
            isSuccess = true;
        }
        result?.Invoke(isSuccess); //ここで呼び出し元のresult処理を呼び出す
    }

    //ユーザー情報を保存する
    private void SaveUserData()
    {
        SaveData saveData = new SaveData();
        saveData.userName = this.userName;
        saveData.stageClearNumber = this.stageClearNumber;
        saveData.userID = this.userID;
        string json = JsonConvert.SerializeObject(saveData);
        var writer = new StreamWriter(Application.persistentDataPath + "/saveData.json");
        writer.Write(json);
        writer.Flush();
        writer.Close();
    }

    public bool LoadUserData()
    {
        if (!File.Exists(Application.persistentDataPath + "/saveData.json"))
        {
            return false;
        }

        var reader =
            new StreamReader(Application.persistentDataPath + "/saveData.json");
        string json = reader.ReadToEnd();
        reader.Close();
        SaveData saveData = JsonConvert.DeserializeObject<SaveData>(json);
        this.userID = saveData.userID;
        this.userName = saveData.userName;
        this.stageClearNumber = saveData.stageClearNumber;

        return true;
    }

    public void StageProgress(int number)
    {
        if(number > stageClearNumber)
        {
            stageClearNumber++;
            SaveUserData();
        }
    }
}

