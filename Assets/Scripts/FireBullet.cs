using UnityEngine;

public class FireBullet : MonoBehaviour
{
    public float fMoveSpeed = 7.0f;     // 移動値
    //---------------------------
    //          追加
    public GameObject BulletObj;        // 弾のゲームオブジェクト
    //---------------------------
    Vector3 bulletPoint;                // 弾の位置

    void Start()
    {
        bulletPoint = transform.Find("BulletPoint").localPosition;
    }
    // Update is called once per frame
    void Update()
	{
        // ボタンを押したとき
        if (Input.GetButtonDown("Fire1"))
        {
            // 弾の生成
            Instantiate(BulletObj, transform.position + bulletPoint, Quaternion.identity);

        }
    }
}