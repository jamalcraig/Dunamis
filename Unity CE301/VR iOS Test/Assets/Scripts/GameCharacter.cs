using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCharacter : MonoBehaviour {

    public float moveSpeed;
    public float rotateSpeed;
    public bool findPath;
    public List<Node> pathway = new List<Node>();
    public int current = 0;
    public int rad = 5;

    
    void Start() {

    }


    void Update() {
        //UpdateCheckpoint();
    }

    public void UpdateCheckpoint() {

        if (current > pathway.Count) {
            current = 0;
        }
        if (pathway.Count > 0 && Vector3.Distance(pathway[current].worldPos, transform.position) < rad) {
            current++;
            if (current >= pathway.Count) {
                current = 0;
            }
        }


        if (pathway.Count > 0)
            transform.position = Vector3.MoveTowards(transform.position, pathway[current].worldPos, Time.deltaTime * moveSpeed);
    }
}
