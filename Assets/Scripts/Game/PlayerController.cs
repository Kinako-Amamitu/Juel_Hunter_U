using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] GameGenerator gameGenerator;

    private Rigidbody2D rb;     // コンポーネントの取得用
  

    private Vector3 lookDirection = new Vector3(0, -1.0f,0);      // キャラの向きの情報の設定用

    [SerializeField] List<Bullet> juelPrefabs; //判定に使う用のジュエルプレハブ

    int juelRnd; //ジュエルのランダムなID

    Bullet nextBullet; //次のバレット


    private void Start()
    {

        TryGetComponent(out rb);

    }

    private void Update()
    {
        /* マウスクリックを検知 */
        if (Input.GetMouseButtonDown(0))
        {

            // タップした場所からRayを作成
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Raycastを作成
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

            //Rayが何かに衝突したことを検知 & 衝突した対象が自分自身かを判別
            if (hit2d && hit2d.transform.gameObject.tag == "Player")
            {
                gameGenerator.FireBullet();



            }

        }
    }

    private void FixedUpdate()
    {
        
    }
    /// <summary>
    /// プレイヤーの進行方向の取得用
    /// </summary>
    /// <returns></returns>
    public Vector3 GetLookDirection()
    {
        Vector3 direction = new Vector3(-Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad), Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad));
        return direction;
    }

    public void SiteJuel(int rnd)
    {
        // 表示位置
        Vector3 setupPosition = new Vector3(-2.5f, 0, 0);

        // ブロック生成
        Bullet prefab = juelPrefabs[rnd];
        Instantiate(prefab, setupPosition, Quaternion.identity);

    }
}
