using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour
{
#if DEBUG
    const string API_BASE_URL = "http://localhost:8000/api/";
#else
    const string API_BASE_URL = "https://api-juelhunter.japaneast.cloudapp.azure.com/api/";//APIのデプロイURL
#endif
    private static NetworkManager instance;

    
    private int userID = 0;  //自分のユーザーID
    private string userName = ""; //自分のユーザー名

    private int stageID; //ステージID
    private int score; //スコア

    private string authToken; //APIトークン

    private int stageClearNumber=0; //ステージクリア状況

    public int stageCullentClear
    {
        get { return stageClearNumber; }
    }

    public int UserID
    {
        get { return userID; }
    }

    public int StageID
    {
        get { return stageID; }
    }

    public int Score
    {
        get { return score; }
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

        request.SetRequestHeader("Authorization", "Bearer" + authToken);

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
            this.authToken = response.Authtoken;
            SaveUserData();
            isSuccess = true;
        }
        result?.Invoke(isSuccess); //ここで呼び出し元のresult処理を呼び出す
    }

    //ユーザー情報を保存する
    public void SaveUserData()
    {
        SaveData saveData = new SaveData();
        saveData.authToken = this.authToken;
        saveData.userName = this.userName;
        saveData.stageClearNumber = this.stageClearNumber;
        saveData.userID = this.userID;
        saveData.stage_ID = this.stageID;
        saveData.score = this.score;
        string json = JsonConvert.SerializeObject(saveData);
        var writer = new StreamWriter(Application.persistentDataPath + "/saveData.json");
        writer.Write(json);
        writer.Flush();
        writer.Close();
    }

    //トークン生成処理
    public IEnumerator CreateToken(Action<bool>responce)
    {
        var requestData = new
        {
            user_id = this.UserID
        };
        string json = JsonConvert.SerializeObject(requestData);
        UnityWebRequest request = UnityWebRequest.Post(API_BASE_URL + "users/createToken", json, "application/json");
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            //通信が成功したとき、返ってきたJSONをオブジェクトに変換
            string resultJson = request.downloadHandler.text;
            RegistUserResponse response = JsonConvert.DeserializeObject<RegistUserResponse>(resultJson);

            //ファイルにユーザーIDを保存
            this.userID = response.UserID;
            this.authToken = response.Authtoken;
            SaveUserData();
        }
        responce?.Invoke(request.result == UnityWebRequest.Result.Success);
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
        this.authToken = saveData.authToken;
        this.userID = saveData.userID;
        this.userName = saveData.userName;
        this.stageClearNumber = saveData.stageClearNumber;
        this.stageID = saveData.stage_ID;
        this.score = saveData.score;

        if(authToken==null)
        {
            
            StartCoroutine(NetworkManager.Instance.CreateToken(result =>
            {
                SaveUserData();
            }));
       
        }

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

    public IEnumerator RegistScore(int score,int stage_id,Action<RegistRankingResponse> result)
    {
        //サーバーに送信するオブジェクトを作成
        RegistRankingRequest request_data = new RegistRankingRequest();
        request_data.UserID = userID;
        request_data.Score = score;
        request_data.StageID = stage_id;

        //サーバーに送信するオブジェクトをJSONに変換
        string json = JsonConvert.SerializeObject(request_data);

        //送信
        UnityWebRequest request = UnityWebRequest.Post(
            API_BASE_URL + "ranking/store", json, "application/json");

        request.SetRequestHeader("Authorization", "Bearer " + authToken);

        yield return request.SendWebRequest();
        
        if (request.result == UnityWebRequest.Result.Success && request.responseCode == 200)
        {
            //通信が成功したとき、返ってきたJSONをオブジェクトに変換
            string resultJson = request.downloadHandler.text;
            RegistRankingResponse response = JsonConvert.DeserializeObject<RegistRankingResponse>(resultJson);

            //ファイルにランキングを保存
            this.stageID = response.StageID;
            this.userID = response.UserID;
            this.score = response.Score;
            SaveUserData();

            result?.Invoke(response); //ここで呼び出し元のresult処理を呼び出す

        }
        else
        {
            result?.Invoke(null); //ここで呼び出し元のresult処理を呼び出す
        }
        
    }

    public IEnumerator GetScore(int stage_id, Action<RegistRankingResponse[]> result)
    {

        //受信
        UnityWebRequest request = UnityWebRequest.Get(
            API_BASE_URL + "ranking/index/"+stage_id);

        request.SetRequestHeader("Authorization", "Bearer " + authToken);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success && request.responseCode == 200)
        {
            //通信が成功したとき、返ってきたJSONをオブジェクトに変換
            string resultJson = request.downloadHandler.text;
            RegistRankingResponse[] response = JsonConvert.DeserializeObject<RegistRankingResponse[]>(resultJson);


            result?.Invoke(response); //ここで呼び出し元のresult処理を呼び出す

        }
        else
        {
            result?.Invoke(null); //ここで呼び出し元のresult処理を呼び出す
        }

    }
}

