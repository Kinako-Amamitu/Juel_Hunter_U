using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameGenerator : MonoBehaviour
{
    [SerializeField] List<Bullet> bulletPrefab;     // 弾のプレファブ
    [SerializeField] private Transform firePoint;     // 発射ポイント
    //[SerializeField] private float fireRate = 2.0f;   // 発射間隔(秒)
    [SerializeField] private PlayerController playerController;      // PlayerControllerへの参照
    [SerializeField] List<GameObject> juelPrefabs; //判定に使う用のジュエルプレハブ
    [SerializeField] private Transform outZone; //ゲームオーバー判定位置
    [SerializeField] float gameTimer; //ゲーム時間
    [SerializeField]int juelRequired; //条件個数

    //フィールド上のアイテム
    List<GameObject> bullets;

    // 削除できるアイテム数
    [SerializeField] int deleteCount;


    ObjCtrl obj;

    //スコア
    int gameScore;

    //シーンの判定変数
    static int currentStage;

    

    //ゲームの判定
    public bool isgameOver=false;
    public bool isgameClear = false;

    Bullet bullet;


    // UI
    [SerializeField] TextMeshProUGUI textGameScore;
    [SerializeField] TextMeshProUGUI textGameTimer;
    [SerializeField] TextMeshProUGUI stageclearText;
    [SerializeField] TextMeshProUGUI gameoverText;
    [SerializeField] Text target1;
    [SerializeField] GameObject posemenuPanel;
    [SerializeField] GameObject gameoverPanel;
    

    //private float timer = 0f;       // タイマー

    private void Start()
    {
        //オブジェクトクラスを取得
        obj = GetComponent<ObjCtrl>();

        //クリア条件初期化
        target1.text = juelRequired.ToString();

        // 全アイテム
        bullets = new List<GameObject>();

        //初弾のジュエルを抽選
        StartCoroutine(UpdateBullet());
    }

    private void Update()
    {
        if (isgameOver == true)
        {
            return;
        }
        else if (isgameClear == true)
        {
            return;
        }
        // ゲームタイマー更新
        gameTimer -= Time.deltaTime;
        textGameTimer.text = "" + (int)gameTimer;

        if(juelRequired<=0)
        {
            target1.text ="OK！！" ;

            GameClear();
        }

        //ゲーム終了
        if (0 >= gameTimer)
        {
            
         

            GameOver();

            // Updateに入らないようにする
            enabled = false;
          
;            // この時点でUpdateから抜ける
            return;
        }

        if(bullet!=null)
        {
            bullet.transform.position = new Vector3(firePoint.transform.position.x, firePoint.transform.position.y, bullet.transform.position.z);
        }
    }

    /// <summary>
    /// 弾の生成
    /// </summary>
    public void FireBullet()
    {
        if(isgameOver==true)
        {
            return;
        }
        else if(isgameClear==true)
        {
            return;
        }

        if (bullet != null)
        {
            if (playerController != null)
            {

                // PlayerControllerから向いている方向を取得
                Vector3 direction = playerController.GetLookDirection();

                // BulletのShootメソッドを呼び出して弾を発射
                bullet.Shoot(direction);

                bullet = null;

                StartCoroutine(UpdateBullet());
            }
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

    //ゲームオーバーを判定する
    public void GameOver()
    {
        gameoverText.SetText("GameOver!!");
        gameoverPanel.SetActive(true);
        isgameOver = true;
        GameObject.Find("Player").GetComponent<ObjCtrl>().GameModeChange();
        Time.timeScale = 0;
    }

    //ゲームクリアを判定する
    public void GameClear()
    {
        stageclearText.SetText("StageClear!!");
        isgameClear = true;
        GameObject.Find("Player").GetComponent<ObjCtrl>().GameModeChange();
        Time.timeScale = 0;

        Result();
    }

    public void Result()
    {
        Time.timeScale = 1;
        //画面遷移
        Initiate.DoneFading();
        Initiate.Fade("ResultScene", Color.white, 1.0f);
    }

    public void Retry()
    {
        Time.timeScale = 1;

        SceneManager.LoadScene("Stage" + currentStage);
    }

    private IEnumerator UpdateBullet()
    {
        yield return new WaitForSeconds(1.0f);

        // 色ランダム
        int rnd = Random.Range(0, juelPrefabs.Count);

        // 弾の生成
        bullet = Instantiate(bulletPrefab[rnd], firePoint.position + new Vector3(0, 0, -1.0f), Quaternion.identity);
    }

   static public void UpdateStageScene(int currentScene)
    {
        currentStage = currentScene;

        //画面遷移
        Initiate.DoneFading();
        Initiate.Fade("Stage"+currentStage, Color.white, 1.0f);
    }
}