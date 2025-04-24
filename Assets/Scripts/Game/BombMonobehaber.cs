////////////////////////////////////////////////////////////////
///
/// 爆弾オブジェクトを管理するスクリプト
/// 
/// Aughter:木田晃輔
///
////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombMonobehaber : MonoBehaviour
{
    //Unityからアタッチ
    [SerializeField] GameObject bomb; //ボムプレハブ

    //弾のクラス使用
    Bullet bullet;


    private void Start()
    {

        bullet = GetComponent<Bullet>();



    }

    /// <summary>
    /// レイ可視化
    /// </summary>
    void OnDrawGizmos()
    {
        //　CircleCastのレイを可視化
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere((Vector2)transform.position , 1.0f );
    }

    /// <summary>
    /// トリガー接触判定
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Rail(right)" ||
            collision.gameObject.tag == "Rail(left)"||
            collision.gameObject.tag == "Rail(down)" ||
            collision.gameObject.tag == "Rail(down)(slow)" ||
            collision.gameObject.tag == "Rail(up)"||
            collision.gameObject.tag == "Enemy")
        {//レールか敵にあたった

            //爆発
            bullet.Explosion(bomb);
            
        }
    }
}
