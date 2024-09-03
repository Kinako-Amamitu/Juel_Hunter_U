using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GameGenerator : MonoBehaviour
{
    [SerializeField] List<Bullet> bulletPrefab;     // �e�̃v���t�@�u
    [SerializeField] private Transform firePoint;     // ���˃|�C���g
    //[SerializeField] private float fireRate = 2.0f;   // ���ˊԊu(�b)
    [SerializeField] private PlayerController playerController;      // PlayerController�ւ̎Q��
    [SerializeField] List<GameObject> juelPrefabs; //����Ɏg���p�̃W���G���v���n�u
    [SerializeField] private Transform outZone; //�Q�[���I�[�o�[����ʒu
    [SerializeField] float gameTimer; //�Q�[������

    //�t�B�[���h��̃A�C�e��
    List<GameObject> bullets;

    // �폜�ł���A�C�e����
    [SerializeField] int deleteCount;

    


    //�X�R�A
    int gameScore;

    //������
    int juelRequired = 9;



    // UI
    [SerializeField] TextMeshProUGUI textGameScore;
    [SerializeField] TextMeshProUGUI textGameTimer;
    [SerializeField] TextMeshProUGUI stageclearText;
    [SerializeField] TextMeshProUGUI gameoverText;
    [SerializeField] Text target1;
    [SerializeField] GameObject posemenuPanel;
    

    //private float timer = 0f;       // �^�C�}�[

    private void Start()
    {
      
        
        // �S�A�C�e��
        bullets = new List<GameObject>();
    }

    private void Update()
    {

        // �Q�[���^�C�}�[�X�V
        gameTimer -= Time.deltaTime;
        textGameTimer.text = "" + (int)gameTimer;

        if(juelRequired<=0)
        {
            target1.text ="OK�I�I" ;
            stageclearText.SetText("StageClear!!");

           

            return;
        }

        //�Q�[���I��
        if (0 >= gameTimer)
        {

            gameoverText.SetText("GameOver!!");
            // Update�ɓ���Ȃ��悤�ɂ���
            enabled = false;
          
;            // ���̎��_��Update���甲����
            return;
        }
    }

    /// <summary>
    /// �e�̐���
    /// </summary>
    public void FireBullet()
    {
        // �F�����_��
        int rnd = Random.Range(0, bulletPrefab.Count);
        // �e�̐���
    Bullet bullet = Instantiate(bulletPrefab[rnd], firePoint.position, Quaternion.identity);

        if (playerController != null)
        {

            // PlayerController��������Ă���������擾
            Vector3 direction = playerController.GetLookDirection();

            // Bullet��Shoot���\�b�h���Ăяo���Ēe�𔭎�
            bullet.Shoot(direction); 
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

}