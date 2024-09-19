using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{

    public AudioClip start; //�Q�[���X�^�[�g��
    AudioSource audioSource; //SE���͂ɃI�[�f�B�I�\�[�X���g�p����

    // Start is called before the first frame update
    void Start()
    {
        //AudioComponent���擾
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //�^�b�`�����Ƃ�
        /*if (Input.touchCount > 0)
        {
            Touch t1 = Input.GetTouch(0);
            if (t1.phase == TouchPhase.Began)
            {
                
            }
        }
        */
    }

    public void GameStart()
    {
        audioSource.PlayOneShot(start);
        bool isSuccess = NetworkManager.Instance.LoadUserData();
        if (!isSuccess)
        {
            //���[�U�[�f�[�^���ۑ�����Ă��Ȃ��ꍇ�͓o�^
            StartCoroutine(NetworkManager.Instance.RegistUser(Guid.NewGuid().ToString(), result => {
                //��ʑJ��
                Initiate.DoneFading();
                Initiate.Fade("StageSelect", Color.black, 0.5f);
            }));
        }
        else
        {
            //��ʑJ��
            Initiate.DoneFading();
            Initiate.Fade("StageSelect", Color.black, 0.5f);
        };
    }
}
