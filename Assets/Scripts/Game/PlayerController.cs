using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] GameGenerator gameGenerator;

    private Rigidbody2D rb;     // コンポーネントの取得用
  

    private Vector3 lookDirection = new Vector3(0, -1.0f,0);      // キャラの向きの情報の設定用


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
            if (hit2d && hit2d.transform.gameObject.tag=="Player")
            {
                gameGenerator.FireBullet();


            }

        }
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
}
