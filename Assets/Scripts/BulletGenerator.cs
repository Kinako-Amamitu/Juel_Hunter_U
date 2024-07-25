using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGenerator : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;     // 弾のプレファブ
    [SerializeField] private Transform firePoint;     // 発射ポイント
    //[SerializeField] private float fireRate = 2.0f;   // 発射間隔(秒)
    [SerializeField] private PlayerController playerController;      // PlayerControllerへの参照

    //private float timer = 0f;       // タイマー


    private void Update()
    {

        // タイマーを更新
        //timer += Time.deltaTime;


            //timer = 0f; // タイマーをリセット
        
    }

    /// <summary>
    /// 弾の生成
    /// </summary>
    public void FireBullet()
    {

        // 弾の生成
        Bullet bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        if (playerController != null)
        {

            // PlayerControllerから向いている方向を取得
            Vector3 direction = playerController.GetLookDirection();

            // BulletのShootメソッドを呼び出して弾を発射
            bullet.Shoot(direction);
        }
    }
}