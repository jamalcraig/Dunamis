using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour {

    public GameObject rock;
    public List<Transform> rockSpawnPos;
    public RockDetector rockDetector;
    public List<GameObject> rocksSpawned = new List<GameObject>();
    // Start is called before the first frame update
    void Start() {

        Transform[] a = gameObject.GetComponentsInChildren<Transform>();
        for (int i = 0; i < a.Length; i++) {
            if (a[i].gameObject.tag == "Spawn Pos") {
                rockSpawnPos.Add(a[i].transform);
            }
        }
        rockDetector = gameObject.GetComponentInChildren<RockDetector>();
    }

    // Update is called once per frame
    void Update() {

    }

    public void SpawnRock() {
        //print($"Spawn rock triggered for {gameObject.name}. It has {rockDetector.rocksDetected} rocks detected");
        if (rocksSpawned.Count < 6) {
            int randomPos = Random.Range(0, rockSpawnPos.Count - 1);
            rocksSpawned.Add(Instantiate(rock, rockSpawnPos[randomPos].position, rockSpawnPos[randomPos].rotation, gameObject.transform));
        }
    }

    public void RemoveRock(GameObject g) {
        rocksSpawned.Remove(g);
        //print($"Removed {g.name} - Rocks Left: {rocksSpawned.Count}");
        Destroy(g);
    }
}
