using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeDispenser : MonoBehaviour {

    public GameObject gem;
    public Transform spawnPos;
    public float spawnInterval;
    public float timeTilSpawn;
    
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        timeTilSpawn -= Time.deltaTime;
        if (timeTilSpawn <= 0) {
            SpawnGem();
            timeTilSpawn = spawnInterval;
        }
    }

    public void SpawnGem() {
        Instantiate(gem, spawnPos.position, spawnPos.rotation, gameObject.transform);
    }
}
