using TMPro;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// �e�̃v���n�u�ɃA�^�b�`���鐧��p�N���X
/// </summary>
public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;�@// �e�̑��x
    [SerializeField] private float destroyTime;�@// �e�̐�������
    [SerializeField] TextMeshProUGUI gameoverText; //�Q�[���I�[�o�[�̃e�L�X�g
    [SerializeField] GameObject over; //���C�ݒ�p�I�u�W�F�N�g
    PlayerController player; //�v���C���[

    int layerMask = 1 << 7;
    




    private void Start()
    {
       
        gameoverText=GameObject.Find("gameoverText").GetComponent<TextMeshProUGUI>();

    }

    private void Update()
    {
        
    }
    /// <summary>
    /// �o���b�g����
    /// </summary>
    public void Shoot(Vector2 direction)
    {

        // �e�� Rigidbody2D �R���|�[�l���g���A�^�b�`����Ă��邩�m�F�������
        if (TryGetComponent(out Rigidbody2D rb))
        {

            player = GameObject.Find("Player").GetComponent<PlayerController>();

            Ray2D ray = new Ray2D(transform.position,transform.up);

           RaycastHit2D hit=Physics2D.Raycast(transform.position, player.GetLookDirection(),10.0f,layerMask);
            if (hit.collider)
            {
                Debug.Log(hit.point);
               
                this.transform.DOMove(hit.point, 1.0f);
            }
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, 5);
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
        else if(collision.gameObject.tag=="LeftWall")
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0.1f, 0);
        }
        else if(collision.gameObject.tag=="juel")
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0.1f, 0);
        }
    }
}

