using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGenerator : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;     // �e�̃v���t�@�u
    [SerializeField] private Transform firePoint;     // ���˃|�C���g
    //[SerializeField] private float fireRate = 2.0f;   // ���ˊԊu(�b)
    [SerializeField] private PlayerController playerController;      // PlayerController�ւ̎Q��

    //private float timer = 0f;       // �^�C�}�[


    private void Update()
    {

        // �^�C�}�[���X�V
        //timer += Time.deltaTime;


            //timer = 0f; // �^�C�}�[�����Z�b�g
        
    }

    /// <summary>
    /// �e�̐���
    /// </summary>
    public void FireBullet()
    {

        // �e�̐���
        Bullet bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        if (playerController != null)
        {

            // PlayerController��������Ă���������擾
            Vector3 direction = playerController.GetLookDirection();

            // Bullet��Shoot���\�b�h���Ăяo���Ēe�𔭎�
            bullet.Shoot(direction);
        }
    }
}