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
    [SerializeField] private float bulletSpeed;　// 弾の速度
    [SerializeField] private float destroyTime;　// 弾の生存期間
    [SerializeField] GameObject over; //レイ設定用オブジェクト
    [SerializeField] GameObject juel; //判定に使う用のジュエル
    [SerializeField] GameGenerator gameGenerator;
    [SerializeField] private GameObject explosionPrefab; //爆発エフェクト
    [SerializeField] private GameObject getPrefab; //取得エフェクト
    [SerializeField] private GameObject deleat; //発射しっぱいエフェクト
    //PlayerController playerController;



    int layerMask = 1 << 7;

    int juelTime = 0;//ジュエルの待機時間

    bool juelMode = true; //ジュエルの状態

    private void Start()
    {



        gameGenerator = GameObject.Find("GameGenerator").GetComponent<GameGenerator>();
        GetComponent<ObjCtrl>();
    }

    private void Update()
    {
        if(juelMode==false)
        {
            juelTime++;
            if (juelTime > 100)
            {
                Destroy(gameObject);
            }
        }
        else if(juelMode==true)
        {
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

           RaycastHit2D hit=Physics2D.Raycast(player.transform.position, player.GetLookDirection(),10.0f, layerMask);
            if (hit.collider)
            {
                Debug.Log(hit.point);
               
                this.transform.DOMove(hit.point, 1.0f);
            }
            else
            {
                GameObject explosion = Instantiate(deleat, gameObject.transform.position, Quaternion.identity);
                explosion.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                gameGenerator.FaildShoot();
                Destroy(gameObject);
            }
        }

        // 一定時間後に弾を破壊する
        //Destroy(gameObject, destroyTime);
    }

    public void Explosion(GameObject gameObject)
    {

        var h = Physics2D.CircleCastAll(gameObject.transform.position,1.0f, Vector2.zero);
        Debug.Log(h.Length);
        foreach (var hit in h)
        {
            if (hit.collider.CompareTag("Black")|| 
                hit.collider.CompareTag("Red") ||
               hit.collider.CompareTag("Blue") ||
               hit.collider.CompareTag("Green") ||
               hit.collider.CompareTag("Purple") ||
               hit.collider.CompareTag("Yellow")||
               hit.collider.CompareTag("Enemy"))
            {
                Destroy(hit.collider.gameObject);
                gameGenerator.AddScore(50);
                gameGenerator.Quest(1);
            }
        }
        GameObject explosion = Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.identity);
        explosion.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        gameGenerator.Explosion();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Out")
            {
                GameObject.Find("Player").GetComponent<ObjCtrl>().isgameMode = true;

                gameGenerator.GameOver();

            }
            else if (collision.gameObject.tag == "LeftWall")
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(0.1f, 0);
            }
            else if (collision.gameObject.tag == "Black_hole")
            {
                Destroy(gameObject);
            }
            else if (collision.gameObject.tag == "Enemy_hole")
            {
                Destroy(gameObject);
            }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Rail(right)")
        {
            juelMode = true;
            bool isSameright=false;

            GetComponent<Rigidbody2D>().velocity = new Vector2(0.2f, 0);

            RaycastHit2D hit = Physics2D.Raycast(transform.position+transform.right*0.4f, transform.right, 0.6f);

            Debug.DrawRay(transform.position + transform.right * 0.4f, transform.right* 0.6f, Color.red, 5);
            

            if (hit.collider)
            {
                if(gameObject.tag=="Black")
                {
                    if (hit.collider.tag != "Black"&&
                        hit.collider.tag != "Rail(right)"&&
                        hit.collider.tag=="Red" ||
                        hit.collider.tag=="Blue" ||
                        hit.collider.tag=="Green" ||
                        hit.collider.tag=="Purple" ||
                        hit.collider.tag=="Yellow")
                    {
                        isSameright = true;

                        RaycastHit2D hit2 = Physics2D.Raycast(transform.position - transform.right * 0.4f, transform.right * -1, 0.6f);

                        Debug.DrawRay(transform.position - transform.right * 0.4f, transform.right * -0.6f, Color.red, 5);

                        if (hit2.collider)
                        {
                            if (hit2.collider.tag != "Black" &&
                                hit2.collider.tag != "Rail(right)" &&
                                hit2.collider.tag == "Red" ||
                                hit2.collider.tag == "Blue" ||
                                hit2.collider.tag == "Green" ||
                                hit2.collider.tag == "Purple" ||
                                hit2.collider.tag == "Yellow")
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
                if (hit.collider.tag == gameObject.tag)
                {
                    isSameright = true;

                    RaycastHit2D hit2 = Physics2D.Raycast(transform.position - transform.right * 0.4f, transform.right*-1, 0.6f);

                    Debug.DrawRay(transform.position - transform.right * 0.4f, transform.right * -0.6f, Color.red, 5);

                    if (hit2.collider)
                    {
                        if(hit2.collider.tag==gameObject.tag)
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
        else if (collision.gameObject.tag == "Rail(up)")
        {
            juelMode = true;
            bool isSameright = false;

            GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.2f);

            RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.up * 0.4f, transform.up, 0.6f);

            Debug.DrawRay(transform.position + transform.up * 0.4f, transform.up * 0.6f, Color.red, 5);

            if (hit.collider)
            {
                if (gameObject.tag == "Black")
                {
                    if (hit.collider.tag != "Black" &&
                        hit.collider.tag != "Rail(up)" &&
                        hit.collider.tag == "Red" ||
                        hit.collider.tag == "Blue" ||
                        hit.collider.tag == "Green" ||
                        hit.collider.tag == "Purple" ||
                        hit.collider.tag == "Yellow")
                    {
                        isSameright = true;

                        RaycastHit2D hit2 = Physics2D.Raycast(transform.position - transform.up * 0.4f, transform.up * -1, 0.6f);

                        Debug.DrawRay(transform.position - transform.up * 0.4f, transform.up * -0.6f, Color.red, 5);

                        if (hit2.collider)
                        {
                            if (hit2.collider.tag != "Black" &&
                                hit2.collider.tag != "Rail(up)" &&
                                hit2.collider.tag == "Red" ||
                                hit2.collider.tag == "Blue" ||
                                hit2.collider.tag == "Green" ||
                                hit2.collider.tag == "Purple" ||
                                hit2.collider.tag == "Yellow")
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
                    if (hit.collider.tag == gameObject.tag)
                {
                    isSameright = true;

                    RaycastHit2D hit2 = Physics2D.Raycast(transform.position - transform.up * 0.4f, transform.up * -1, 0.6f);

                    Debug.DrawRay(transform.position - transform.up * 0.4f, transform.up * -0.6f, Color.red, 5);

                    if (hit2.collider)
                    {
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
        else if (collision.gameObject.tag == "Rail(down)")
        {
            juelMode = true;
            bool isSameright = false;

            GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, -0.2f);

            RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.up * 0.4f, transform.up, 0.6f);

            Debug.DrawRay(transform.position + transform.up * 0.4f, transform.up * 0.6f, Color.red, 5);

            if (hit.collider)
            {
                if (gameObject.tag == "Black")
                {
                    if (hit.collider.tag != "Black" &&
                        hit.collider.tag != "Rail(down)" &&
                        hit.collider.tag == "Red" ||
                        hit.collider.tag == "Blue" ||
                        hit.collider.tag == "Green" ||
                        hit.collider.tag == "Purple" ||
                        hit.collider.tag == "Yellow")
                    {
                        isSameright = true;

                        RaycastHit2D hit2 = Physics2D.Raycast(transform.position - transform.up * 0.4f, transform.up * -1, 0.6f);

                        Debug.DrawRay(transform.position - transform.up * 0.4f, transform.up * -0.6f, Color.red, 5);

                        if (hit2.collider)
                        {
                            if (hit2.collider.tag != "Black" &&
                                hit2.collider.tag != "Rail(down)" &&
                                hit2.collider.tag == "Red" ||
                                hit2.collider.tag == "Blue" ||
                                hit2.collider.tag == "Green" ||
                                hit2.collider.tag == "Purple" ||
                                hit2.collider.tag == "Yellow")
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
                if (hit.collider.tag == gameObject.tag)
                {
                    isSameright = true;

                    RaycastHit2D hit2 = Physics2D.Raycast(transform.position - transform.up * 0.4f, transform.up * -1, 0.6f);

                    Debug.DrawRay(transform.position - transform.up * 0.4f, transform.up * -0.6f, Color.red, 5);

                    if (hit2.collider)
                    {
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
        else if (collision.gameObject.tag == "Rail(down)(slow)")
        {
            juelMode = true;
            bool isSameright = false;

            GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, -0.1f);

            RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.up * 0.4f, transform.up, 0.6f);

            Debug.DrawRay(transform.position + transform.up * 0.4f, transform.up * 0.6f, Color.red, 5);

            if (hit.collider)
            {
                if (gameObject.tag == "Black")
                {
                    if (hit.collider.tag != "Black" &&
                        hit.collider.tag != "Rail(down)(slow)" &&
                        hit.collider.tag == "Red" ||
                        hit.collider.tag == "Blue" ||
                        hit.collider.tag == "Green" ||
                        hit.collider.tag == "Purple" ||
                        hit.collider.tag == "Yellow")
                    {
                        isSameright = true;

                        RaycastHit2D hit2 = Physics2D.Raycast(transform.position - transform.up * 0.4f, transform.up * -1, 0.6f);

                        Debug.DrawRay(transform.position - transform.up * 0.4f, transform.up * -0.6f, Color.red, 5);

                        if (hit2.collider)
                        {
                            if (hit2.collider.tag != "Black" &&
                                hit2.collider.tag != "Rail(down)(slow)" &&
                                hit2.collider.tag == "Red" ||
                                hit2.collider.tag == "Blue" ||
                                hit2.collider.tag == "Green" ||
                                hit2.collider.tag == "Purple" ||
                                hit2.collider.tag == "Yellow")
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
                if (hit.collider.tag == gameObject.tag)
                {
                    isSameright = true;

                    RaycastHit2D hit2 = Physics2D.Raycast(transform.position - transform.up * 0.4f, transform.up * -1, 0.6f);

                    Debug.DrawRay(transform.position - transform.up * 0.4f, transform.up * -0.6f, Color.red, 5);

                    if (hit2.collider)
                    {
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
        {
            juelMode = true;
            bool isSameright = false;

            GetComponent<Rigidbody2D>().velocity = new Vector2(-0.2f, 0);

            RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.right * 0.4f, transform.right, 0.6f);

            Debug.DrawRay(transform.position + transform.right * 0.4f, transform.right * 0.6f, Color.red, 5);


            if (hit.collider)
            {
                if (gameObject.tag == "Black")
                {
                    if (hit.collider.tag != "Black" &&
                        hit.collider.tag != "Rail(left)" &&
                        hit.collider.tag == "Red" ||
                        hit.collider.tag == "Blue" ||
                        hit.collider.tag == "Green" ||
                        hit.collider.tag == "Purple" ||
                        hit.collider.tag == "Yellow")
                    {
                        isSameright = true;

                        RaycastHit2D hit2 = Physics2D.Raycast(transform.position - transform.right * 0.4f, transform.right * -1, 0.6f);

                        Debug.DrawRay(transform.position - transform.right * 0.4f, transform.right * -0.6f, Color.red, 5);

                        if (hit2.collider)
                        {
                            if (hit2.collider.tag != "Black" &&
                                hit2.collider.tag != "Rail(left)" &&
                                hit2.collider.tag == "Red" ||
                                hit2.collider.tag == "Blue" ||
                                hit2.collider.tag == "Green" ||
                                hit2.collider.tag == "Purple" ||
                                hit2.collider.tag == "Yellow")
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
                if (hit.collider.tag == gameObject.tag)
                {
                    isSameright = true;

                    RaycastHit2D hit2 = Physics2D.Raycast(transform.position - transform.right * 0.4f, transform.right * -1, 0.6f);

                    Debug.DrawRay(transform.position - transform.right * 0.4f, transform.right * -0.6f, Color.red, 5);

                    if (hit2.collider)
                    {
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
        else if(collision.gameObject.tag == "Bullet_Point")
        {
            juelMode = true;
        }



    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Bullet_Point"||
            collision.gameObject.tag == "Rail(right)" ||
            collision.gameObject.tag == "Rail(left)" ||
            collision.gameObject.tag == "Rail(down)" ||
            collision.gameObject.tag == "Rail(down)(slow)" ||
            collision.gameObject.tag == "Rail(up)")
        {
            juelMode = false;
        }
    }
}

