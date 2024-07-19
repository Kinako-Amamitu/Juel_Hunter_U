using UnityEngine;

public class FireBullet : MonoBehaviour
{
    public float fMoveSpeed = 7.0f;     // �ړ��l
    //---------------------------
    //          �ǉ�
    public GameObject BulletObj;        // �e�̃Q�[���I�u�W�F�N�g
    //---------------------------
    Vector3 bulletPoint;                // �e�̈ʒu

    void Start()
    {
        bulletPoint = transform.Find("BulletPoint").localPosition;
    }
    // Update is called once per frame
    void Update()
	{
        // �{�^�����������Ƃ�
        if (Input.GetButtonDown("Fire1"))
        {
            // �e�̐���
            Instantiate(BulletObj, transform.position + bulletPoint, Quaternion.identity);

        }
    }
}