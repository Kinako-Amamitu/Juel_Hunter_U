using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BulletGenerator : MonoBehaviour
{
    [SerializeField] List<Bullet> bulletPrefab;     // �e�̃v���t�@�u
    [SerializeField] private Transform firePoint;     // ���˃|�C���g
    //[SerializeField] private float fireRate = 2.0f;   // ���ˊԊu(�b)
    [SerializeField] private PlayerController playerController;      // PlayerController�ւ̎Q��

    //�t�B�[���h��̃A�C�e��
    List<GameObject> bullets;

    // �폜�ł���A�C�e����
    [SerializeField] int deleteCount;

    //�X�R�A
    int gameScore;

    // UI
    [SerializeField] TextMeshProUGUI textGameScore;
    [SerializeField] TextMeshProUGUI textGameTimer;
    [SerializeField] GameObject panelGameResult;

    //private float timer = 0f;       // �^�C�}�[

    private void Start()
    {
        // �S�A�C�e��
        bullets = new List<GameObject>();
    }

    private void Update()
    {

    

        // �폜���ꂽ�A�C�e�����N���A
        bullets.RemoveAll(item => item == null);

        
            // �����蔻�肪�������I�u�W�F�N�g
            //GameObject obj = hit2d.collider.gameObject;
            //CheckItems(obj);

        // �^�C�}�[���X�V
        //timer += Time.deltaTime;


        //timer = 0f; // �^�C�}�[�����Z�b�g

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

    // �����Ɠ����F�̃A�C�e�����폜����
    void DeleteItems(List<GameObject> checkItems)
    {

        // �폜�\���ɒB���Ă��Ȃ������牽�����Ȃ�
        if (checkItems.Count < deleteCount) return;

        // �폜���ăX�R�A���Z
        List<GameObject> destroyItems = new List<GameObject>();
        foreach (var item in checkItems)
        {
            // ���Ԃ�Ȃ��̍폜�����A�C�e�����J�E���g
            if (!destroyItems.Contains(item))
            {
                destroyItems.Add(item);
            }

            //�폜
            Destroy(item);
        }

        // ���ۂɍ폜���ăX�R�A���Z
        gameScore += destroyItems.Count * 100;

        // �X�R�A�\���X�V
        textGameScore.text = "" + gameScore;
    }

    //�����F�̃A�C�e����Ԃ�
    List<GameObject> GetSameItems(GameObject target)
    {
        List<GameObject> ret = new List<GameObject>();

        foreach (var item in bullets)
        {
            // �A�C�e�����Ȃ��A�����A�C�e���A�Ⴄ�F�A�����������ꍇ�̓X�L�b�v
            if (!item || target == item) continue;

            if (item.GetComponent<SpriteRenderer>().sprite
                != target.GetComponent<SpriteRenderer>().sprite)
            {
                continue;
            }

            float distance
                = Vector2.Distance(target.transform.position, item.transform.position);

            if (distance > 1.1f) continue;

            // �����܂ŗ�����A�C�e���ǉ�
            ret.Add(item);
        }

        return ret;
    }

    // �����Ɠ����F�̃A�C�e����T��
    void CheckItems(GameObject target)
    {
        // ���̃A�C�e���Ɠ����F��ǉ�����
        List<GameObject> checkItems = new List<GameObject>();

        //������ǉ�
        checkItems.Add(target);

        // �`�F�b�N�ς̃C���f�b�N�X
        int checkIndex = 0;

        // checkItems�̍ő�l�܂Ń��[�v
        while (checkIndex < checkItems.Count)
        {
            // �אڂ��铯���F���擾
            List<GameObject> sameItems = GetSameItems(checkItems[checkIndex]);
            // �`�F�b�N�ς̃C���f�b�N�X��i�߂�
            checkIndex++;

            // �܂��ǉ�����ĂȂ��A�C�e����ǉ�����
            foreach (var item in sameItems)
            {
                if (checkItems.Contains(item)) continue;
                checkItems.Add(item);
            }
        }

        // �폜
        DeleteItems(checkItems);
    }
}