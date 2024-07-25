using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallObject : MonoBehaviour
{
    [SerializeField]
    public new Renderer renderer;

    [SerializeField]
    public bool isTouch = false;

    public float MoveSpeed = 20.0f;         // �ړ��l

    int frameCount = 0;             // �t���[���J�E���g
    const int deleteFrame = 180;    // �폜�t���[��

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        // �ʒu�̍X�V
        transform.Translate(MoveSpeed * Time.deltaTime, 0, 0);

        // ���t���[���ŏ���
        //if (++frameCount > deleteFrame)
        //{
        //    Destroy(gameObject);
        //}
    }


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