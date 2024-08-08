using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BulletGenerator : MonoBehaviour
{
    [SerializeField] List<Bullet> bulletPrefab;     // 弾のプレファブ
    [SerializeField] private Transform firePoint;     // 発射ポイント
    //[SerializeField] private float fireRate = 2.0f;   // 発射間隔(秒)
    [SerializeField] private PlayerController playerController;      // PlayerControllerへの参照

    //フィールド上のアイテム
    List<GameObject> bullets;

    // 削除できるアイテム数
    [SerializeField] int deleteCount;

    //スコア
    int gameScore;

    // UI
    [SerializeField] TextMeshProUGUI textGameScore;
    [SerializeField] TextMeshProUGUI textGameTimer;
    [SerializeField] GameObject panelGameResult;

    //private float timer = 0f;       // タイマー

    private void Start()
    {
        // 全アイテム
        bullets = new List<GameObject>();
    }

    private void Update()
    {

    

        // 削除されたアイテムをクリア
        bullets.RemoveAll(item => item == null);

        
            // 当たり判定があったオブジェクト
            //GameObject obj = hit2d.collider.gameObject;
            //CheckItems(obj);

        // タイマーを更新
        //timer += Time.deltaTime;


        //timer = 0f; // タイマーをリセット

    }

    /// <summary>
    /// 弾の生成
    /// </summary>
    public void FireBullet()
    {
        // 色ランダム
        int rnd = Random.Range(0, bulletPrefab.Count);
        // 弾の生成
    Bullet bullet = Instantiate(bulletPrefab[rnd], firePoint.position, Quaternion.identity);

        if (playerController != null)
        {

            // PlayerControllerから向いている方向を取得
            Vector3 direction = playerController.GetLookDirection();

            // BulletのShootメソッドを呼び出して弾を発射
            bullet.Shoot(direction);
        }
    }

    // 引数と同じ色のアイテムを削除する
    void DeleteItems(List<GameObject> checkItems)
    {

        // 削除可能数に達していなかったら何もしない
        if (checkItems.Count < deleteCount) return;

        // 削除してスコア加算
        List<GameObject> destroyItems = new List<GameObject>();
        foreach (var item in checkItems)
        {
            // かぶりなしの削除したアイテムをカウント
            if (!destroyItems.Contains(item))
            {
                destroyItems.Add(item);
            }

            //削除
            Destroy(item);
        }

        // 実際に削除してスコア加算
        gameScore += destroyItems.Count * 100;

        // スコア表示更新
        textGameScore.text = "" + gameScore;
    }

    //同じ色のアイテムを返す
    List<GameObject> GetSameItems(GameObject target)
    {
        List<GameObject> ret = new List<GameObject>();

        foreach (var item in bullets)
        {
            // アイテムがない、同じアイテム、違う色、処理が遠い場合はスキップ
            if (!item || target == item) continue;

            if (item.GetComponent<SpriteRenderer>().sprite
                != target.GetComponent<SpriteRenderer>().sprite)
            {
                continue;
            }

            float distance
                = Vector2.Distance(target.transform.position, item.transform.position);

            if (distance > 1.1f) continue;

            // ここまで来たらアイテム追加
            ret.Add(item);
        }

        return ret;
    }

    // 引数と同じ色のアイテムを探す
    void CheckItems(GameObject target)
    {
        // このアイテムと同じ色を追加する
        List<GameObject> checkItems = new List<GameObject>();

        //自分を追加
        checkItems.Add(target);

        // チェック済のインデックス
        int checkIndex = 0;

        // checkItemsの最大値までループ
        while (checkIndex < checkItems.Count)
        {
            // 隣接する同じ色を取得
            List<GameObject> sameItems = GetSameItems(checkItems[checkIndex]);
            // チェック済のインデックスを進める
            checkIndex++;

            // まだ追加されてないアイテムを追加する
            foreach (var item in sameItems)
            {
                if (checkItems.Contains(item)) continue;
                checkItems.Add(item);
            }
        }

        // 削除
        DeleteItems(checkItems);
    }
}