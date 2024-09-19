using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameGenerator : MonoBehaviour
{
    [SerializeField] List<Bullet> bulletPrefab;     // 弾のプレファブ
    [SerializeField] private Transform[] firePoint;     // 発射ポイント
    //[SerializeField] private float fireRate = 2.0f;   // 発射間隔(秒)
    [SerializeField] private PlayerController[] playerController;      // PlayerControllerへの参照
    [SerializeField] List<GameObject> juelPrefabs; //判定に使う用のジュエルプレハブ
    [SerializeField] private Transform outZone; //ゲームオーバー判定位置
    [SerializeField] float gameTimer; //ゲーム時間
    [SerializeField]int juelRequired; //条件個数
    [SerializeField] int playerNum; //プレイヤー数

    // 削除できるアイテム数
    [SerializeField] int deleteCount;

    //オブジェクト関数への
    ObjCtrl obj;

    //スコア
    public static int gameScore;

    //シーンの判定変数
    public static int currentStage;

    

    //ゲームの判定
    public bool isgameOver=false;
    public bool isgameClear = false;

    Bullet[] bullet;
    Result result;


    // UI
    [SerializeField] TextMeshProUGUI textGameScore;
    [SerializeField] TextMeshProUGUI textGameTimer;
    [SerializeField] TextMeshProUGUI stageclearText;
    [SerializeField] TextMeshProUGUI gameoverText;
    [SerializeField] Text target1;
    [SerializeField] GameObject posemenuPanel;
    [SerializeField] GameObject gameoverPanel;

    //SE
    public AudioClip sound1; //発射音
    public AudioClip sound2; //ジュエル削除音
    public AudioClip gameOver; //ゲームオーバー時
    public AudioClip gameClear; //ゲームクリア時
    public AudioClip pageUp; //メニューを開く
    public AudioClip pageDown; //メニューを閉じる
    public AudioClip select; //汎用決定音
    public AudioClip cancel; //汎用キャンセル音
    AudioSource audioSource; //SE入力にオーディオソースを使用する

    //private float timer = 0f;       // タイマー

    private void Start()
    {
        //オブジェクトクラスを取得
        obj = GetComponent<ObjCtrl>();

        bullet = new Bullet[playerNum];

        //リザルトを取得
        result = GetComponent<Result>();


        //AudioComponentを取得
        audioSource = GetComponent<AudioSource>();

        //クリア条件初期化
        target1.text = juelRequired.ToString();

        //初弾のジュエルを抽選
        for(int i=0;i<playerNum;i++)
        {
            StartCoroutine(UpdateBullet(i));
        }
        
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
        textGameTimer.text = "Time" + (int)gameTimer;

        if(juelRequired<=0)
        {
            target1.text ="OK！！" ;

            GameClear();
        }

        //ゲーム終了
        if (gameTimer <= 0)
        {
            
         

            GameOver();

            // Updateに入らないようにする
            enabled = false;
          
;            // この時点でUpdateから抜ける
            return;
        }

        for (int i = 0; i < playerNum; i++)
        {
            if (bullet[i] != null)
            {
                bullet[i].transform.position = new Vector3(firePoint[i].transform.position.x, firePoint[i].transform.position.y, bullet[i].transform.position.z);
            }
        }
    }

    /// <summary>
    /// 弾の生成
    /// </summary>
    public void FireBullet(int Num)
    {
        if(isgameOver==true)
        {
            return;
        }
        else if(isgameClear==true)
        {
            return;
        }


        if (bullet[Num] != null)
        {
            if (playerController[Num] != null)
            {

                audioSource.PlayOneShot(sound1);

                // PlayerControllerから向いている方向を取得
                Vector3 direction = playerController[Num].GetLookDirection();

                // BulletのShootメソッドを呼び出して弾を発射
                bullet[Num].Shoot(direction,playerController[Num]);

                bullet[Num] = null;

                StartCoroutine(UpdateBullet(Num));
            }
        }
    }

    //点数を加算させる
    public void AddScore(int score)
    {
        gameScore += score;
        textGameScore.text = "Score:"+gameScore.ToString();
    }

    //クリア条件を達成させる
    public void Quest(int target)
    {
        audioSource.PlayOneShot(sound2);
        juelRequired -= target;
        target1.text = juelRequired.ToString();
    }

    /// <summary>
    /// ポーズメニュー関連
    /// </summary>
    public void Pose()
    {
        audioSource.PlayOneShot(pageUp);
        Time.timeScale = 0;
        for (int i = 0; i < playerNum; i++)
        {
            playerController[i].isplayerMode = true;
        }
            
        posemenuPanel.SetActive(true);
    }

    //リスタートする
    public void Restart()
    {
        posemenuPanel.SetActive(false);
        Time.timeScale = 1;
        audioSource.PlayOneShot(pageDown);
    }

    //ステージセレクトに戻る
    public void StageSelect()
    {
        Time.timeScale = 1;
        audioSource.PlayOneShot(cancel);
        //画面遷移
        Initiate.DoneFading();
        Initiate.Fade("StageSelect", Color.black, 0.5f);
    }

    //ゲームオーバーを判定する
    public void GameOver()
    {
        gameoverText.SetText("GameOver!!");
        audioSource.PlayOneShot(gameOver);
        gameoverPanel.SetActive(true);
        isgameOver = true;
        GameObject.Find("Player").GetComponent<ObjCtrl>().GameModeChange();
        
        if(playerNum==2)
        {
            GameObject.Find("Player2").GetComponent<ObjCtrl>().GameModeChange();
        }
        Time.timeScale = 0;
    }

    //ゲームクリアを判定する
    public void GameClear()
    {
        NetworkManager.Instance.StageProgress(currentStage);
        audioSource.PlayOneShot(gameClear);
        stageclearText.SetText("StageClear!!");
        AddScore((int)Math.Ceiling(gameTimer)*30);
        isgameClear = true;
        GameObject.Find("Player").GetComponent<ObjCtrl>().GameModeChange();
        if (playerNum == 2)
        {
            GameObject.Find("Player2").GetComponent<ObjCtrl>().GameModeChange();
        }
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
        audioSource.PlayOneShot(select);
        Time.timeScale = 1;

        SceneManager.LoadScene("Stage" + currentStage);
    }

    private IEnumerator UpdateBullet(int Num)
    {
        yield return new WaitForSeconds(1.0f);

        // 色ランダム
        int rnd = UnityEngine.Random.Range(0, juelPrefabs.Count);

        // 弾の生成
        bullet[Num] = Instantiate(bulletPrefab[rnd], firePoint[Num].position + new Vector3(0, 0, -1.0f), Quaternion.identity);
    }

   static public void UpdateStageScene(int currentScene)
    {
        currentStage = currentScene;

        //画面遷移
        Initiate.DoneFading();
        Initiate.Fade("Stage"+currentStage, Color.white, 1.0f);
    }

    public static int Scoreset()
    {
        return gameScore;
    }

    public static int Stageset()
    {
        return currentStage;
    }
}