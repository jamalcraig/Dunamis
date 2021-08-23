using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanguardPathFind : AStarBase {

   // GameObject dot;
    // Start is called before the first frame update
    void Start() {
        //markerr = dot;
    }

    // Update is called once per frame
    void Update() {
        if (gameCharacter.findPath)
            RecalculateFullPath();
    }
}
