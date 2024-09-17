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
    private int userID = 0;  //�����̃��[�U�[ID
    private string userName = ""; //�����̃��[�U�[��

    private int stageClearNumber=0; //�X�e�[�W�N���A��

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

    //���[�U�[�o�^����
    public IEnumerator RegistUser(string name, Action<bool> result)
    {
        //�T�[�o�[�ɑ��M����I�u�W�F�N�g���쐬
        RegistUserRequest request_data = new RegistUserRequest();
        request_data.Name = name;

        //�T�[�o�[�ɑ��M����I�u�W�F�N�g��JSON�ɕϊ�
        string json = JsonConvert.SerializeObject(request_data);

        //���M
        UnityWebRequest request = UnityWebRequest.Post(
            API_BASE_URL + "users/store", json, "application/json");

        yield return request.SendWebRequest();
        bool isSuccess = false;
        if(request.result==UnityWebRequest.Result.Success&& request.responseCode == 200)
        {
            //�ʐM�����������Ƃ��A�Ԃ��Ă���JSON���I�u�W�F�N�g�ɕϊ�
            string resultJson = request.downloadHandler.text;
            RegistUserResponse response = JsonConvert.DeserializeObject<RegistUserResponse>(resultJson);

            //�t�@�C���Ƀ��[�U�[ID��ۑ�
            this.userName = name;
            this.userID = response.UserID;
            SaveUserData();
            isSuccess = true;
        }
        result?.Invoke(isSuccess); //�����ŌĂяo������result�������Ăяo��
    }

    //���[�U�[����ۑ�����
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

