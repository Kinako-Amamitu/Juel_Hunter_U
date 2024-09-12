using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    [SerializeField] Text stage;
    [SerializeField] Text score;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StageSelect()
    {
        //‰æ–Ê‘JˆÚ
        Initiate.DoneFading();
        Initiate.Fade("StageSelect", Color.white, 1.0f);
    }

    public void SetScore(int stageNum,int scoreNum)
    {
        stage.text = stageNum.ToString();

        score.text = scoreNum.ToString();

    }
}