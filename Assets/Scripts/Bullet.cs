using UnityEngine;

/// <summary>
/// �e�̃v���n�u�ɃA�^�b�`���鐧��p�N���X
/// </summary>
public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;�@// �e�̑��x
    [SerializeField] private float destroyTime;�@// �e�̐�������


    /// <summary>
    /// �o���b�g����
    /// </summary>
    public void Shoot(Vector2 direction)
    {

        // �e�� Rigidbody2D �R���|�[�l���g���A�^�b�`����Ă��邩�m�F�������
        if (TryGetComponent(out Rigidbody2D rb))
        {

            // Rigidbody2D �� AddForce ���\�b�h�𗘗p���āA�v���C���[�̐i�s�����Ɠ��������ɒe�𔭎˂���
            rb.AddForce(direction * bulletSpeed);
        }

        // ��莞�Ԍ�ɒe��j�󂷂�
        //Destroy(gameObject, destroyTime);
    }

    /// <summary>
    /// �Փ˔���
    /// </summary>
    /// <param name="other"></param>
    void OnCollisionEnter(Collision other)
    {
        //�{�[���ɂԂ������Ƃ�
        if (other.gameObject.tag == "Ball")
        {
            Debug.Log("�{�[���ɂԂ������I");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("�{�[������Ȃ��Ƃ���ɂԂ������I");
        }

    }
}

