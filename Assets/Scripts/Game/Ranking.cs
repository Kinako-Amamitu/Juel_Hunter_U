////////////////////////////////////////////////////////////////
///
/// ランキング画面を管理するスクリプト
/// 
/// Aughter:木田晃輔
///
////////////////////////////////////////////////////////////////

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
    [SerializeField] GameObject[] rankingSendButton; //ランキングの遷移ボタン

    /// <summary>
    /// 音関連
    /// </summary>
    AudioSource audioSource; //BGM,SE入力にオーディオソースを使用する

    public AudioClip select; //汎用決定音
    public AudioClip cancel; //汎用キャンセル音

    bool rankingRug=false; //遷移後の遅延


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
        {//ステージ１で左を押した場合
            currentStage = 1;
            stage.text = string.Format("STAGE: {0}", currentStage);
        }
        else
        {
            if(currentStage==11)
            {//ステージ10で右を押した場合
                stage.text = string.Format("3minMode");
            }
            else if(currentStage == 12)
            {//5minModeで右を押した場合
                stage.text = string.Format("5minMode");
            }
            else
            {
                stage.text = string.Format("STAGE: {0}", currentStage);
            }
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

        if (currentStage == 11)
        {//5minModeで左を押した場合
            stage.text = string.Format("3minMode");
        }
        else if (currentStage == 12)
        {
            stage.text = string.Format("5minMode");
        }
        else
        {
            stage.text = string.Format("STAGE: {0}", currentStage);
        }

        for (int i = 0; i < ranking.Length; i++)
        {//前のランキングは消しておく
            rankingCurrent[i].text = null;
            ranking[i].text ="";
        }

        rankingRug = true;

        //ランキングを読み込む
        LordRanking();

    }

    private void Update()
    {
        if(rankingRug==true)
        {//少し遅らせてランキング表示
            ButtonDown();
            rankingRug = false;
            Invoke(nameof(ButtonUp), 1.0f);
        }
    }

    /// <summary>
    /// ボタン非表示
    /// </summary>
    private void ButtonDown()
    {
        for(int i=0;i<rankingSendButton.Length;i++)
        {
            rankingSendButton[i].SetActive(false);
        }
        
    }

    /// <summary>
    /// ボタン表示
    /// </summary>
    private void ButtonUp()
    {
        for (int i = 0; i < rankingSendButton.Length; i++)
        {
            rankingSendButton[i].SetActive(true);
        }
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

        if (currentStage == 11)
        {//11ステージが選ばれた
            stage.text = string.Format("3minMode");
        }
        else if (currentStage == 12)
        {//12ステージが選ばれた
            stage.text = string.Format("5minMode");
        }
        else
        {
            stage.text = string.Format("STAGE: {0}", currentStage);
        }

        for (int i = 0; i < ranking.Length; i++)
        {//前のランキングは消しておく
            rankingCurrent[i].text = null;
            ranking[i].text = "";
        }

        rankingRug = true;

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
