////////////////////////////////////////////////////////////////
///
/// リザルトを管理するスクリプト
/// 
/// Aughter:木田晃輔
///
////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    //Unityからアタッチ
    [SerializeField] Text stage;
    [SerializeField] Text scoreText;

    //se
    public AudioClip retry; //汎用決定音
    public AudioClip stageSelect; //汎用キャンセル音
    AudioSource audioSource; //SE入力にオーディオソースを使用する


    int stageCullent; //何ステージクリアしたかの判定
    int score;//点数

    // Start is called before the first frame update
    void Start()
    {
        //クリアしたステージの情報を読み込む
        stageCullent = GameGenerator.Stageset();

        //AudioComponentを取得
        audioSource = GetComponent<AudioSource>();

        //クリアしたステージの番号を設定
        stage.text = string.Format("STAGE {0}", stageCullent);

        //スコアを取得
        score = GameGenerator.Scoreset();

        //スコアをテキストに代入
        scoreText.text = string.Format("Score:{0}", score);
    }

    /// <summary>
    /// 同じステージをリプレイする
    /// </summary>
    public void RetryStage()
    {
        //リトライ音声
        audioSource.PlayOneShot(retry);
        //画面遷移
        Initiate.DoneFading();
        Initiate.Fade("Stage"+stageCullent, Color.white, 1.0f);
    }

    /// <summary>
    /// ステージセレクトへ
    /// </summary>
    public void StageSelect()
    {
        //ステージセレクト音
        audioSource.PlayOneShot(stageSelect);
        //画面遷移
        Initiate.DoneFading();
        Initiate.Fade("StageSelect", Color.white, 1.0f);

        
    }

    /// <summary>
    /// ランキング画面へ
    /// </summary>
    public void Ranking()
    {
        //ランキング画面移動音
        audioSource.PlayOneShot(retry);

        //画面遷移
        Initiate.DoneFading();
        Initiate.Fade("Ranking", Color.white, 1.0f);
    }
}