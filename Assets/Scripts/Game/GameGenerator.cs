using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GameGenerator : MonoBehaviour
{
    [SerializeField] List<Bullet> bulletPrefab;     // 弾のプレファブ
    [SerializeField] private Transform firePoint;     // 発射ポイント
    //[SerializeField] private float fireRate = 2.0f;   // 発射間隔(秒)
    [SerializeField] private PlayerController playerController;      // PlayerControllerへの参照
    [SerializeField] List<GameObject> juelPrefabs; //判定に使う用のジュエルプレハブ
    [SerializeField] private Transform outZone; //ゲームオーバー判定位置
    [SerializeField] float gameTimer; //ゲーム時間

    //フィールド上のアイテム
    List<GameObject> bullets;

    // 削除できるアイテム数
    [SerializeField] int deleteCount;

    


    //スコア
    int gameScore;

    //条件個数
    int juelRequired = 9;



    // UI
    [SerializeField] TextMeshProUGUI textGameScore;
    [SerializeField] TextMeshProUGUI textGameTimer;
    [SerializeField] TextMeshProUGUI stageclearText;
    [SerializeField] TextMeshProUGUI gameoverText;
    [SerializeField] Text target1;
    [SerializeField] GameObject posemenuPanel;
    

    //private float timer = 0f;       // タイマー

    private void Start()
    {
      
        
        // 全アイテム
        bullets = new List<GameObject>();
    }

    private void Update()
    {

        // ゲームタイマー更新
        gameTimer -= Time.deltaTime;
        textGameTimer.text = "" + (int)gameTimer;

        if(juelRequired<=0)
        {
            target1.text ="OK！！" ;
            stageclearText.SetText("StageClear!!");

           

            return;
        }

        //ゲーム終了
        if (0 >= gameTimer)
        {

            gameoverText.SetText("GameOver!!");
            // Updateに入らないようにする
            enabled = false;
          
;            // この時点でUpdateから抜ける
            return;
        }
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

    //点数を加算させる
    public void AddScore(int score)
    {
        gameScore += score;
        textGameScore.text = gameScore.ToString();
    }

    //クリア条件を達成させる
    public void Quest(int target)
    {
        juelRequired -= target;
        target1.text = juelRequired.ToString();
    }

    /// <summary>
    /// ポーズメニュー関連
    /// </summary>
    public void Pose()
    {
        Time.timeScale = 0;
        posemenuPanel.SetActive(true);
    }

    //リスタートする
    public void Restart()
    {
        posemenuPanel.SetActive(false);
        Time.timeScale = 1;
    }

    //最初からやり直す
    public void Reset()
    {
        Time.timeScale = 1;
        //画面遷移
        Initiate.DoneFading();
        Initiate.Fade("Stage1", Color.black, 0.5f);
    
    }

    //ステージセレクトに戻る
    public void StageSelect()
    {
        Time.timeScale = 1;
        //画面遷移
        Initiate.DoneFading();
        Initiate.Fade("StageSelect", Color.black, 0.5f);
    }

}