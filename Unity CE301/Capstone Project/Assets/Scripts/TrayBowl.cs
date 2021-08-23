using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrayBowl : MonoBehaviour {

    public GameObject tray;
    Vector3 offset;
    // Start is called before the first frame update
    void Start() {
        offset = transform.position - tray.transform.position;
    }

    // Update is called once per frame
    void Update() {
        transform.position = tray.transform.position + offset;
    }
}
