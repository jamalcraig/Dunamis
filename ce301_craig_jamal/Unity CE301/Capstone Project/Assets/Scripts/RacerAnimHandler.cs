using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerAnimHandler : MonoBehaviour {

    public Racer r;
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {

    }

    public void SetToEscape() {
        r.SetToEscape();
        if (r.spawnedHealthOrb == null)
            r.SpawnHealthOrb();
        
    }

}
