////////////////////////////////////////////////////////////////
///
/// �G�̎����������Ǘ�����X�N���v�g
/// 
/// Aughter:�ؓc�W��
///
////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// �G�̃v���n�u�ɃA�^�b�`����N���X
/// </summary>
public class EnemySpawn : MonoBehaviour
{
    //Unity����擾
    [SerializeField] GameObject Enemy; //�G�̃I�u�W�F�N�g�擾

    // �N��������1�񂾂����s
    void Start()
    {
        //�G�����Ԋu��������悤�ɐݒ�
        InvokeRepeating("Spawn", 3.0f,3.0f);
    }

    /// <summary>
    /// �G�̐���
    /// </summary>
    private void Spawn()
    {
        //�G�����̃X�N���v�g�ɕt�����ꏊ���琶��
        Instantiate(Enemy, transform.position,Quaternion.identity);
    }
}
