using TMPro;
using UnityEngine;

/// <summary>
/// �e�̃v���n�u�ɃA�^�b�`���鐧��p�N���X
/// </summary>
public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;�@// �e�̑��x
    [SerializeField] private float destroyTime;�@// �e�̐�������
    [SerializeField] TextMeshProUGUI gameoverText; //�Q�[���I�[�o�[�̃e�L�X�g


    private void Start()
    {
        gameoverText=GameObject.Find("gameoverText").GetComponent<TextMeshProUGUI>();
    }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Rail")
        {
            GetComponent<Rigidbody2D>().velocity=new Vector2(0.1f,0);
        }
        else if(collision.gameObject.tag=="Out")
        {
            gameoverText.SetText("GameOver!!");
        }
    }
}

