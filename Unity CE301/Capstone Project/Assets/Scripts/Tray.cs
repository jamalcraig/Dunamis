using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tray : MonoBehaviour {

    public Material m;
    public Material grabbedMat;
    public Renderer ren;
    // Start is called before the first frame update
    void Start() {
        ren = GetComponent<Renderer>();
        m = ren.material;
    }

    // Update is called once per frame
    void Update() {

    }

    public void SetToGrabbedMat() {
        ren.material = grabbedMat;
    }

    public void SetToUngrabbedMat() {
        ren.material = m;
    }
}
