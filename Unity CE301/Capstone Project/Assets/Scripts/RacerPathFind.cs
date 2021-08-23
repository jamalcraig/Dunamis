using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerPathFind : AStarBase {

    Exit[] exits;
    public GameObject wpp;
    // Start is called before the first frame update
    void Start() {
        wayPointsParent = GameObject.FindGameObjectWithTag("Waypoints Parent");
        player = FindObjectOfType<Player>();
        exits = FindObjectsOfType<Exit>();
        SetLayerMasks();
    }

    // Update is called once per frame
    public float timeAtCalc;
    void Update() {
        
        if (gameCharacter.findPathToPlayer) {
            timeAtCalc = Time.realtimeSinceStartup;
            //bool f = FindPathToPlayer();
            if (!FindPathToPlayer()) {
                gameCharacter.GetComponent<Racer>().MoveWithLOS();
                print($"FindPathToPlayer can't find a path; so MoveWithLOS");
            }
            //print($"Time taken to calc path: ({(Time.realtimeSinceStartup - timeAtCalc)}s)  ({(Time.realtimeSinceStartup - timeAtCalc) * 1000}ms) - (Time: {Time.realtimeSinceStartup}s minus {timeAtCalc}s)");

        }
        if (gameCharacter.GetComponent<Racer>().findPathToExit) {
            FindExit();
        }
    }

    public void FindExit() {
        //print("FindExit() - " + gameCharacter.thisInst);
        PrepareForNewSearch();

        for (int i = 0; i < exits.Length; i++) {
            path = new List<Node>();
            //grid.CreateGrid();
            grid.UpdateGrid();
            CalcPath(gameCharacterTransform.localPosition, exits[i].transform.position);
            pathways.Add(path);
        }

        if (pathways.Count > 0) {
            List<Node> shortestPath = pathways[0];
            int ok = 0;
            //print("Initial Shortest path count" + shortestPath.Count);
            for (int i = 0; i < pathways.Count; i++) {
                if (pathways[i].Count < shortestPath.Count) {

                    shortestPath = pathways[i];
                    ok = i;
                }
            }

            //print("Shortest path count after being set" + shortestPath.Count);
            path = pathways[ok];
            grid.path = pathways[ok];
            gameCharacter.pathway = pathways[ok];

            //print($"Racer({gameCharacter.thisInst}) Path Length: {path.Count}");
            //PlaceMarkers();
        }

    }

    public List<GameObject> waypointsList = new List<GameObject>();
    public void PlaceMarkers() {
        //gets all NPCSpheres and puts them in a list
        //GameObject[] waypointsArray = this.gameObject.transform.GetChild();
        waypointsList = new List<GameObject>();
        for (int i = 0; i < wpp.transform.childCount; i++) {
            waypointsList.Add(wpp.transform.GetChild(i).gameObject);
        }
        //waypointsList.Reverse();
      

        //adds spheres to match the amount in the path
        Quaternion q = gameCharacter.transform.rotation;
        int diff = path.Count - waypointsList.Count;
        if (diff > 0) {
            Debug.Log("Diff: " + diff);
            for (int i = 0; i < diff; i++) {
                GameObject g = Instantiate(waypointMarkerIcon, gameCharacterTransform.transform.position, q, wpp.transform);
                
                waypointsList.Add(g);

            }

            // removes spheres to match the amount in the path
        } else if (diff != 0) {
            for (int i = waypointsList.Count - 1; i > path.Count - 1; i--) {
                print($"Destroyed {i} {waypointsList[i].name}");
                Destroy(waypointsList[i]);
            }
        }
        if (waypointsList.Count > 0) {
            //Destroy(waypointsList[waypointsList.Count - 1]);
            //GameObject g = Instantiate(waypointMarkerIconLast, gameCharacterTransform.transform.position, q, wayPointsParent.transform);
            //waypointsList.Add(g);
        }

        waypointsList.Reverse();
        // moves all spheres to their appropriate positions 
        for (int i = 0; i < path.Count; i++) {
            waypointsList[i].transform.position = path[i].worldPos + new Vector3(0, 2f, 0);
            waypointsList[i].GetComponent<WaypointMarker>().gameCharacter = gameCharacter;
            waypointsList[i].GetComponent<WaypointMarker>().target = waypointsList[waypointsList.Count-1];
        }
    }
}
