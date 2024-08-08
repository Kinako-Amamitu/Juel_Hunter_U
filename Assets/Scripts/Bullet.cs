using UnityEngine;

/// <summary>
/// 弾のプレハブにアタッチする制御用クラス
/// </summary>
public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;　// 弾の速度
    [SerializeField] private float destroyTime;　// 弾の生存期間


    /// <summary>
    /// バレット発射
    /// </summary>
    public void Shoot(Vector2 direction)
    {

        // 弾に Rigidbody2D コンポーネントがアタッチされているか確認した上で
        if (TryGetComponent(out Rigidbody2D rb))
        {

            // Rigidbody2D の AddForce メソッドを利用して、プレイヤーの進行方向と同じ方向に弾を発射する
            rb.AddForce(direction * bulletSpeed);
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
    }
}

