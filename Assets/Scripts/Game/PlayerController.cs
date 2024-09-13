using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] GameGenerator gameGenerator;
    [SerializeField] string playerTag;
    [SerializeField] int playerNum;


    


    private void Start()
    {


        

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
            if (hit2d && hit2d.transform.gameObject.tag == playerTag)
            {
                
                gameGenerator.FireBullet(playerNum);



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

}
