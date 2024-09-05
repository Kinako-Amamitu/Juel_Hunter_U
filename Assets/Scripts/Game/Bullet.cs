using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

/// <summary>
/// �e�̃v���n�u�ɃA�^�b�`���鐧��p�N���X
/// </summary>
public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;�@// �e�̑��x
    [SerializeField] private float destroyTime;�@// �e�̐�������
    [SerializeField] TextMeshProUGUI gameoverText; //�Q�[���I�[�o�[�̃e�L�X�g
    [SerializeField] TextMeshProUGUI gamescoreText; //���_�̃e�L�X�g
    [SerializeField] GameObject over; //���C�ݒ�p�I�u�W�F�N�g
    [SerializeField] GameObject juel; //����Ɏg���p�̃W���G��



    PlayerController player; //�v���C���[

    [SerializeField] GameGenerator gameGenerator;
    //PlayerController playerController;
   

    int layerMask = 1 << 7;
    


    private void Start()
    {
       
        gameoverText=GameObject.Find("gameoverText").GetComponent<TextMeshProUGUI>();
        gameGenerator = GameObject.Find("GameGenerator").GetComponent<GameGenerator>();
        GetComponent<ObjCtrl>();
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

           RaycastHit2D hit=Physics2D.Raycast(player.transform.position, player.GetLookDirection(),10.0f, layerMask);
            if (hit.collider)
            {
                Debug.Log(hit.point);
               
                this.transform.DOMove(hit.point, 1.0f);
            }
        }

        // ��莞�Ԍ�ɒe��j�󂷂�
        //Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*
        if(collision.gameObject.tag=="Rail(right)")
        {
            GetComponent<Rigidbody2D>().velocity=new Vector2(0.1f,0);
        }       
        else if(collision.gameObject.tag=="Rail(down)")
        {
            GetComponent<Rigidbody2D>().velocity=new Vector2(0f,-0.1f);
        }
        */
        if(collision.gameObject.tag=="Out")
        {
            GameObject.Find("Player").GetComponent<ObjCtrl>().isgameMode = true;

            gameGenerator.GameOver();
           
        }
        else if(collision.gameObject.tag=="LeftWall")
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0.1f, 0);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Rail(right)")
        {
            bool isSameright=false;

            GetComponent<Rigidbody2D>().velocity = new Vector2(0.2f, 0);

            RaycastHit2D hit = Physics2D.Raycast(transform.position+transform.right*0.4f, transform.right, 0.6f);

            Debug.DrawRay(transform.position + transform.right * 0.4f, transform.right* 0.6f, Color.red, 5);
            

            if (hit.collider)
            {
                if (hit.collider.tag == gameObject.tag)
                {
                    isSameright = true;

                    RaycastHit2D hit2 = Physics2D.Raycast(transform.position - transform.right * 0.4f, transform.right*-1, 0.6f);

                    Debug.DrawRay(transform.position - transform.right * 0.4f, transform.right * -0.6f, Color.red, 5);

                    if (hit2.collider)
                    {
                        if(hit2.collider.tag==gameObject.tag)
                        {
                            //���E�̃W���G���Ǝ��g������
                            Destroy(gameObject);
                            Destroy(hit2.collider.gameObject);
                            Destroy(hit.collider.gameObject);

                            gameGenerator.Quest(3);
                            gameGenerator.AddScore(100);
                        }
                    }
                }
                
            }
        }
        else if (collision.gameObject.tag == "Rail(down)")
        {
           bool isSameright = false;

            GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, -0.2f);

            RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.up * 0.4f, transform.up, 0.6f);

            Debug.DrawRay(transform.position + transform.up * 0.4f, transform.up * 0.6f, Color.red, 5);

            if (hit.collider)
            {
                if (hit.collider.tag == gameObject.tag)
                {
                    isSameright = true;

                    RaycastHit2D hit2 = Physics2D.Raycast(transform.position - transform.up * 0.4f, transform.up * -1, 0.6f);

                    Debug.DrawRay(transform.position - transform.up * 0.4f, transform.up * -0.6f, Color.red, 5);

                    if (hit2.collider)
                    {
                        if (hit2.collider.tag == gameObject.tag)
                        {
                            //���E�̃W���G���Ǝ��g������
                            Destroy(gameObject);
                            Destroy(hit2.collider.gameObject);
                            Destroy(hit.collider.gameObject);

                            gameGenerator.Quest(3);
                            gameGenerator.AddScore(100);
                        }
                    }
                }

            }
        }
    }
}

