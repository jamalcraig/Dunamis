using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnersManager : MonoBehaviour {

    Bindings bindings;

    public RockSpawner[] rockSpawners;
    public float rockSpawnInterval = 3f;
    public float rockTimeTilSpawn;

    public EnemySpawner[] enemySpawners;
    public float enemySpawnInterval = 3f;
    public float enemyTimeTilSpawn;
    public bool canSpawnEnemies = true;
    public int maxEnemies;
    public GameObject enemy;
    public List<GameObject> enemiesSpawned = new List<GameObject>();

    // Start is called before the first frame update
    void Start() {

        bindings = FindObjectOfType<Bindings>();
        rockSpawners = FindObjectsOfType<RockSpawner>();
        enemySpawners = FindObjectsOfType<EnemySpawner>();

        rockSpawners[Random.Range(0, rockSpawners.Length)].SpawnRock();
        enemySpawners[Random.Range(0, enemySpawners.Length)].SpawnEnemy();
    }

    // Update is called once per frame
    void Update() {
        if (bindings.CIRCLEBUT)
            canSpawnEnemies = !canSpawnEnemies;

        rockTimeTilSpawn -= Time.deltaTime;
        if (rockTimeTilSpawn <= 0) {
            //for (int i = 0; i < spawners.Length; i++) {
            // spawners[i].SpawnRock();
            //}
            rockSpawners[Random.Range(0, rockSpawners.Length)].SpawnRock();
            rockTimeTilSpawn = rockSpawnInterval;
        }

        if (canSpawnEnemies)
            SpawnEnemy();
    }

    void SpawnEnemy() {

        enemyTimeTilSpawn -= Time.deltaTime;
        if (enemyTimeTilSpawn <= 0) {

            if (enemiesSpawned.Count < maxEnemies) {
                //enemySpawners[Random.Range(0, enemySpawners.Length)].SpawnEnemy();
                int ran = Random.Range(0, enemySpawners.Length);
                enemyTimeTilSpawn = enemySpawnInterval;
                GameObject g = Instantiate(enemy, Vector3.zero, enemySpawners[ran].transform.rotation, enemySpawners[ran].transform);
                g.GetComponentInChildren<Racer>().currentState = Racer.State.Chasing;
                g.GetComponentInChildren<Racer>().gameObject.transform.position = enemySpawners[ran].transform.position;
                enemiesSpawned.Add(g);

            }
        }
    }

    public void RemoveEnemy(GameObject g) {
        enemiesSpawned.Remove(g);
        //print($"Removed {g.name} - Rocks Left: {rocksSpawned.Count}");
        Destroy(g);
    }
}
