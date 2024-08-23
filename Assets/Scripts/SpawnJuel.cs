using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnJuel : MonoBehaviour
{
    [SerializeField] List<GameObject> juelPrefabs;
    [SerializeField] int juelLayer;
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
            // Fƒ‰ƒ“ƒ_ƒ€
            int rnd = Random.Range(0, juelPrefabs.Count);
            // ’e‚Ì¶¬
            Instantiate(juelPrefabs[rnd], transform.position+Vector3.right*0.005f, Quaternion.identity);
        }
    }
}
