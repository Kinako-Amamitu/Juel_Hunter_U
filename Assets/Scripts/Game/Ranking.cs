using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.UI;



public class Ranking : MonoBehaviour
{

    int currentStage;
    string rankingScore;

    [SerializeField] Text stage;
    [SerializeField] Text[] ranking;

    ////�����L���O�\���p�̃e�L�X�g�v���n�u
    //[SerializeField] GameObject rankItemPrefab;

    ////�����L���O�e�L�X�g�̐e�̃Q�[���I�u�W�F�N�g
    //[SerializeField] GameObject parentGameObject;


    public void LordRanking()
    {
        StartCoroutine(NetworkManager.Instance.GetScore(currentStage, result =>
        {
            for(int i=0;i<ranking.Length;i++)
            {
                if(result.Length<=i)
                {
                    break;
                }

                if (i==0)
                {
                    ranking[i].text = string.Format("1st:{0}", result[i].Score);
                }
                else if (i == 1)
                {
                    ranking[i].text = string.Format("2nd:{0}", result[i].Score);
                }
                else if (i == 2)
                {
                    ranking[i].text = string.Format("3rd:{0}", result[i].Score);
                }
                else
                {
                    ranking[i].text = string.Format("{0}th:{1}",i+1, result[i].Score);
                }

            }
            
        }));
    }


    //IEnumerator GetUser()
    //{
    //    string url = hostName + "/api/ranking/index";

    //    UnityWebRequest request = UnityWebRequest.Get(url);
    //    request.SetRequestHeader("x-functions-key", apiKey);
    //    yield return request.SendWebRequest();

    //    if (request.result == UnityWebRequest.Result.Success)
    //    {//���������ꍇ
    //        string json = request.downloadHandler.text;

    //        //JSON����f�V���A���C�Y
    //        var userDatas = JsonConvert.DeserializeObject<Ranking_Data[]>(json);

    //        //�e�L�X�g��ݒ�
    //        for (int i = 0; i < userDatas.Length; i++)
    //        {
    //            GameObject textObj = Instantiate(rankItemPrefab,
    //             new Vector3(0, 0, 0),
    //             Quaternion.identity,
    //             parentGameObject.transform);
    //            textObj.GetComponent<Text>().text = string.Format("{0}",userDatas[i].Score); //API����擾�����X�R�A�ɂ�����
    //        }

    //    }
    //    else
    //    {//���s�����ꍇ
    //        GameObject textObj = Instantiate(rankItemPrefab,
    //        new Vector3(0, 0, 0),
    //        Quaternion.identity,
    //        parentGameObject.transform);
    //        textObj.GetComponent<Text>().text = "�G���[���b�Z�[�W!!"; //API����擾�������O�E�X�R�A�ɂ�����

    //    }
    //}

    // Start is called before the first frame update
    void Start()
    {
        currentStage = GameGenerator.Stageset();
        stage.text = string.Format("STAGE: {0}", currentStage);

        LordRanking();
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void StageSelect()
    {
 
            Initiate.DoneFading();

            Initiate.Fade("StageSelect", Color.black, 1.0f);
    }

    public void Title()
    {
        Initiate.DoneFading();

        Initiate.Fade("Title", Color.black, 1.0f);
    }
}
