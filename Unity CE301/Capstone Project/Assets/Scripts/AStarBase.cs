using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarBase : MonoBehaviour {
    public Player player;
    public GameCharacter gameCharacter;
    public GameObject markerr;
    public GameObject waypointMarkerIcon;
    public GameObject waypointMarkerIconLast;
    public Transform gameCharacterTransform, target;
    public List<Transform> checkpoints = new List<Transform>();
    public int sandCost, mudCost;

    public RockSpawner[] rockSpawners;
    public List<List<Node>> pathways = new List<List<Node>>();


    public List<Node> path = new List<Node>();

    public VariableGrid grid;
    public GameObject wayPointsParent;

    LayerMask mudMask;
    LayerMask sandMask;
    LayerMask rockSpawnerMask;
    LayerMask enemyMask;
    LayerMask blockedPathMask;

    private void Start() {
        //NEED TO ASSIGN THE TYPE OF CHARACTER IN EACH SUBCLASS
        //gameCharacter = GetComponent<GameCharacter>();
        //gameCharacterTransform = gameCharacter.transform;

        wayPointsParent = GameObject.FindGameObjectWithTag("Waypoints Parent");

        rockSpawners = FindObjectsOfType<RockSpawner>();
    }

    private void Awake() {
        //grid = GetComponent<VariableGrid>();
    }

    private void Update() {

        if (Input.GetKeyDown(KeyCode.Space)) {
            RecalculateFullPath();
        }
    }

    //this needs to be called in every class that extends this class
    public void SetLayerMasks() {
        sandMask = LayerMask.GetMask("Sand");
        mudMask = LayerMask.GetMask("Mud");
        rockSpawnerMask = LayerMask.GetMask("RockSpawner");
        enemyMask = LayerMask.GetMask("Enemy");
        blockedPathMask = LayerMask.GetMask("BlockedPath");
    }

    public void ClearPath() {
        print("Cleared Path");
        path.Clear();
        //grid.CreateGrid();
        grid.UpdateGrid();
        pathways.Clear();
        //PlaceSpheres();
        PlaceWaypointMarkerIcon();
    }

    public void RecalculateFullPath() {

        path = new List<Node>();
        //grid.CreateGrid();
        grid.UpdateGrid();

        CalcPath(gameCharacterTransform.localPosition, checkpoints[0].position);
        for (int i = 0; i < checkpoints.Count - 1; i++) {
            CalcPath(checkpoints[i].position, checkpoints[i + 1].position);
        }
        CalcPath(checkpoints[checkpoints.Count - 1].localPosition, target.position);
        //CalcPath(target.localPosition, gameCharacterTransform.position);


        //Debug.Log("Path count: " + path.Count);
        grid.path = path;
        gameCharacter.pathway = path;
        

        PlaceSpheres();
    }

    //call this just before you call CalcPath(), to reset the previous path and grid
    public void PrepareForNewSearch() {
        path = new List<Node>();
        //grid.CreateGrid();
        grid.UpdateGrid();
    }

    public bool FindPathToPlayer() {
        float timeAtCalc = Time.realtimeSinceStartup;
        //print("FindPathToPlayer() - " + gameCharacter.thisInst);
        PrepareForNewSearch();
        bool foundPath = CalcPath(gameCharacterTransform.localPosition, player.transform.position);
        //print($"[CalcPath - Test] Time taken to calc path: Seconds:{(Time.realtimeSinceStartup - timeAtCalc)}  Miliseconds: {(Time.realtimeSinceStartup - timeAtCalc) * 1000} - Time: {Time.realtimeSinceStartup} minus {timeAtCalc}");

        grid.path = path;
        gameCharacter.pathway = path;
        //PlaceWaypointMarkerIcon();

        return foundPath;
    }

    public void FindRockSpawner(bool showWaypointMarkers = true, bool simplify = false) {

        pathways.Clear();
        path.Clear();
        for (int i = 0; i < rockSpawners.Length; i++) {

            //if (rockSpawners[i].rockDetector.rocksDetected > 0) {
            if (rockSpawners[i].rocksSpawned.Count > 0) {
                path = new List<Node>();
                //grid.CreateGrid();
                grid.UpdateGrid();
                CalcPath(gameCharacterTransform.localPosition, rockSpawners[i].transform.position, simplify);
                path.Reverse();
                pathways.Add(path);
            }
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
        }
        //PlaceSpheres();
        if (showWaypointMarkers)
            PlaceWaypointMarkerIcon();
        //print($"Find Rock Spawner --- ok: {ok}: Pathways.Length: {pathways.Count}");
    }

    public void PlaceWaypointMarkerIcon() {
        //gets all NPCSpheres and puts them in a list
        GameObject[] waypointsArray = GameObject.FindGameObjectsWithTag("Waypoint Marker Icon");
        List<GameObject> waypointsList = new List<GameObject>();
        foreach (GameObject go in waypointsArray) {
            waypointsList.Add(go);
        }

        //adds spheres to match the amount in the path
        Quaternion q = gameCharacter.transform.rotation;
        int diff = path.Count - waypointsList.Count;
        if (diff > 0) {
            //Debug.Log("Diff: " + diff);
            for (int i = 0; i < diff; i++) {
                GameObject g = Instantiate(waypointMarkerIcon, gameCharacterTransform.transform.position, q, wayPointsParent.transform);

                waypointsList.Add(g);

            }

            // removes spheres to match the amount in the path
        } else if (diff != 0) {
            for (int i = waypointsList.Count - 1; i > path.Count - 1; i--) {
                Destroy(waypointsList[i]);
            }
        }
        if (waypointsList.Count > 0) {
            //Destroy(waypointsList[waypointsList.Count - 1]);
            //GameObject g = Instantiate(waypointMarkerIconLast, gameCharacterTransform.transform.position, q, wayPointsParent.transform);
            //waypointsList.Add(g);
        }

        // moves all spheres to their appropriate positions 
        for (int i = 0; i < path.Count; i++) {
            waypointsList[i].transform.position = path[i].worldPos + new Vector3(0, 2f, 0);
            waypointsList[i].GetComponent<WaypointMarker>().gameCharacter = gameCharacter;
            waypointsList[i].GetComponent<WaypointMarker>().target = waypointsList[0];
        }
    }

    public void PlaceSpheres() {
        //gets all NPCSpheres and puts them in a list
        GameObject[] sphh = GameObject.FindGameObjectsWithTag("NPCSphere");
        List<GameObject> sph = new List<GameObject>();
        foreach (GameObject go in sphh) {
            sph.Add(go);
        }

        //adds spheres to match the amount in the path
        Quaternion q = gameCharacter.transform.rotation;
        int diff = path.Count - sph.Count;
        if (diff > 0) {
            //Debug.Log("Diff: " + diff);
            for (int i = 0; i < diff; i++) {
                GameObject g = Instantiate(markerr, gameCharacterTransform.transform.position, q, wayPointsParent.transform);
                
                sph.Add(g);

            }

            // removes spheres to match the amount in the path
        } else if (diff != 0) {
            for (int i = sph.Count - 1; i > path.Count - 1; i--) {
                Destroy(sph[i]);
            }
        }

        // moves all spheres to their appropriate positions 
        for (int i = 0; i < path.Count; i++) {
            sph[i].transform.position = path[i].worldPos + new Vector3(0,2f,0);
            sph[i].GetComponent<WaypointMarker>().gameCharacter = gameCharacter;
            sph[i].GetComponent<WaypointMarker>().target = sph[0];
        }
    }

    //A* Search Algorithm
    public bool CalcPath(Vector3 startPos, Vector3 targetPos, bool simplify = false) {
        float timeAtCalc = Time.realtimeSinceStartup;
        Node startNode = grid.GetNodePos(startPos);
        Node targetNode = grid.GetNodePos(targetPos);

        //does't bother find a path if the target is on a blocked path
        if (!targetNode.traverseable) {
            print("Target on blocked path");
            return false;
        }

        //lists of the open and closed nodes
        List<Node> openSet = new List<Node>();
        //HashSet<Node> openSet = new HashSet<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        //iterates through all the nodes in the open set
        while (openSet.Count > 0) {
            Node currentNode = openSet[0];
            /*Node currentNode = startNode;

            foreach (Node n in openSet) {
                if (n.fCost < currentNode.fCost || n.fCost == currentNode.fCost && n.hCost < currentNode.hCost) {
                    currentNode = n;
                }
            } */

            
            //assigns the lowest costing node as the current node
            for (int i = 1; i < openSet.Count; i++) {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost) {
                    currentNode = openSet[i];
                }
            } 

            //updates open and closed sets
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            //exits the loop if the path finishes
            if (currentNode == targetNode) {
                if (simplify) {
                    RetracePathSimplified(startNode, targetNode);
                } else {
                    RetracePath(startNode, targetNode);
                }
                //print($"[CalcPath] Time taken to calc path: Seconds:{(Time.realtimeSinceStartup - timeAtCalc)}  Miliseconds: {(Time.realtimeSinceStartup - timeAtCalc) * 1000} - Time: {Time.realtimeSinceStartup} minus {timeAtCalc}");
                return true;
            }

            /*
            foreach (Node neighbour in grid.GetNeighbourNodes(currentNode)) {
                if (!neighbour.traverseable || closedSet.Contains(neighbour)) {
                    continue;
                }

                //adjusts the cost of the node based on mask
                int currentToNeighbourDist = GetDistance(currentNode, neighbour);
                if (neighbour.layerMask == sandMask) {
                    currentToNeighbourDist *= sandCost;
                    //print("SandMask");
                } else if (neighbour.layerMask == mudMask) {
                    currentToNeighbourDist *= mudCost;
                    //print("MudMask");
                } else if (neighbour.layerMask == rockSpawnerMask) {
                    currentToNeighbourDist += 20;
                    //print("RS Mask");
                } else if (neighbour.layerMask == enemyMask) {
                    currentToNeighbourDist += 20;
                    //print("EnemyMask");
                }
                
                int newMovementCostToNeighbour = currentNode.gCost + currentToNeighbourDist;
                //print($"NewMovementCostToNeighbour: {newMovementCostToNeighbour}");

                //updates the neighbour node costs if the new cost is shorter or is undefined
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    //makes sure the node is in the open set
                    if (!openSet.Contains(neighbour)) {
                        openSet.Add(neighbour);
                    }
                }
            }*/

            List<Node> neighbour = grid.GetNeighbourNodes(currentNode);
            //foreach (Node neighbour in grid.GetNeighbourNodes(currentNode)) {
            for (int i = 0; i < neighbour.Count; i++){
                if (!neighbour[i].traverseable || closedSet.Contains(neighbour[i])) {
                    continue;
                }

                //adjusts the cost of the node based on mask
                int currentToNeighbourDist = GetDistance(currentNode, neighbour[i]);
                if (neighbour[i].layerMask == sandMask) {
                    currentToNeighbourDist *= sandCost;
                    //print("SandMask");
                } else if (neighbour[i].layerMask == mudMask) {
                    currentToNeighbourDist *= mudCost;
                    //print("MudMask");
                } else if (neighbour[i].layerMask == rockSpawnerMask) {
                    currentToNeighbourDist += 20;
                    //print("RS Mask");
                } else if (neighbour[i].layerMask == enemyMask) {
                    currentToNeighbourDist += 20;
                    //print("EnemyMask");
                }

                int newMovementCostToNeighbour = currentNode.gCost + currentToNeighbourDist;
                //print($"NewMovementCostToNeighbour: {newMovementCostToNeighbour}");

                //updates the neighbour node costs if the new cost is shorter or is undefined
                if (newMovementCostToNeighbour < neighbour[i].gCost || !openSet.Contains(neighbour[i])) {
                    //neighbour[i].gCost = newMovementCostToNeighbour;
                    //neighbour[i].hCost = GetDistance(neighbour[i], targetNode);
                    neighbour[i].SetCosts(newMovementCostToNeighbour, GetDistance(neighbour[i], targetNode));
                    neighbour[i].parent = currentNode;

                    //makes sure the node is in the open set
                    if (!openSet.Contains(neighbour[i])) {
                        openSet.Add(neighbour[i]);
                    }
                }
            }
            //print($"OpenSet: {openSet.Count} - ClosedSet: {closedSet.Count}");
        }
        print($"Failed to find path - [CalcPath - after While] Time taken to calc path: Seconds:{(Time.realtimeSinceStartup - timeAtCalc)}  Miliseconds: {(Time.realtimeSinceStartup - timeAtCalc) * 1000} - Time: {Time.realtimeSinceStartup} minus {timeAtCalc}");

        return false;
    }

    //lists all the nodes in a path and sets the grid path to it
    public void RetracePath(Node startNode, Node endNode) {
        List<Node> tempPath = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode) {
            tempPath.Add(currentNode);
            currentNode = currentNode.parent;
        }

        tempPath.Reverse();
        //List<Node> simplifiedPath = SimplifyPath(tempPath);
        //path.AddRange(simplifiedPath);
        path.AddRange(tempPath);
    }

    public void RetracePathSimplified(Node startNode, Node endNode) {
        List<Node> tempPath = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode) {
            tempPath.Add(currentNode);
            currentNode = currentNode.parent;
        }

        tempPath.Reverse();
        List<Node> simplifiedPath = SimplifyPath(tempPath);
        path.AddRange(simplifiedPath);
        //path.AddRange(tempPath);
    }

    List<Node> SimplifyPath(List<Node> path) {
        List<Node> simplifiedPath = new List<Node>();
        Vector2 oldDirection = Vector2.zero;

        for (int i = 1; i < path.Count; i++) {
            Vector2 newDirection = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if (oldDirection != newDirection) {
                simplifiedPath.Add(path[i]);
            }
            oldDirection = newDirection;
        }
        if (!simplifiedPath.Contains(path[path.Count - 1]))
            simplifiedPath.Add(path[path.Count - 1]);

        return simplifiedPath;
    }

    //calculates the distance between two nodes
    public int GetDistance(Node nodeA, Node nodeB) {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (distX > distY) {
            return (14 * distY) + (10 * (distX - distY));
        } else {
            return (14 * distX) + (10 * (distY - distX));
        }
    }
}
