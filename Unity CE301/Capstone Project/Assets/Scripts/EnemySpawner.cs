using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    SpawnersManager spawnersManager;
    public Transform enemySpawnPos;
    public int maxEnemies;
    // Start is called before the first frame update
    void Start() {
        enemySpawnPos = this.transform;
        spawnersManager = FindObjectOfType<SpawnersManager>();
    }

    // Update is called once per frame
    void Update() {

    }

    public void SpawnEnemy() {
        //print($"Spawn rock triggered for {gameObject.name}. It has {rockDetector.rocksDetected} rocks detected");
        
            
            
        
    }

    public void RemoveEnemy(GameObject g) {
        //enemiesSpawned.Remove(g);
        //print($"Removed {g.name} - Rocks Left: {rocksSpawned.Count}");
        //Destroy(g);
        spawnersManager.RemoveEnemy(g);
    }
}
