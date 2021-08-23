using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClonePathFind : AStarBase {
    // Start is called before the first frame update
    void Start() {

        wayPointsParent = GameObject.FindGameObjectWithTag("Waypoints Parent");

        rockSpawners = FindObjectsOfType<RockSpawner>();
        player = FindObjectOfType<Player>();
        SetLayerMasks();
    }

    // Update is called once per frame
    void Update() {
        if (gameCharacter.findPathToRockSpawner) {
            FindRockSpawner(false);
            gameCharacter.pathway.Reverse();
        }

        if (gameCharacter.findPathToPlayer){
            FindPathToPlayer();
        }
    }
}
