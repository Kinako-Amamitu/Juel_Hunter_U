using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.UI;



public class Ranking : MonoBehaviour
{
    //ステージの情報
    int currentStage;

    /// <summary>
    /// Unityアタッチ変数
    /// </summary>
    [SerializeField] Text stage; //ステージ
    [SerializeField] Text[] ranking; //ランキング(スコア)
    [SerializeField] Text[] rankingCurrent; //ランキングの順位

    /// <summary>
    /// 音関連
    /// </summary>
    AudioSource audioSource; //BGM,SE入力にオーディオソースを使用する

    public AudioClip select; //汎用決定音
    public AudioClip cancel; //汎用キャンセル音


    /// <summary>
    /// ランキングを読み込む
    /// </summary>
    public void LordRanking()
    {
        //データベースからスコア取得
        StartCoroutine(NetworkManager.Instance.GetScore(currentStage, result =>
        {
            for(int i=0;i<ranking.Length;i++)
            {//ランキングの数だけ
                if(result.Length<=i)
                {//データ数がループ数を超えたら
                    break;
                }

                if (i==0)
                {//1位
                    // 色を指定
                    rankingCurrent[i].color = new Color(1.0f, 0.92f, 0.016f, 1.0f);
                    rankingCurrent[i].text = string.Format("1st");
                    // 色を指定
                    ranking[i].color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
                    ranking[i].text += string.Format("{0}",result[i].Score);
                }
                else if (i == 1)
                {//2位
                    // 色を指定
                    rankingCurrent[i].color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
                    rankingCurrent[i].text = string.Format("2nd");
                    // 色を指定
                    ranking[i].color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
                    ranking[i].text += string.Format("{0}", result[i].Score);
                }
                else if (i == 2)
                {//3位
                    // 色を指定
                    rankingCurrent[i].color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
                    rankingCurrent[i].text = string.Format("3rd");
                    // 色を指定
                    ranking[i].color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
                    ranking[i].text += string.Format("{0}", result[i].Score);
                }
                else
                {//それ以外
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

    void Start()
    {
        //AudioComponentを取得
        audioSource = GetComponent<AudioSource>();

        //ランキング表示してるステージを表示
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
        
        //ランキングを読み込む
        LordRanking();
    }

    /// <summary>
    /// 前ステージのランキング見る
    /// </summary>
    public void RankingDown()
    {
        if(currentStage==1)
        {//ステージ1の場合
            currentStage += 11;
        }
        else
        {
            currentStage--;
        }
        stage.text = string.Format("STAGE: {0}", currentStage);

        for (int i = 0; i < ranking.Length; i++)
        {//前のランキングは消しておく
            rankingCurrent[i].text = null;
            ranking[i].text ="";
        }

        //ランキングを読み込む
        LordRanking();

    }

    /// <summary>
    /// 次ステージのランキングを見る
    /// </summary>
    public void RankingUp()
    {
        if(currentStage==12)
        {//ステージ１２の場合
            currentStage -= 11;
        }
        else
        {
            currentStage++;
        }
        stage.text = string.Format("STAGE: {0}", currentStage);
        for (int i = 0; i < ranking.Length; i++)
        {//前のランキングは消しておく
            rankingCurrent[i].text = null;
            ranking[i].text = "";
        }

        //ランキングを読み込む
        LordRanking();
    }

    /// <summary>
    /// ステージセレクトへ
    /// </summary>
    public void StageSelect()
    {
        audioSource.PlayOneShot(select);
            Initiate.DoneFading();

            Initiate.Fade("StageSelect", Color.black, 1.0f);
    }

    /// <summary>
    /// タイトルへ
    /// </summary>
    public void Title()
    {
        audioSource.PlayOneShot(cancel);
        Initiate.DoneFading();

        Initiate.Fade("Title", Color.black, 1.0f);
    }
}
