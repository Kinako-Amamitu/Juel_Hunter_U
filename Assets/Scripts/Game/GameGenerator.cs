using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


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

   

    // UI
    [SerializeField] TextMeshProUGUI textGameScore;
    [SerializeField] TextMeshProUGUI textGameTimer;
    

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

        //�Q�[���I��
        if (0 > gameTimer)
        {
            

            // Update�ɓ���Ȃ��悤�ɂ���
            enabled = false;
            // ���̎��_��Update���甲����
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
}