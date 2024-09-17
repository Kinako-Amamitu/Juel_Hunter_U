using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    [SerializeField] Text stage;
    [SerializeField] Text scoreText;

    int stageCullent;
    int score;
    // Start is called before the first frame update
    void Start()
    {
        stageCullent = GameGenerator.Stageset();

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
        //‰æ–Ê‘JˆÚ
        Initiate.DoneFading();
        Initiate.Fade("Stage"+stageCullent, Color.white, 1.0f);
    }

    public void StageSelect()
    {
        //‰æ–Ê‘JˆÚ
        Initiate.DoneFading();
        Initiate.Fade("StageSelect", Color.white, 1.0f);
    }
}