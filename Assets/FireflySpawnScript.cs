using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflySpawnScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject firefly;

    public int spawnNumber = 100;

    void Start()
    {
        for(int i = 0; i < spawnNumber; i++){
            GameObject newFirefly = Instantiate(firefly);
            newFirefly.transform.position = new Vector3(Random.Range(-20, 20),Random.Range(-20, 20),Random.Range(-20, 20));
            newFirefly.transform.parent = this.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
