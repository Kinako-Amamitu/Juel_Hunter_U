using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] GameGenerator gameGenerator;

    private Rigidbody2D rb;     // �R���|�[�l���g�̎擾�p
  

    private Vector3 lookDirection = new Vector3(0, -1.0f,0);      // �L�����̌����̏��̐ݒ�p

    [SerializeField] List<Bullet> juelPrefabs; //����Ɏg���p�̃W���G���v���n�u

    int juelRnd; //�W���G���̃����_����ID

    Bullet nextBullet; //���̃o���b�g


    private void Start()
    {

        TryGetComponent(out rb);

    }

    private void Update()
    {
        /* �}�E�X�N���b�N�����m */
        if (Input.GetMouseButtonDown(0))
        {

            // �^�b�v�����ꏊ����Ray���쐬
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Raycast���쐬
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

            //Ray�������ɏՓ˂������Ƃ����m & �Փ˂����Ώۂ��������g���𔻕�
            if (hit2d && hit2d.transform.gameObject.tag == "Player")
            {
                gameGenerator.FireBullet();



            }

        }
    }

    private void FixedUpdate()
    {
        
    }
    /// <summary>
    /// �v���C���[�̐i�s�����̎擾�p
    /// </summary>
    /// <returns></returns>
    public Vector3 GetLookDirection()
    {
        Vector3 direction = new Vector3(-Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad), Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad));
        return direction;
    }

    public void SiteJuel(int rnd)
    {
        // �\���ʒu
        Vector3 setupPosition = new Vector3(-2.5f, 0, 0);

        // �u���b�N����
        Bullet prefab = juelPrefabs[rnd];
        Instantiate(prefab, setupPosition, Quaternion.identity);

    }
}
