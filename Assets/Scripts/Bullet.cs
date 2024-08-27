using TMPro;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 弾のプレハブにアタッチする制御用クラス
/// </summary>
public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;　// 弾の速度
    [SerializeField] private float destroyTime;　// 弾の生存期間
    [SerializeField] TextMeshProUGUI gameoverText; //ゲームオーバーのテキスト
    [SerializeField] GameObject over; //レイ設定用オブジェクト
    PlayerController player; //プレイヤー

    int layerMask = 1 << 7;
    




    private void Start()
    {
       
        gameoverText=GameObject.Find("gameoverText").GetComponent<TextMeshProUGUI>();

    }

    private void Update()
    {
        
    }
    /// <summary>
    /// バレット発射
    /// </summary>
    public void Shoot(Vector2 direction)
    {

        // 弾に Rigidbody2D コンポーネントがアタッチされているか確認した上で
        if (TryGetComponent(out Rigidbody2D rb))
        {

            player = GameObject.Find("Player").GetComponent<PlayerController>();

            Ray2D ray = new Ray2D(transform.position,transform.up);

           RaycastHit2D hit=Physics2D.Raycast(transform.position, player.GetLookDirection(),10.0f,layerMask);
            if (hit.collider)
            {
                Debug.Log(hit.point);
               
                this.transform.DOMove(hit.point, 1.0f);
            }
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, 5);
        }

        // 一定時間後に弾を破壊する
        //Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Rail")
        {
            GetComponent<Rigidbody2D>().velocity=new Vector2(0.1f,0);
        }
        else if(collision.gameObject.tag=="Out")
        {
            gameoverText.SetText("GameOver!!");
        }
        else if(collision.gameObject.tag=="LeftWall")
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0.1f, 0);
        }
        else if(collision.gameObject.tag=="juel")
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0.1f, 0);
        }
    }
}

