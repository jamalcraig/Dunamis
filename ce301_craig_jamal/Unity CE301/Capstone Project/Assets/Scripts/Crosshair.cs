using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour {

    public Camera cam;
    public Vector3 offSetFromCam = new Vector3(0.5F, 0.5F, 0.8f);
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        transform.position = cam.ViewportToWorldPoint(offSetFromCam);
        
    }

    private void LateUpdate() {
        transform.forward = Camera.main.transform.forward;
    }
}
