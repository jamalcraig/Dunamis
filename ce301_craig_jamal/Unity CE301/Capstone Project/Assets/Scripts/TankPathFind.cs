using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPathFind : AStarBase {

    public GameObject d;

    // Start is called before the first frame update
    void Start() {
        //gameCharacter = GetComponent<Tank>();
        //gameCharacterTransform = gameCharacter.transform;
        markerr = d;
        wayPointsParent = GameObject.FindGameObjectWithTag("Waypoints Parent");

        rockSpawners = FindObjectsOfType<RockSpawner>();
        SetLayerMasks();
    }

    // Update is called once per frame
    void Update() {

        if (gameCharacter.findPath)
            RecalculateFullPath();

        if (gameCharacter.findPathToRockSpawner)
            FindRockSpawner(true, false);

        if (gameCharacter.clearPath) {
            ClearPath();
            gameCharacter.clearPath = false;
        }
    }
}
