using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleStabiliser : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {

    }

    Vector3 z = Vector3.zero;
    // Update is called once per frame
    void Update() {
        transform.up = z;
    }
}
