using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameGenerator : MonoBehaviour
{
    [SerializeField] List<Bullet> bulletPrefab;     // �e�̃v���t�@�u
    [SerializeField] private Transform[] firePoint;     // ���˃|�C���g
    //[SerializeField] private float fireRate = 2.0f;   // ���ˊԊu(�b)
    [SerializeField] private PlayerController[] playerController;      // PlayerController�ւ̎Q��
    [SerializeField] List<GameObject> juelPrefabs; //����Ɏg���p�̃W���G���v���n�u
    [SerializeField] private Transform outZone; //�Q�[���I�[�o�[����ʒu
    [SerializeField] float gameTimer; //�Q�[������
    [SerializeField]int juelRequired; //������
    [SerializeField] int playerNum; //�v���C���[��

    // �폜�ł���A�C�e����
    [SerializeField] int deleteCount;

    //�I�u�W�F�N�g�֐��ւ�
    ObjCtrl obj;

    //�X�R�A
    public static int gameScore;

    //�V�[���̔���ϐ�
    public static int currentStage;

    

    //�Q�[���̔���
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
    public AudioClip sound1; //���ˉ�
    public AudioClip sound2; //�W���G���폜��
    public AudioClip gameOver; //�Q�[���I�[�o�[��
    public AudioClip gameClear; //�Q�[���N���A��
    public AudioClip pageUp; //���j���[���J��
    public AudioClip pageDown; //���j���[�����
    public AudioClip select; //�ėp���艹
    public AudioClip cancel; //�ėp�L�����Z����
    AudioSource audioSource; //SE���͂ɃI�[�f�B�I�\�[�X���g�p����

    //private float timer = 0f;       // �^�C�}�[

    private void Start()
    {
        //�I�u�W�F�N�g�N���X���擾
        obj = GetComponent<ObjCtrl>();

        bullet = new Bullet[playerNum];

        //���U���g���擾
        result = GetComponent<Result>();


        //AudioComponent���擾
        audioSource = GetComponent<AudioSource>();

        //�N���A����������
        target1.text = juelRequired.ToString();

        //���e�̃W���G���𒊑I
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
        // �Q�[���^�C�}�[�X�V
        gameTimer -= Time.deltaTime;
        textGameTimer.text = "Time" + (int)gameTimer;

        if(juelRequired<=0)
        {
            target1.text ="OK�I�I" ;

            GameClear();
        }

        //�Q�[���I��
        if (gameTimer <= 0)
        {
            
         

            GameOver();

            // Update�ɓ���Ȃ��悤�ɂ���
            enabled = false;
          
;            // ���̎��_��Update���甲����
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
    /// �e�̐���
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

                // PlayerController��������Ă���������擾
                Vector3 direction = playerController[Num].GetLookDirection();

                // Bullet��Shoot���\�b�h���Ăяo���Ēe�𔭎�
                bullet[Num].Shoot(direction,playerController[Num]);

                bullet[Num] = null;

                StartCoroutine(UpdateBullet(Num));
            }
        }
    }

    //�_�������Z������
    public void AddScore(int score)
    {
        gameScore += score;
        textGameScore.text = "Score:"+gameScore.ToString();
    }

    //�N���A������B��������
    public void Quest(int target)
    {
        audioSource.PlayOneShot(sound2);
        juelRequired -= target;
        target1.text = juelRequired.ToString();
    }

    /// <summary>
    /// �|�[�Y���j���[�֘A
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

    //���X�^�[�g����
    public void Restart()
    {
        posemenuPanel.SetActive(false);
        Time.timeScale = 1;
        audioSource.PlayOneShot(pageDown);
    }

    //�X�e�[�W�Z���N�g�ɖ߂�
    public void StageSelect()
    {
        Time.timeScale = 1;
        audioSource.PlayOneShot(cancel);
        //��ʑJ��
        Initiate.DoneFading();
        Initiate.Fade("StageSelect", Color.black, 0.5f);
    }

    //�Q�[���I�[�o�[�𔻒肷��
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

    //�Q�[���N���A�𔻒肷��
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
        //��ʑJ��
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

        // �F�����_��
        int rnd = UnityEngine.Random.Range(0, juelPrefabs.Count);

        // �e�̐���
        bullet[Num] = Instantiate(bulletPrefab[rnd], firePoint[Num].position + new Vector3(0, 0, -1.0f), Quaternion.identity);
    }

   static public void UpdateStageScene(int currentScene)
    {
        currentStage = currentScene;

        //��ʑJ��
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