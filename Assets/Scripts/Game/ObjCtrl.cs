using UnityEngine;
using System.Collections;

public class ObjCtrl : MonoBehaviour
{
    //ゲームオブジェクトの変数
    public GameObject obj;

    //クラス継承
    GameGenerator gameGenerator;

    //回転用
    Vector2 sPos;   //タッチした座標
    Quaternion sRot;//タッチしたときの回転
    float wid, hei, diag;  //スクリーンサイズ
    float tx, ty;    //変数

    public bool isgameMode = false; //ゲームの状態
    public bool isrotate = false; //回転の状態

    void Start()
    {
        wid = Screen.width;//横
        hei = Screen.height;//縦
        diag = Mathf.Sqrt(Mathf.Pow(wid, 2) + Mathf.Pow(hei, 2));//スワイプ時の力

        //ゲームジェネレーターを探す
        gameGenerator = GameObject.Find("GameGenerator").GetComponent<GameGenerator>();
    }

    void Update()
    {
       
        if (isgameMode)
        {//ゲームの状態
            return;
        }

        if (gameGenerator.isgameOver == true)
        {//ゲームオーバー時
            return;
        }


        if (Input.touchCount > 0)
        {//タッチ入力されていたら
            
            //回転
            Touch t1 = Input.GetTouch(0);

           

            if (t1.phase == TouchPhase.Began)
            {//タッチ
                sPos = t1.position;
                sRot = obj.transform.rotation;
            }
            else if (t1.phase == TouchPhase.Moved || t1.phase == TouchPhase.Stationary)
            {//スワイプ
                tx = (t1.position.x - sPos.x) / wid; //横移動量(-1<tx<1)
                ty = (t1.position.y - sPos.y) / hei; //縦移動量(-1<ty<1)
                obj.transform.rotation = sRot;
                obj.transform.Rotate(new Vector3(0, 0, -360 * tx), Space.World);
            }
        }

    }

    /// <summary>
    /// ゲームの状態更新
    /// </summary>
    public void GameModeChange()
    {
        if(isgameMode==false)
        {
            isgameMode = true;
        }
        else if (isgameMode == true)
        {
            isgameMode = false;
        }
    }
}