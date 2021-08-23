using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPathFind : AStarBase {

    public GameObject dot;

    // Start is called before the first frame update
    void Start() {
        //gameCharacter = GetComponent<Tank>();
        //gameCharacterTransform = gameCharacter.transform;
        markerr = dot;
    }

    // Update is called once per frame
    void Update() {

        if (gameCharacter.findPath)
        RecalculateFullPath();
    }
}
