using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    [SerializeField] Text stage;
    [SerializeField] Text scoreText;

    //se
    public AudioClip retry; //汎用決定音
    public AudioClip stageSelect; //汎用キャンセル音
    AudioSource audioSource; //SE入力にオーディオソースを使用する


    int stageCullent;
    int score;
    // Start is called before the first frame update
    void Start()
    {
        stageCullent = GameGenerator.Stageset();

        //AudioComponentを取得
        audioSource = GetComponent<AudioSource>();

        stage.text = string.Format("STAGE {0}", stageCullent);

        score = GameGenerator.Scoreset();

        scoreText.text = string.Format("Score:{0}", score);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RetryStage()
    {
        audioSource.PlayOneShot(retry);
        //画面遷移
        Initiate.DoneFading();
        Initiate.Fade("Stage"+stageCullent, Color.white, 1.0f);
    }

    public void StageSelect()
    {
        audioSource.PlayOneShot(stageSelect);
        //画面遷移
        Initiate.DoneFading();
        Initiate.Fade("StageSelect", Color.white, 1.0f);

        
    }

    public void Ranking()
    {
        audioSource.PlayOneShot(retry);

        //画面遷移
        Initiate.DoneFading();
        Initiate.Fade("Ranking", Color.white, 1.0f);
    }
}