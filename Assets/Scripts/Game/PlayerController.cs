////////////////////////////////////////////////////////////////
///
/// プレイヤーの動作を管理するスクリプト
/// 
/// Aughter:木田晃輔
///
////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーのプレハブにアタッチするクラス
/// </summary>
public class PlayerController : MonoBehaviour
{
    //Unityから取得
    [SerializeField] GameGenerator gameGenerator; //ゲームマネージャー取得
    [SerializeField] string playerTag;            //プレイヤーの認識タグ取得
    [SerializeField] int playerNum;               //プレイヤーの総数取得


    //ゲームをプレイしているかの判定
   public bool isplayerMode = false;
    

    //毎フレーム更新する
    private void Update()
    {
        if(isplayerMode==true)
        {//ゲームを中断したりクリア、ゲームオーバーなら
            return;
        }

        /* マウスクリックを検知 */
        if (Input.GetMouseButtonDown(0))
        {

            // タップした場所からRayを作成
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Raycastを作成
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

            //Rayが何かに衝突したことを検知 & 衝突した対象が自分自身かを判別
            if (hit2d && hit2d.transform.gameObject.tag == playerTag)
            {
                //ジュエルを撃つ
                gameGenerator.FireBullet(playerNum);
            }

        }
    }

    /// <summary>
    /// プレイヤーの進行方向の取得用
    /// </summary>
    /// <returns></returns>
    public Vector3 GetLookDirection()
    {
        //プレイヤーの向きを取得して返す
        Vector3 direction = new Vector3(-Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad), Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad));
        return direction;
    }

}
