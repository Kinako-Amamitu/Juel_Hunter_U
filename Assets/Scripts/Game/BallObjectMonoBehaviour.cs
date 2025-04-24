////////////////////////////////////////////////////////////////
///
/// オブジェクトの挙動を管理するスクリプト
/// 
/// Aughter:木田晃輔
///
////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallObject : MonoBehaviour
{
    //Unityからアタッチ
    [SerializeField]
    public new Renderer renderer; //レンダラー

    [SerializeField]
    public bool isTouch = false; //タッチ判定

    public float MoveSpeed = 20.0f;         // 移動値

    int frameCount = 0;             // フレームカウント
    const int deleteFrame = 180;    // 削除フレーム

    // Update is called once per frame
    void Update()
    {
        // 位置の更新
        transform.Translate(MoveSpeed * Time.deltaTime, 0, 0);
    }

    /// <summary>
    /// 接触判定
    /// </summary>
    /// <param name="other"></param>
    void OnCollisionEnter(Collision other)
    {
        //ボールにぶつかったとき
        if (other.gameObject.tag == "Ball")
        {
            Debug.Log("ボールにぶつかった！");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("ボールじゃないところにぶつかった！");
        }
       
    }
}