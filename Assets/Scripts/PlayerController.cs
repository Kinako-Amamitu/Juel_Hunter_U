using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb;     // コンポーネントの取得用
    private Animator anim;      // コンポーネントの取得用

    private Vector3 lookDirection = new Vector3(0, -1.0f,0);      // キャラの向きの情報の設定用


    private void Start()
    {

        TryGetComponent(out rb);
        TryGetComponent(out anim);
    }

    private void Update()
    {
        if (!anim)
        {
            return;
        }

        // キャラの向いている方向と移動アニメの同期
        SyncMoveAnimation();
    }


    /// <summary>
    /// キャラの向いている方向と移動アニメの同期
    /// </summary>
    private void SyncMoveAnimation()
    {
            // 正規化
            lookDirection.Normalize();

            // キー入力の値とBlendTreeでせっていした移動アニメ用の値を確認し、移動アニメを再生
            anim.SetFloat("Look X", lookDirection.x);
            anim.SetFloat("Look Y", lookDirection.y);

            // キー入力の値とBlendTreeで設定した移動アニメ用の値を確認し、移動アニメを再生
            anim.SetFloat("Speed", lookDirection.sqrMagnitude);
    }

    /// <summary>
    /// プレイヤーの進行方向の取得用
    /// </summary>
    /// <returns></returns>
    public Vector3 GetLookDirection()
    {

        return transform.forward;
    }
}
