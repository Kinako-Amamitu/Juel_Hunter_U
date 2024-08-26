using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //タッチしたとき
        if (Input.touchCount > 0)
        {
            Touch t1 = Input.GetTouch(0);
            if (t1.phase == TouchPhase.Began)
            {
                bool isSuccess = NetworkManager.Instance.LoadUserData();
                if (!isSuccess)
                {
                    //ユーザーデータが保存されていない場合は登録
                    StartCoroutine(NetworkManager.Instance.RegistUser(Guid.NewGuid().ToString(), result => {
                        //画面遷移
                        Initiate.DoneFading();
                    Initiate.Fade("GameScene", Color.black, 0.5f);}));
                }
                else {
                    //画面遷移
                    Initiate.DoneFading();
                    Initiate.Fade("GameScene", Color.black, 0.5f);
                };
            }
        }
    }
}
