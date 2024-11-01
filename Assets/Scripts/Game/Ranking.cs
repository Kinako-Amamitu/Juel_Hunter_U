using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.UI;



public class Ranking : MonoBehaviour
{
    //�X�e�[�W�̏��
    int currentStage;

    /// <summary>
    /// Unity�A�^�b�`�ϐ�
    /// </summary>
    [SerializeField] Text stage; //�X�e�[�W
    [SerializeField] Text[] ranking; //�����L���O(�X�R�A)
    [SerializeField] Text[] rankingCurrent; //�����L���O�̏���

    /// <summary>
    /// ���֘A
    /// </summary>
    AudioSource audioSource; //BGM,SE���͂ɃI�[�f�B�I�\�[�X���g�p����

    public AudioClip select; //�ėp���艹
    public AudioClip cancel; //�ėp�L�����Z����


    /// <summary>
    /// �����L���O��ǂݍ���
    /// </summary>
    public void LordRanking()
    {
        //�f�[�^�x�[�X����X�R�A�擾
        StartCoroutine(NetworkManager.Instance.GetScore(currentStage, result =>
        {
            for(int i=0;i<ranking.Length;i++)
            {//�����L���O�̐�����
                if(result.Length<=i)
                {//�f�[�^�������[�v���𒴂�����
                    break;
                }

                if (i==0)
                {//1��
                    // �F���w��
                    rankingCurrent[i].color = new Color(1.0f, 0.92f, 0.016f, 1.0f);
                    rankingCurrent[i].text = string.Format("1st");
                    // �F���w��
                    ranking[i].color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
                    ranking[i].text += string.Format("{0}",result[i].Score);
                }
                else if (i == 1)
                {//2��
                    // �F���w��
                    rankingCurrent[i].color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
                    rankingCurrent[i].text = string.Format("2nd");
                    // �F���w��
                    ranking[i].color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
                    ranking[i].text += string.Format("{0}", result[i].Score);
                }
                else if (i == 2)
                {//3��
                    // �F���w��
                    rankingCurrent[i].color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
                    rankingCurrent[i].text = string.Format("3rd");
                    // �F���w��
                    ranking[i].color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
                    ranking[i].text += string.Format("{0}", result[i].Score);
                }
                else
                {//����ȊO
                    // �F���w��
                    rankingCurrent[i].color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
                    rankingCurrent[i].text = string.Format("{0}th",i+1);
                    // �F���w��
                    ranking[i].color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
                    ranking[i].text += string.Format("{0}", result[i].Score);
                }

            }
            
        }));
    }

    void Start()
    {
        //AudioComponent���擾
        audioSource = GetComponent<AudioSource>();

        //�����L���O�\�����Ă�X�e�[�W��\��
        currentStage = GameGenerator.Stageset();
        if (currentStage == 0)
        {
            currentStage = 1;
            stage.text = string.Format("STAGE: {0}", currentStage);
        }
        else
        {
            stage.text = string.Format("STAGE: {0}", currentStage);
        }
        
        //�����L���O��ǂݍ���
        LordRanking();
    }

    /// <summary>
    /// �O�X�e�[�W�̃����L���O����
    /// </summary>
    public void RankingDown()
    {
        if(currentStage==1)
        {//�X�e�[�W1�̏ꍇ
            currentStage += 11;
        }
        else
        {
            currentStage--;
        }
        stage.text = string.Format("STAGE: {0}", currentStage);

        for (int i = 0; i < ranking.Length; i++)
        {//�O�̃����L���O�͏����Ă���
            rankingCurrent[i].text = null;
            ranking[i].text ="";
        }

        //�����L���O��ǂݍ���
        LordRanking();

    }

    /// <summary>
    /// ���X�e�[�W�̃����L���O������
    /// </summary>
    public void RankingUp()
    {
        if(currentStage==12)
        {//�X�e�[�W�P�Q�̏ꍇ
            currentStage -= 11;
        }
        else
        {
            currentStage++;
        }
        stage.text = string.Format("STAGE: {0}", currentStage);
        for (int i = 0; i < ranking.Length; i++)
        {//�O�̃����L���O�͏����Ă���
            rankingCurrent[i].text = null;
            ranking[i].text = "";
        }

        //�����L���O��ǂݍ���
        LordRanking();
    }

    /// <summary>
    /// �X�e�[�W�Z���N�g��
    /// </summary>
    public void StageSelect()
    {
        audioSource.PlayOneShot(select);
            Initiate.DoneFading();

            Initiate.Fade("StageSelect", Color.black, 1.0f);
    }

    /// <summary>
    /// �^�C�g����
    /// </summary>
    public void Title()
    {
        audioSource.PlayOneShot(cancel);
        Initiate.DoneFading();

        Initiate.Fade("Title", Color.black, 1.0f);
    }
}
