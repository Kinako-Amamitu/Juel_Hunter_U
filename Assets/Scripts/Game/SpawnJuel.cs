using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnJuel : MonoBehaviour
{
    [SerializeField] List<GameObject> juelPrefabs;
    [SerializeField] int juelLayer;

    int rnd1=-1;
    int rnd2=-1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit2D hitObj= Physics2D.Raycast(transform.position, Vector2.right,0.3f, 1<<juelLayer);

        if (!hitObj)
        {
            // �F�����_��
            int rnd = Random.Range(0, juelPrefabs.Count);

            while (true)
            {
                if (rnd1 == rnd2 && rnd == rnd1)
                {
                    // �F�����_��
                    rnd = Random.Range(0, juelPrefabs.Count);
                }
                else
                {
                    break;
                }
            }
            rnd2 = rnd1;
            rnd1 = rnd;


            // �e�̐���
            Instantiate(juelPrefabs[rnd], transform.position+Vector3.right*0.005f, Quaternion.identity);
        }
    }
}
