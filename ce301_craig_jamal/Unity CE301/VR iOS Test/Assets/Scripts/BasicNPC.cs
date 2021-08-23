using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicNPC : GameCharacter {

    //public List<Transform> checkpoints = new List<Transform>();



    //  void FixedUpdate() {

    //      if (current > pathway.Count) {
    //	current = 0;
    //}
    //if (pathway.Count > 0 && Vector3.Distance(pathway[current].worldPos, transform.position) < rad) {
    //	current++;
    //	if (current >= pathway.Count) {
    //		current = 0;
    //	}
    //}


    //if (pathway.Count > 0)
    //	transform.position = Vector3.MoveTowards(transform.position, pathway[current].worldPos, Time.deltaTime * moveSpeed);
    //  }

    private void Update() {
        UpdateCheckpoint();
    }
}
