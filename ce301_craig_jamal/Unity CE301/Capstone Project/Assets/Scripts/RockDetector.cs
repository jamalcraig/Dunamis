using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockDetector : MonoBehaviour {

    public int rocksDetected {
        get { return colliders.Count; }
    }
    public bool hasRocks;
   // List<Collider> colliders = new List<Collider>();
    public List<GameObject> colliders = new List<GameObject>();
    // Start is called before the first frame update
    void Start() {
        //rocksDetected = 0;
    }

    // Update is called once per frame
    void Update() {

        if (colliders.Count > 0) {
            hasRocks = true;
        } else {
            hasRocks = false;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.GetComponent<RockCube>()) {
            //colliders.Add(other.gameObject.GetComponent<Collider>());
            colliders.Add(other.gameObject);
        }
    }

   

}
