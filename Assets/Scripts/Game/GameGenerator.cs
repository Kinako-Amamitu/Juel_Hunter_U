using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameGenerator : MonoBehaviour
{
    [SerializeField] List<Bullet> bulletPrefab;     // �e�̃v���t�@�u
    [SerializeField] private Transform firePoint;     // ���˃|�C���g
    //[SerializeField] private float fireRate = 2.0f;   // ���ˊԊu(�b)
    [SerializeField] private PlayerController playerController;      // PlayerController�ւ̎Q��
    [SerializeField] List<GameObject> juelPrefabs; //����Ɏg���p�̃W���G���v���n�u
    [SerializeField] private Transform outZone; //�Q�[���I�[�o�[����ʒu
    [SerializeField] float gameTimer; //�Q�[������
    [SerializeField]int juelRequired; //������

    //�t�B�[���h��̃A�C�e��
    List<GameObject> bullets;

    // �폜�ł���A�C�e����
    [SerializeField] int deleteCount;


    ObjCtrl obj;

    //�X�R�A
    int gameScore;

    //�V�[���̔���ϐ�
    static int currentStage;

    

    //�Q�[���̔���
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
    

    //private float timer = 0f;       // �^�C�}�[

    private void Start()
    {
        //�I�u�W�F�N�g�N���X���擾
        obj = GetComponent<ObjCtrl>();

        //�N���A����������
        target1.text = juelRequired.ToString();

        // �S�A�C�e��
        bullets = new List<GameObject>();

        //���e�̃W���G���𒊑I
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
        // �Q�[���^�C�}�[�X�V
        gameTimer -= Time.deltaTime;
        textGameTimer.text = "" + (int)gameTimer;

        if(juelRequired<=0)
        {
            target1.text ="OK�I�I" ;

            GameClear();
        }

        //�Q�[���I��
        if (0 >= gameTimer)
        {
            
         

            GameOver();

            // Update�ɓ���Ȃ��悤�ɂ���
            enabled = false;
          
;            // ���̎��_��Update���甲����
            return;
        }

        if(bullet!=null)
        {
            bullet.transform.position = new Vector3(firePoint.transform.position.x, firePoint.transform.position.y, bullet.transform.position.z);
        }
    }

    /// <summary>
    /// �e�̐���
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

                // PlayerController��������Ă���������擾
                Vector3 direction = playerController.GetLookDirection();

                // Bullet��Shoot���\�b�h���Ăяo���Ēe�𔭎�
                bullet.Shoot(direction);

                bullet = null;

                StartCoroutine(UpdateBullet());
            }
        }
    }

    //�_�������Z������
    public void AddScore(int score)
    {
        gameScore += score;
        textGameScore.text = gameScore.ToString();
    }

    //�N���A������B��������
    public void Quest(int target)
    {
        juelRequired -= target;
        target1.text = juelRequired.ToString();
    }

    /// <summary>
    /// �|�[�Y���j���[�֘A
    /// </summary>
    public void Pose()
    {
        Time.timeScale = 0;
        posemenuPanel.SetActive(true);
    }

    //���X�^�[�g����
    public void Restart()
    {
        posemenuPanel.SetActive(false);
        Time.timeScale = 1;
    }

    //�ŏ������蒼��
    public void Reset()
    {
        Time.timeScale = 1;
        //��ʑJ��
        Initiate.DoneFading();
        Initiate.Fade("Stage1", Color.black, 0.5f);
    
    }

    //�X�e�[�W�Z���N�g�ɖ߂�
    public void StageSelect()
    {
        Time.timeScale = 1;
        //��ʑJ��
        Initiate.DoneFading();
        Initiate.Fade("StageSelect", Color.black, 0.5f);
    }

    //�Q�[���I�[�o�[�𔻒肷��
    public void GameOver()
    {
        gameoverText.SetText("GameOver!!");
        gameoverPanel.SetActive(true);
        isgameOver = true;
        GameObject.Find("Player").GetComponent<ObjCtrl>().GameModeChange();
        Time.timeScale = 0;
    }

    //�Q�[���N���A�𔻒肷��
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
        //��ʑJ��
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

        // �F�����_��
        int rnd = Random.Range(0, juelPrefabs.Count);

        // �e�̐���
        bullet = Instantiate(bulletPrefab[rnd], firePoint.position + new Vector3(0, 0, -1.0f), Quaternion.identity);
    }

   static public void UpdateStageScene(int currentScene)
    {
        currentStage = currentScene;

        //��ʑJ��
        Initiate.DoneFading();
        Initiate.Fade("Stage"+currentStage, Color.white, 1.0f);
    }
}