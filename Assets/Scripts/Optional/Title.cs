using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{

    public AudioClip start; //ゲームスタート音
    AudioSource audioSource; //SE入力にオーディオソースを使用する

    // Start is called before the first frame update
    void Start()
    {
        //AudioComponentを取得
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //タッチしたとき
        /*if (Input.touchCount > 0)
        {
            Touch t1 = Input.GetTouch(0);
            if (t1.phase == TouchPhase.Began)
            {
                
            }
        }
        */

    }

    public void GameStart()
    {
        audioSource.PlayOneShot(start);
        bool isSuccess = NetworkManager.Instance.LoadUserData();
        if (!isSuccess)
        {
            //ユーザーデータが保存されていない場合は登録
            StartCoroutine(NetworkManager.Instance.RegistUser(Guid.NewGuid().ToString(), result =>
            {
                //画面遷移
                Initiate.DoneFading();
                Initiate.Fade("StageSelect", Color.black, 0.5f);
            }));
        }
        else
        {

            //画面遷移
            Initiate.DoneFading();
            Initiate.Fade("StageSelect", Color.black, 0.5f);
        };
    }

    //ゲーム終了
    public void EndGame()
    {

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
    }
}
