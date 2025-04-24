////////////////////////////////////////////////////////////////
///
/// 発射物を管理するスクリプト
/// 
/// Aughter:木田晃輔
///
////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using DG.Tweening;

/// <summary>
/// 弾のプレハブにアタッチする制御用クラス
/// </summary>
public class Bullet : MonoBehaviour
{

    //Unityから取得
    [SerializeField] private float bulletSpeed;　           // 弾の速度
    [SerializeField] private float destroyTime;　           // 弾の生存期間
    [SerializeField] GameObject over;                       //レイ設定用オブジェクト
    [SerializeField] GameObject juel;                       //判定に使う用のジュエル
    [SerializeField] GameGenerator gameGenerator;           //ゲームマネージャー取得
    [SerializeField] private GameObject explosionPrefab;    //爆発エフェクト
    [SerializeField] private GameObject getPrefab;          //取得エフェクト
    [SerializeField] private GameObject deleat;             //発射しっぱいエフェクト


    int layerMask = 1 << 7; //レイヤーマスク

    int juelTime = 0;//ジュエルの待機時間

    
    bool juelMode = true; //ジュエルの状態


    //起動した後1回だけ実行
    private void Start()
    {


        //ゲームマネージャーを探して代入
        gameGenerator = GameObject.Find("GameGenerator").GetComponent<GameGenerator>();

        //オブジェクト操作クラスを取得
        GetComponent<ObjCtrl>();
    }

    //毎フレーム実行する
    private void Update()
    {
        if(gameGenerator.isgameClear==true)
        {//ゲームをクリアしている時
            GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f); //弾の動きを止める
        }
        if(juelMode==false)
        {//ジュエルが使えない状態になった時
            juelTime++; //ジュエル表示時間
            if (juelTime > 100)
            {
                Destroy(gameObject);
            }
        }
        else if(juelMode==true)
        {//ジュエルが機能回復したら
            juelTime = 0;
        }

 
        
    }

    /// <summary>
    /// バレット発射
    /// </summary>
    public void Shoot(Vector2 direction,PlayerController player)
    {

        // 弾に Rigidbody2D コンポーネントがアタッチされているか確認した上で
        if (TryGetComponent(out Rigidbody2D rb))
        {
            //プレイヤーの向いている方向を判定とする
           RaycastHit2D hit=Physics2D.Raycast(player.transform.position, player.GetLookDirection(),10.0f, layerMask);
            if (hit.collider)
            {//レールに向かって発射したら
                //発射物を移動させる
                this.transform.DOMove(hit.point, 1.0f);
            }
            else
            {//それ以外

                //ジュエルを破壊する
                GameObject explosion = Instantiate(deleat, gameObject.transform.position, Quaternion.identity);
                explosion.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                gameGenerator.FaildShoot();
                Destroy(gameObject);
            }
        }
    }

    //爆弾の爆発処理
    public void Explosion(GameObject gameObject)
    {
        //爆発範囲
        var h = Physics2D.CircleCastAll(gameObject.transform.position,1.0f, Vector2.zero);
        foreach (var hit in h)
        {//爆発範囲内のオブジェクト分実行
            if (hit.collider.CompareTag("Black")|| 
                hit.collider.CompareTag("Red") ||
               hit.collider.CompareTag("Blue") ||
               hit.collider.CompareTag("Green") ||
               hit.collider.CompareTag("Purple") ||
               hit.collider.CompareTag("Yellow")||
               hit.collider.CompareTag("Enemy"))
            {//ジュエル、敵の場合
                //破壊して点数獲得
                Destroy(hit.collider.gameObject);
                gameGenerator.AddScore(50);
                gameGenerator.Quest(1);
            }
        }
        //爆発のエフェクトを出して消滅
        GameObject explosion = Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.identity);
        explosion.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        gameGenerator.Explosion();
        Destroy(gameObject);
    }

    //トリガーで接触判定
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Out")
        {//アウトゾーンに入ったら

                //プレイヤー停止
                GameObject.Find("Player").GetComponent<ObjCtrl>().isgameMode = true;
                
                //ゲームオーバー判定
                gameGenerator.GameOver();

        }
        else if (collision.gameObject.tag == "LeftWall")
        {//左の壁(レール内判定)にあたると
            //ジュエルを強制移動
                GetComponent<Rigidbody2D>().velocity = new Vector2(0.1f, 0);
        }
        else if (collision.gameObject.tag == "Black_hole")
        {//ブラックホールにあたると
                Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Enemy_hole")
        {//敵の生成位置にあたると
                Destroy(gameObject);
        }
    }

    //トリガーで接触最中の判定
    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Rail(right)")
        {//右の壁にあたると

            //ジュエルとして機能
            juelMode = true;
            //ジュエルを右に移動
            GetComponent<Rigidbody2D>().velocity = new Vector2(0.2f, 0);
            //両隣のジュエルを判定
            RaycastHit2D hit = Physics2D.Raycast(transform.position+transform.right*0.4f, transform.right, 0.6f);
            ////レイ判定デバッグ用
            //Debug.DrawRay(transform.position + transform.right * 0.4f, transform.right* 0.6f, Color.red, 5);
            

            if (hit.collider)
            {//何かと接触
                if(gameObject.tag=="Black")
                {//ブラックジュエルの場合
                    if (hit.collider.tag != "Black"&&
                        hit.collider.tag != "Rail(right)"&&
                        hit.collider.tag=="Red" ||
                        hit.collider.tag=="Blue" ||
                        hit.collider.tag=="Green" ||
                        hit.collider.tag=="Purple" ||
                        hit.collider.tag=="Yellow")
                    {//レール、ブラックジュエル以外のジュエルの場合

                        //両隣を判定
                        RaycastHit2D hit2 = Physics2D.Raycast(transform.position - transform.right * 0.4f, transform.right * -1, 0.6f);
                        ////レイ判定デバッグ用
                        //Debug.DrawRay(transform.position - transform.right * 0.4f, transform.right * -0.6f, Color.red, 5);

                        if (hit2.collider)
                        {//両隣が何か当たった
                            if (hit2.collider.tag != "Black" &&
                                hit2.collider.tag != "Rail(right)" &&
                                hit2.collider.tag == "Red" ||
                                hit2.collider.tag == "Blue" ||
                                hit2.collider.tag == "Green" ||
                                hit2.collider.tag == "Purple" ||
                                hit2.collider.tag == "Yellow")
                            {//レール、ブラックジュエル以外のジュエルの場合
                                //取得するときのエフェクト
                                GameObject get1 = Instantiate(getPrefab, gameObject.transform.position, Quaternion.identity);
                                GameObject get2 = Instantiate(getPrefab, hit.collider.transform.position, Quaternion.identity);
                                GameObject get3 = Instantiate(getPrefab, hit2.collider.gameObject.transform.position, Quaternion.identity);
                                get1.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                                get2.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                                get3.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

                                //左右のジュエルと自身を消す
                                Destroy(gameObject);
                                Destroy(hit2.collider.gameObject);
                                Destroy(hit.collider.gameObject);

                                //条件達成、得点加算
                                gameGenerator.Quest(3);
                                gameGenerator.AddScore(100);
                            }
                        }
                    }
                }
                if (hit.collider.tag == gameObject.tag)
                {//何かとぶつかった

                    //両隣にレイ判定を飛ばす
                    RaycastHit2D hit2 = Physics2D.Raycast(transform.position - transform.right * 0.4f, transform.right*-1, 0.6f);

                    ////レイ判定デバッグ用
                    //Debug.DrawRay(transform.position - transform.right * 0.4f, transform.right * -0.6f, Color.red, 5);

                    if (hit2.collider)
                    {//両隣が何かと接触
                        if(hit2.collider.tag==gameObject.tag)
                        {//同じ色のジュエルなら
                            //取得エフェクト
                            GameObject get1 = Instantiate(getPrefab, gameObject.transform.position, Quaternion.identity);
                            GameObject get2 = Instantiate(getPrefab, hit.collider.transform.position, Quaternion.identity);
                            GameObject get3 = Instantiate(getPrefab, hit2.collider.gameObject.transform.position, Quaternion.identity);
                            get1.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                            get2.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                            get3.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

                            //左右のジュエルと自身を消す
                            Destroy(gameObject);
                            Destroy(hit2.collider.gameObject);
                            Destroy(hit.collider.gameObject);

                           //条件達成、得点加算
                            gameGenerator.Quest(3);
                            gameGenerator.AddScore(100);
                        }
                    }
                }
                
            }
        }
        else if (collision.gameObject.tag == "Rail(up)")
        {//上昇レールに当たった場合
            //ジュエルとして機能
            juelMode = true;
            //上に移動
            GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.2f);
            //上下に判定を飛ばす
            RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.up * 0.4f, transform.up, 0.6f);
            ////レイ判定デバッグ用
            //Debug.DrawRay(transform.position + transform.up * 0.4f, transform.up * 0.6f, Color.red, 5);

            if (hit.collider)
            {//何かと接触
                if (gameObject.tag == "Black")
                {//ブラックジュエルの場合
                    if (hit.collider.tag != "Black" &&
                        hit.collider.tag != "Rail(up)" &&
                        hit.collider.tag == "Red" ||
                        hit.collider.tag == "Blue" ||
                        hit.collider.tag == "Green" ||
                        hit.collider.tag == "Purple" ||
                        hit.collider.tag == "Yellow")
                    {//上昇レール又はブラックジュエル以外の場合

                        //レイを飛ばして上下判定
                        RaycastHit2D hit2 = Physics2D.Raycast(transform.position - transform.up * 0.4f, transform.up * -1, 0.6f);
                        ////レイ判定デバッグ用
                        //Debug.DrawRay(transform.position - transform.up * 0.4f, transform.up * -0.6f, Color.red, 5);

                        if (hit2.collider)
                        {//両隣が何かと接触
                            if (hit2.collider.tag != "Black" &&
                                hit2.collider.tag != "Rail(up)" &&
                                hit2.collider.tag == "Red" ||
                                hit2.collider.tag == "Blue" ||
                                hit2.collider.tag == "Green" ||
                                hit2.collider.tag == "Purple" ||
                                hit2.collider.tag == "Yellow")
                            {//上昇レール又はブラックジュエル以外の場合
                                //取得エフェクト
                                GameObject get1 = Instantiate(getPrefab, gameObject.transform.position, Quaternion.identity);
                                GameObject get2 = Instantiate(getPrefab, hit.collider.transform.position, Quaternion.identity);
                                GameObject get3 = Instantiate(getPrefab, hit2.collider.gameObject.transform.position, Quaternion.identity);
                                get1.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                                get2.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                                get3.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

                                //左右のジュエルと自身を消す
                                Destroy(gameObject);
                                Destroy(hit2.collider.gameObject);
                                Destroy(hit.collider.gameObject);

                                //条件達成、得点加算
                                gameGenerator.Quest(3);
                                gameGenerator.AddScore(100);
                            }
                        }
                    }
                }
                    if (hit.collider.tag == gameObject.tag)
                    {//何かと接触

                    //上下にレイを飛ばす
                    RaycastHit2D hit2 = Physics2D.Raycast(transform.position - transform.up * 0.4f, transform.up * -1, 0.6f);
                    //レイ判定デバッグ用
                    Debug.DrawRay(transform.position - transform.up * 0.4f, transform.up * -0.6f, Color.red, 5);

                    if (hit2.collider)
                    {//上下が何かと接触
                        if (hit2.collider.tag == gameObject.tag)
                        {//同じ色のジュエルなら
                            //取得エフェクト
                            GameObject get1 = Instantiate(getPrefab, gameObject.transform.position, Quaternion.identity);
                            GameObject get2 = Instantiate(getPrefab, hit.collider.transform.position, Quaternion.identity);
                            GameObject get3 = Instantiate(getPrefab, hit2.collider.gameObject.transform.position, Quaternion.identity);
                            get1.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                            get2.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                            get3.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

                            //左右のジュエルと自身を消す
                            Destroy(gameObject);
                            Destroy(hit2.collider.gameObject);
                            Destroy(hit.collider.gameObject);

                            //条件達成、得点加算
                            gameGenerator.Quest(3);
                            gameGenerator.AddScore(100);
                        }
                    }
                }

            }
        }
        else if (collision.gameObject.tag == "Rail(down)")
        {//下降レールに当たった
            //ジュエルとして機能
            juelMode = true;
            //下に移動
            GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, -0.2f);
            //上下にレイ判定を飛ばす
            RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.up * 0.4f, transform.up, 0.6f);
            ////レイ判定デバッグ用
            //Debug.DrawRay(transform.position + transform.up * 0.4f, transform.up * 0.6f, Color.red, 5);

            if (hit.collider)
            {//何かと接触
                if (gameObject.tag == "Black")
                {//ブラックジュエルの場合
                    if (hit.collider.tag != "Black" &&
                        hit.collider.tag != "Rail(down)" &&
                        hit.collider.tag == "Red" ||
                        hit.collider.tag == "Blue" ||
                        hit.collider.tag == "Green" ||
                        hit.collider.tag == "Purple" ||
                        hit.collider.tag == "Yellow")
                    {//レールとブラックジュエル以外の場合

                        //レイ判定を上下に飛ばす
                        RaycastHit2D hit2 = Physics2D.Raycast(transform.position - transform.up * 0.4f, transform.up * -1, 0.6f);
                        ////レイ判定デバッグ用
                        //Debug.DrawRay(transform.position - transform.up * 0.4f, transform.up * -0.6f, Color.red, 5);

                        if (hit2.collider)
                        {//上下に何かと接触
                            if (hit2.collider.tag != "Black" &&
                                hit2.collider.tag != "Rail(down)" &&
                                hit2.collider.tag == "Red" ||
                                hit2.collider.tag == "Blue" ||
                                hit2.collider.tag == "Green" ||
                                hit2.collider.tag == "Purple" ||
                                hit2.collider.tag == "Yellow")
                            {//レールでもブラックジュエルでもない場合
                                //取得エフェクト
                                GameObject get1 = Instantiate(getPrefab, gameObject.transform.position, Quaternion.identity);
                                GameObject get2 = Instantiate(getPrefab, hit.collider.transform.position, Quaternion.identity);
                                GameObject get3 = Instantiate(getPrefab, hit2.collider.gameObject.transform.position, Quaternion.identity);
                                get1.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                                get2.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                                get3.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

                                //左右のジュエルと自身を消す
                                Destroy(gameObject);
                                Destroy(hit2.collider.gameObject);
                                Destroy(hit.collider.gameObject);

                                //条件達成、得点加算
                                gameGenerator.Quest(3);
                                gameGenerator.AddScore(100);
                            }
                        }
                    }
                }
                if (hit.collider.tag == gameObject.tag)
                {//何かと接触
                    //上下にレイを飛ばす
                    RaycastHit2D hit2 = Physics2D.Raycast(transform.position - transform.up * 0.4f, transform.up * -1, 0.6f);
                    ////レイ判定デバッグ用
                    //Debug.DrawRay(transform.position - transform.up * 0.4f, transform.up * -0.6f, Color.red, 5);

                    if (hit2.collider)
                    {//上下に何かと接触
                        if (hit2.collider.tag == gameObject.tag)
                        {//同じ色のジュエルの場合
                            //取得エフェクト
                            GameObject get1 = Instantiate(getPrefab, gameObject.transform.position, Quaternion.identity);
                            GameObject get2 = Instantiate(getPrefab, hit.collider.transform.position, Quaternion.identity);
                            GameObject get3 = Instantiate(getPrefab, hit2.collider.gameObject.transform.position, Quaternion.identity);
                            get1.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                            get2.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                            get3.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

                            //左右のジュエルと自身を消す
                            Destroy(gameObject);
                            Destroy(hit2.collider.gameObject);
                            Destroy(hit.collider.gameObject);

                           //条件達成、得点加算
                            gameGenerator.Quest(3);
                            gameGenerator.AddScore(100);
                        }
                    }
                }

            }
        }
        else if (collision.gameObject.tag == "Rail(down)(slow)")
        {//遅い下降レールに当たった
            //ジュエルとして機能
            juelMode = true;
            //右にゆっくり移動
            GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, -0.1f);
            //左右にレイを飛ばす
            RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.up * 0.4f, transform.up, 0.6f);
            ////レイ判定デバッグ用
            //Debug.DrawRay(transform.position + transform.up * 0.4f, transform.up * 0.6f, Color.red, 5);

            if (hit.collider)
            {//何かと接触
                if (gameObject.tag == "Black")
                {//ブラックジュエルの場合
                    if (hit.collider.tag != "Black" &&
                        hit.collider.tag != "Rail(down)(slow)" &&
                        hit.collider.tag == "Red" ||
                        hit.collider.tag == "Blue" ||
                        hit.collider.tag == "Green" ||
                        hit.collider.tag == "Purple" ||
                        hit.collider.tag == "Yellow")
                    {//レール、ブラックジュエル以外
                        //左右にレイを飛ばす
                        RaycastHit2D hit2 = Physics2D.Raycast(transform.position - transform.up * 0.4f, transform.up * -1, 0.6f);
                        ////レイ判定デバッグ用
                        //Debug.DrawRay(transform.position - transform.up * 0.4f, transform.up * -0.6f, Color.red, 5);

                        if (hit2.collider)
                        {//左右同時に接触
                            if (hit2.collider.tag != "Black" &&
                                hit2.collider.tag != "Rail(down)(slow)" &&
                                hit2.collider.tag == "Red" ||
                                hit2.collider.tag == "Blue" ||
                                hit2.collider.tag == "Green" ||
                                hit2.collider.tag == "Purple" ||
                                hit2.collider.tag == "Yellow")
                            {//レール、ブラックジュエル以外
                                //取得エフェクト
                                GameObject get1 = Instantiate(getPrefab, gameObject.transform.position, Quaternion.identity);
                                GameObject get2 = Instantiate(getPrefab, hit.collider.transform.position, Quaternion.identity);
                                GameObject get3 = Instantiate(getPrefab, hit2.collider.gameObject.transform.position, Quaternion.identity);
                                get1.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                                get2.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                                get3.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

                                //左右のジュエルと自身を消す
                                Destroy(gameObject);
                                Destroy(hit2.collider.gameObject);
                                Destroy(hit.collider.gameObject);

                                //条件達成、得点加算
                                gameGenerator.Quest(3);
                                gameGenerator.AddScore(100);
                            }
                        }
                    }
                }
                if (hit.collider.tag == gameObject.tag)
                {//何かと接触
                    //左右にレイを飛ばす
                    RaycastHit2D hit2 = Physics2D.Raycast(transform.position - transform.up * 0.4f, transform.up * -1, 0.6f);
                    //レイ判定デバッグ用
                    Debug.DrawRay(transform.position - transform.up * 0.4f, transform.up * -0.6f, Color.red, 5);

                    if (hit2.collider)
                    {//何かと接触
                        if (hit2.collider.tag == gameObject.tag)
                        {
                            //取得エフェクト
                            GameObject get1 = Instantiate(getPrefab, gameObject.transform.position, Quaternion.identity);
                            GameObject get2 = Instantiate(getPrefab, hit.collider.transform.position, Quaternion.identity);
                            GameObject get3 = Instantiate(getPrefab, hit2.collider.gameObject.transform.position, Quaternion.identity);
                            get1.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                            get2.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                            get3.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

                            //左右のジュエルと自身を消す
                            Destroy(gameObject);
                            Destroy(hit2.collider.gameObject);
                            Destroy(hit.collider.gameObject);

                          
                            gameGenerator.Quest(3);
                            gameGenerator.AddScore(100);
                        }
                    }
                }

            }
        }
        else if (collision.gameObject.tag == "Rail(left)")
        {//左のレールに当たる

            //ジュエルを展開
            juelMode = true;

            //左に移動
            GetComponent<Rigidbody2D>().velocity = new Vector2(-0.2f, 0);

            //両隣にレイ判定を飛ばす
            RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.right * 0.4f, transform.right, 0.6f);

            ////レイ判定デバッグ用
            //Debug.DrawRay(transform.position + transform.right * 0.4f, transform.right * 0.6f, Color.red, 5);


            if (hit.collider)
            {//何かと接触
                if (gameObject.tag == "Black")
                {//ブラックジュエルだった場合
                    if (hit.collider.tag != "Black" &&
                        hit.collider.tag != "Rail(left)" &&
                        hit.collider.tag == "Red" ||
                        hit.collider.tag == "Blue" ||
                        hit.collider.tag == "Green" ||
                        hit.collider.tag == "Purple" ||
                        hit.collider.tag == "Yellow")
                    {//ブラックジュエルとレール以外なら

                        //レイを両隣に飛ばす
                        RaycastHit2D hit2 = Physics2D.Raycast(transform.position - transform.right * 0.4f, transform.right * -1, 0.6f);

                        //レイ判定デバッグ用
                        Debug.DrawRay(transform.position - transform.right * 0.4f, transform.right * -0.6f, Color.red, 5);

                        if (hit2.collider)
                        {//何かと接触
                            if (hit2.collider.tag != "Black" &&
                                hit2.collider.tag != "Rail(left)" &&
                                hit2.collider.tag == "Red" ||
                                hit2.collider.tag == "Blue" ||
                                hit2.collider.tag == "Green" ||
                                hit2.collider.tag == "Purple" ||
                                hit2.collider.tag == "Yellow")
                            {//ブラックジュエルとレール以外なら
                                //取得エフェクト
                                GameObject get1 = Instantiate(getPrefab, gameObject.transform.position, Quaternion.identity);
                                GameObject get2 = Instantiate(getPrefab, hit.collider.transform.position, Quaternion.identity);
                                GameObject get3 = Instantiate(getPrefab, hit2.collider.gameObject.transform.position, Quaternion.identity);
                                get1.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                                get2.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                                get3.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

                                //左右のジュエルと自身を消す
                                Destroy(gameObject);
                                Destroy(hit2.collider.gameObject);
                                Destroy(hit.collider.gameObject);

                                //条件を達成、得点加算
                                gameGenerator.Quest(3);
                                gameGenerator.AddScore(100);
                            }
                        }
                    }
                }
                if (hit.collider.tag == gameObject.tag)
                {//ジュエルと接触

                    //両隣にレイ判定
                    RaycastHit2D hit2 = Physics2D.Raycast(transform.position - transform.right * 0.4f, transform.right * -1, 0.6f);

                    ////レイ判定デバッグ用
                    //Debug.DrawRay(transform.position - transform.right * 0.4f, transform.right * -0.6f, Color.red, 5);

                    if (hit2.collider)
                    {//何かと接触
                        if (hit2.collider.tag == gameObject.tag)
                        {//同じ色のジュエルだったら
                            //取得エフェクト
                            GameObject get1 = Instantiate(getPrefab, gameObject.transform.position, Quaternion.identity);
                            GameObject get2 = Instantiate(getPrefab, hit.collider.transform.position, Quaternion.identity);
                            GameObject get3 = Instantiate(getPrefab, hit2.collider.gameObject.transform.position, Quaternion.identity);
                            get1.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                            get2.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                            get3.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

                            //左右のジュエルと自身を消す
                            Destroy(gameObject);
                            Destroy(hit2.collider.gameObject);
                            Destroy(hit.collider.gameObject);

                            //条件達成、得点加算
                            gameGenerator.Quest(3);
                            gameGenerator.AddScore(100);
                        }
                    }
                }

            }
        }
        else if(collision.gameObject.tag == "Bullet_Point")
        {//発射地点にいる時はジュエルの状態を展開させる
            juelMode = true;
        }



    }

    /// <summary>
    /// コライダー判定から外れた場合
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Bullet_Point"||
            collision.gameObject.tag == "Rail(right)" ||
            collision.gameObject.tag == "Rail(left)" ||
            collision.gameObject.tag == "Rail(down)" ||
            collision.gameObject.tag == "Rail(down)(slow)" ||
            collision.gameObject.tag == "Rail(up)")
        {//どのレール、発射地点2もいない場合
            //ジュエルの機能off
            juelMode = false;
        }
    }
}

