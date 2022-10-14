using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFireflies : MonoBehaviour
{
    // This is the code for spawning fireflies
    // By default it will spawn a firefly when you hit Space
    // To generate a fixed number of fireflies instead, comment out Update function
    // and uncommment the code in Start
    public GameObject fireflyPrefab; 
    public Camera MainCamera;
    public int spawnNum;  // number of desired fireflies (can be edited in Unity window in FireflySpawner object)
    
    void Start()
    {
        //StartCoroutine(AutoSpawn());
    }

    void Update() {
        // Spawn a firefly when you hit Space

        if (Input.GetButtonDown("Jump")) {
            float xPos = Random.Range(-2.5f, 2.5f);
            float yPos = Random.Range(0f, 3f);
            float zPos = Random.Range(-2.5f, 2.5f);
            Vector3 pos = new Vector3(xPos, yPos, zPos);

            Instantiate(fireflyPrefab, pos, Quaternion.identity);
        }
    }

    // Spawn a specified number of fireflies
    IEnumerator AutoSpawn() {
        for (int i = 0; i < spawnNum; i++) {
            float xPos = Random.Range(-2.5f, 2.5f);
            float yPos = Random.Range(0f, 3f);
            float zPos = Random.Range(-2.5f, 2.5f);
            Vector3 pos = new Vector3(xPos, yPos, zPos);

            Instantiate(fireflyPrefab, pos, Quaternion.identity);

            int count = i + 1;
            yield return new WaitForSeconds(1f);
        }
    }

}
