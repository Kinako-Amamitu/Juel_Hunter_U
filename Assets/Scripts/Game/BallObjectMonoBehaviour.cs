using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallObject : MonoBehaviour
{
    [SerializeField]
    public new Renderer renderer;

    [SerializeField]
    public bool isTouch = false;

    public float MoveSpeed = 20.0f;         // 移動値

    int frameCount = 0;             // フレームカウント
    const int deleteFrame = 180;    // 削除フレーム

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        // 位置の更新
        transform.Translate(MoveSpeed * Time.deltaTime, 0, 0);

        // 一定フレームで消す
        //if (++frameCount > deleteFrame)
        //{
        //    Destroy(gameObject);
        //}
    }


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