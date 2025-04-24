////////////////////////////////////////////////////////////////
///
/// ゲーム本編を管理するスクリプト
/// 
/// Aughter:木田晃輔
///
////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Newtonsoft.Json;
using UnityEngine.Networking;

public class GameGenerator : MonoBehaviour
{

    /// <summary>
    /// GameGenerator限定変数
    /// </summary>
    private int userID = 0;  //自分のユーザーID
    private int stageID = 0; //ステージID
    private int score = 0; //スコア
    private int bombFlag = 0; //ボム生成確定フラグ

    /// <summary>
    /// Unityアタッチ用変数宣言
    /// </summary>
    [SerializeField] List<Bullet> bulletPrefab;     // 弾のプレファブ
    [SerializeField] private Transform[] firePoint;     // 発射ポイント
    [SerializeField] private PlayerController[] playerController;      // PlayerControllerへの参照
    [SerializeField] List<GameObject> juelPrefabs; //判定に使う用のジュエルプレハブ
    [SerializeField] private Transform outZone; //ゲームオーバー判定位置
    [SerializeField] float gameTimer; //ゲーム時間
    [SerializeField] int juelRequired; //クリア条件個数
    [SerializeField] int playerNum; //プレイヤー数
    [SerializeField] int deleteCount;  // 削除できるアイテム数





    /// <summary>
    /// ゲームスコアを他のシーンに継承する変数
    /// </summary>
    public static int gameScore;//スコア

    public int Score
    {
        get { return gameScore; }
    }

    /// <summary>
    /// シーンの判定変数
    /// </summary>
    public static int currentStage;

    /// <summary>
    /// ゲームの判定
    /// </summary>
    public bool isgameOver = false; //ゲームオーバー判定
    public bool isgameClear = false;//ゲームクリア判定

    /// <summary>
    /// クラスの継承
    /// </summary>
    Bullet[] bullet; //バレットクラス(配列)
    Result result; //リザルトクラス
    ObjCtrl obj; //オブジェクトクラス

    /// <summary>
    /// UI
    /// </summary>
    [SerializeField] Text textGameScore; //ゲームスコアテキスト
    [SerializeField] Text textGameTimer; //ゲームタイマーテキスト
    [SerializeField] Text stageclearText;//ステージクリアテキスト
    [SerializeField] Text gameoverText;  //ゲームオーバーテキスト
    [SerializeField] Text target1;       //ジュエルを消す数表示テキスト
    [SerializeField] GameObject posemenuPanel; //ポーズメニューのパネル
    [SerializeField] GameObject gameoverPanel; //ゲームオーバー時のパネル


    /// <summary>
    /// 音関連
    /// </summary>
    AudioSource audioSource; //BGM,SE入力にオーディオソースを使用する

    //BGM
    private GameObject ScoreAttack;     //スコアアタック用スタートBGM
    private GameObject ScoreAttackLoop; //スコアアタック用ループBGM

    //SE
    public AudioClip sound1; //発射音
    public AudioClip sound2; //ジュエル削除音
    public AudioClip gameOver; //ゲームオーバー時
    public AudioClip gameClear; //ゲームクリア時
    public AudioClip pageUp; //メニューを開く
    public AudioClip pageDown; //メニューを閉じる
    public AudioClip select; //汎用決定音
    public AudioClip cancel; //汎用キャンセル音
    public AudioClip explosion; //爆発音
    public AudioClip ninja; //忍者死亡音
    public AudioClip delete; //発射しっぱい音

    private void Start()
    {
        //点数初期化
        gameScore = 0;

        //オブジェクトクラスを取得
        obj = GetComponent<ObjCtrl>();

        bullet = new Bullet[playerNum];

        //リザルトを取得
        result = GetComponent<Result>();


        //AudioComponentを取得
        audioSource = GetComponent<AudioSource>();

        //スコアアタック用の音楽オブジェクトを探す
        ScoreAttack = GameObject.Find("BGM");
        ScoreAttackLoop = GameObject.Find("LoopBGM");

        //ループ用音楽チェック
        if(ScoreAttackLoop!=null)
        {//設定されていたら

            //DelayMethodを80秒後に呼び出す
            Invoke(nameof(BGMOver), 80.0f);
        }
        
        //クリア条件チェック
        if (target1 != null)
        {//設定されていたら

            //クリア条件初期化
            target1.text = juelRequired.ToString();
        }
        else
        {//それ以外

        }

        //初弾のジュエルを抽選
        for (int i = 0; i < playerNum; i++)
        {//プレイヤー数の分ループ
            StartCoroutine(UpdateBullet(i)); //ジュエルをランダムに生成する
        }
        Invoke(nameof(GameClear), 22.3f);
    }

    private void Update()
    {
        if (isgameOver == true|| isgameClear == true)
        {//ゲームオーバーになったら
            return; //Updateに入らない
        }

        // ゲームタイマー更新
        gameTimer -= Time.deltaTime;
        textGameTimer.text = "Time:" + (int)gameTimer;

        //指定個数分ジュエルを消したら
        if (juelRequired <= 0)
        {
            target1.text = "OK！！";

            GameClear(); //ゲームクリア
        }

        //ゲーム終了
        if (gameTimer <= 0)
        {
            
            if(currentStage==11||currentStage==12)
            {//スコアアタック用
                GameSet();

                // Updateに入らないようにする
                enabled = false;

                // この時点でUpdateから抜ける
                return;
            }
            else
            {//それ以外
                GameOver();

                // Updateに入らないようにする
                enabled = false;

                // この時点でUpdateから抜ける
                return;
            }
           
        }

        //ジュエルはプレイヤーの発射地点を追尾
        for (int i = 0; i < playerNum; i++)
        {
            if (bullet[i] != null)
            {
                bullet[i].transform.position = new Vector3(firePoint[i].transform.position.x, firePoint[i].transform.position.y, bullet[i].transform.position.z);
            }
        }
    }

    /// <summary>
    /// BGMのループ処理関数
    /// </summary>
    public void BGMOver()
    {
        audioSource = ScoreAttack.GetComponent<AudioSource>();
        audioSource.Stop();
        audioSource = ScoreAttackLoop.GetComponent<AudioSource>();
        audioSource.Play();
    }
    /// <summary>
    /// 弾の発射
    /// </summary>
    public void FireBullet(int Num)
    {
        if (isgameOver == true)
        {// ゲームオーバーになったら
            return; //FireBulletに入らない
        }
        else if (isgameClear == true)
        {// ゲームをクリアしたら
            return; //FireBulletに入らない
        }

        //ジュエルのヌルチェック
        if (bullet[Num] != null)
        {
            if (playerController[Num] != null)
            {//プレイヤーが使われていたら
                // 発射音
                audioSource.PlayOneShot(sound1);

                // PlayerControllerから向いている方向を取得
                Vector3 direction = playerController[Num].GetLookDirection();

                // BulletのShootメソッドを呼び出して弾を発射
                bullet[Num].Shoot(direction, playerController[Num]);

                // バレット状態を解除
                bullet[Num] = null;

                // ジュエルをランダムに生成
                StartCoroutine(UpdateBullet(Num));
            }
        }
    }

    /// <summary>
    /// 点数を加点する
    /// </summary>
    /// <param name="score"></param>
    public void AddScore(int score)
    {
        gameScore += score;
        textGameScore.text = "Score:" + gameScore.ToString();
    }

    /// <summary>
    /// クリア条件の数を減らす
    /// </summary>
    /// <param name="target"></param>
    public void Quest(int target)
    {
        audioSource.PlayOneShot(sound2); //ジュエル取得音
        juelRequired -= target; //取得分減らす

        
        if(target1==null)
        {//スコアアタック用

        }
        else
        {//それ以外
            target1.text = juelRequired.ToString();
        }
        
    }

    /// <summary>
    /// ポーズメニュー関連
    /// </summary>
    /// 
    //ポーズする
    public void Pose()
    {
        audioSource.PlayOneShot(pageUp); //メニューを開く音
        Time.timeScale = 0;//時間を止める

        //プレイヤーを動かさないようにする
        for (int i = 0; i < playerNum; i++)
        {
            playerController[i].isplayerMode = true;
        }
        GameObject.Find("Player").GetComponent<ObjCtrl>().GameModeChange();
        if (playerNum == 2)
        {//プレイヤーが二人いたら
            GameObject.Find("Player2").GetComponent<ObjCtrl>().GameModeChange();
        }
        posemenuPanel.SetActive(true); //メニューパネルを表示
    }
    //メニューからゲームにもどる
    public void Restart()
    {
        posemenuPanel.SetActive(false); //メニューを閉じる
        Time.timeScale = 1; //時間を通常に戻す

        //プレイヤーを動かす
        for (int i = 0; i < playerNum; i++)
        {
            playerController[i].isplayerMode = false;
        }
        GameObject.Find("Player").GetComponent<ObjCtrl>().GameModeChange();

        if (playerNum == 2)
        {
            GameObject.Find("Player2").GetComponent<ObjCtrl>().GameModeChange();
        }
        audioSource.PlayOneShot(pageDown); //メニューを閉じる音
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

    /// <summary>
    /// ゲームオーバー
    /// </summary>
    public void GameOver()
    {
        if (currentStage == 7 || currentStage == 8)
        {//和風ステージの場合
            gameoverText.text = "任務失敗!!";
        }
        else
        {//それ以外
            gameoverText.text = "GameOver!!";
        }
        audioSource.PlayOneShot(gameOver); //ゲームオーバー音
        gameoverPanel.SetActive(true); //ゲームオーバー時のパネル表示
        isgameOver = true; //ゲームオーバー判定アクティブ

        //プレイヤーの動きを止める
        GameObject.Find("Player").GetComponent<ObjCtrl>().GameModeChange();

        if (playerNum == 2)
        {//2人いたら
            GameObject.Find("Player2").GetComponent<ObjCtrl>().GameModeChange();
        }
        Time.timeScale = 0; //時間を止める
    }

    /// <summary>
    /// ゲームクリア
    /// </summary>
    public void GameClear()
    {
        NetworkManager.Instance.StageProgress(currentStage); //ステージクリア状況をネットワーク判定
        audioSource.PlayOneShot(gameClear); //ゲームクリア音
        stageclearText.text = "StageClear!!"; //ステージクリア状況をテキスト表示
        AddScore((int)Math.Ceiling(gameTimer) * 30); //残り時間をスコアに加算
        isgameClear = true; //ゲームクリア判定アクティブ

        //プレイヤーの動きを止める
        GameObject.Find("Player").GetComponent<ObjCtrl>().GameModeChange();
        if (playerNum == 2)
        {
            GameObject.Find("Player2").GetComponent<ObjCtrl>().GameModeChange();
        }
        Time.timeScale = 0;//時間を止める

        //リザルト遷移処理
        Result();
    }

    //発射しっぱい
    public void FaildShoot()
    {
        audioSource.PlayOneShot(delete);
    }

    //忍者死亡
    public void NinjaDead()
    {
        audioSource.PlayOneShot(ninja); //忍者死亡音
    }

    //爆発音の処理
    public void Explosion()
    {
        audioSource.PlayOneShot(explosion); //爆発音
    }

    /// <summary>
    /// スコアアタック用ゲームセット処理
    /// </summary>
    public void GameSet()
    {
        gameoverText.text = "GameSet!!";//テキスト表示
        gameoverPanel.SetActive(true);//ゲームセット用パネル表示

        //プレイヤーの動きを止める
        GameObject.Find("Player").GetComponent<ObjCtrl>().GameModeChange();

        if (playerNum == 2)
        {
            GameObject.Find("Player2").GetComponent<ObjCtrl>().GameModeChange();
        }
        Time.timeScale = 0;
    }

    /// <summary>
    /// リザルト遷移処理
    /// </summary>
    public void Result()
    {
        Time.timeScale = 1; //時間を動かす

        //スコアをデータベースに送信
        StartCoroutine(NetworkManager.Instance.RegistScore(gameScore, currentStage, result =>
        {
            textGameScore.text = gameScore.ToString();
        }));

        //画面遷移
        Initiate.DoneFading();
        Initiate.Fade("ResultScene", Color.white, 1.0f);


    }
    /// <summary>
    /// ゲームオーバー時ステージをやり直す
    /// </summary>
    public void Retry()
    {
        audioSource.PlayOneShot(select); //選択音
        Time.timeScale = 1;//時間を動かす

        SceneManager.LoadScene("Stage" + currentStage); //同じステージをやる
    }

    /// <summary>
    /// ジュエルの生成
    /// </summary>
    /// <param name="Num"></param>
    /// <returns></returns>
    private IEnumerator UpdateBullet(int Num)
    {
        //ボムフラグ変数加算
        bombFlag++;

        //1秒待ってから
        yield return new WaitForSeconds(1.0f);

        // 色ランダム
        int rnd = UnityEngine.Random.Range(0, juelPrefabs.Count);

        //5回目はボム生成確定
        if (bombFlag==5)
        {
            //ボムのプレハブの値
            rnd = 0;

            //ボムフラグ初期化
            bombFlag = 0;
        }
      
        //ボムが選ばれたら
        if(rnd==0)
        {
            //ボムフラグ初期化
            bombFlag = 0;
        }

        // 弾の生成
        bullet[Num] = Instantiate(bulletPrefab[rnd], firePoint[Num].position + new Vector3(0, 0, -1.0f), Quaternion.identity);
    }

    /// <summary>
    /// 指定ステージに遷移する
    /// </summary>
    /// <param name="currentScene"></param>
    static public void UpdateStageScene(int currentScene)
    {
        currentStage = currentScene;

        //画面遷移
        Initiate.DoneFading();
        Initiate.Fade("Stage" + currentStage, Color.white, 1.0f);
    }

    /// <summary>
    /// スコアを継承
    /// </summary>
    /// <returns></returns>
    public static int Scoreset()
    {
        return gameScore;
    }

    /// <summary>
    /// ステージを継承
    /// </summary>
    /// <returns></returns>
    public static int Stageset()
    {
        return currentStage;
    }
}