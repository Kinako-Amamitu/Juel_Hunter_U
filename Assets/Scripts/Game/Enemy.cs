////////////////////////////////////////////////////////////////
///
/// 敵を管理するスクリプト
/// 
/// Aughter:木田晃輔
///
////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    //Unityからアタッチ
    [SerializeField] Vector2 velocity; //質
    [SerializeField] private GameObject explosionPrefab; //爆発エフェクト

    //ゲームマネージャーを使う
    GameGenerator gameGenerator;




    private void Start()
    {
        GetComponent<Rigidbody2D>().velocity = velocity;
        gameGenerator = GameObject.Find("GameGenerator").GetComponent<GameGenerator>();
    }

    /// <summary>
    /// トリガー接触判定
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Black_hole")
        {//ブラックホールに入ったら
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag=="Black"||collision.gameObject.tag == "Blue"||collision.gameObject.tag=="Red"
            ||collision.gameObject.tag=="Green"||collision.gameObject.tag=="Purple"||collision.gameObject.tag=="Yellow")
        {//ジュエルに触れたら

            GameObject explosion = Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.identity);
            explosion.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

            gameGenerator.NinjaDead();

            //忍者とジュエルを消す
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }

    }
}
