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
    [SerializeField] Text[] rankingCurrent;

    AudioSource audioSource; //BGM,SE入力にオーディオソースを使用する

    public AudioClip select; //汎用決定音
    public AudioClip cancel; //汎用キャンセル音

    ////ランキング表示用のテキストプレハブ
    //[SerializeField] GameObject rankItemPrefab;

    ////ランキングテキストの親のゲームオブジェクト
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
                    // 色を指定
                    rankingCurrent[i].color = new Color(1.0f, 0.92f, 0.016f, 1.0f);
                    rankingCurrent[i].text = string.Format("1st");
                    // 色を指定
                    ranking[i].color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
                    ranking[i].text += string.Format("{0}",result[i].Score);
                }
                else if (i == 1)
                {
                    // 色を指定
                    rankingCurrent[i].color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
                    rankingCurrent[i].text = string.Format("2nd");
                    // 色を指定
                    ranking[i].color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
                    ranking[i].text += string.Format("{0}", result[i].Score);
                }
                else if (i == 2)
                {
                    // 色を指定
                    rankingCurrent[i].color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
                    rankingCurrent[i].text = string.Format("3rd");
                    // 色を指定
                    ranking[i].color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
                    ranking[i].text += string.Format("{0}", result[i].Score);
                }
                else
                {
                    // 色を指定
                    rankingCurrent[i].color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
                    rankingCurrent[i].text = string.Format("{0}th",i+1);
                    // 色を指定
                    ranking[i].color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
                    ranking[i].text += string.Format("{0}", result[i].Score);
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
    //    {//成功した場合
    //        string json = request.downloadHandler.text;

    //        //JSONからデシリアライズ
    //        var userDatas = JsonConvert.DeserializeObject<Ranking_Data[]>(json);

    //        //テキストを設定
    //        for (int i = 0; i < userDatas.Length; i++)
    //        {
    //            GameObject textObj = Instantiate(rankItemPrefab,
    //             new Vector3(0, 0, 0),
    //             Quaternion.identity,
    //             parentGameObject.transform);
    //            textObj.GetComponent<Text>().text = string.Format("{0}",userDatas[i].Score); //APIから取得したスコアにしたい
    //        }

    //    }
    //    else
    //    {//失敗した場合
    //        GameObject textObj = Instantiate(rankItemPrefab,
    //        new Vector3(0, 0, 0),
    //        Quaternion.identity,
    //        parentGameObject.transform);
    //        textObj.GetComponent<Text>().text = "エラーメッセージ!!"; //APIから取得した名前・スコアにしたい

    //    }
    //}

    // Start is called before the first frame update
    void Start()
    {
        //AudioComponentを取得
        audioSource = GetComponent<AudioSource>();

        currentStage = GameGenerator.Stageset();
        if (currentStage == 0)
        {
            currentStage = 1;
            stage.text = string.Format("STAGE: {0}", currentStage);
        }
        else
        {
            stage.text = string.Format("STAGE: {0}", currentStage);
        }
        

        LordRanking();
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void RankingDown()
    {
        if(currentStage==1)
        {
            currentStage += 11;
        }
        else
        {
            currentStage--;
        }
        stage.text = string.Format("STAGE: {0}", currentStage);

        for (int i = 0; i < ranking.Length; i++)
        {
            rankingCurrent[i].text = null;
            ranking[i].text ="";
        }
            LordRanking();

        
    }

    public void RankingUp()
    {
        if(currentStage==12)
        {
            currentStage -= 11;
        }
        else
        {
            currentStage++;
        }
        stage.text = string.Format("STAGE: {0}", currentStage);
        for (int i = 0; i < ranking.Length; i++)
        {
            rankingCurrent[i].text = null;
            ranking[i].text = "";
        }
        LordRanking();

        
    }

    public void StageSelect()
    {
        audioSource.PlayOneShot(select);
            Initiate.DoneFading();

            Initiate.Fade("StageSelect", Color.black, 1.0f);
    }

    public void Title()
    {
        audioSource.PlayOneShot(cancel);
        Initiate.DoneFading();

        Initiate.Fade("Title", Color.black, 1.0f);
    }
}
