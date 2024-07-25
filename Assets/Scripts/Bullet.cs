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

    /// <summary>
    /// 衝突判定
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

