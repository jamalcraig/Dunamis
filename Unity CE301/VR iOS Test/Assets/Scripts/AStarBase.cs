using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarBase : MonoBehaviour {
    public GameCharacter gameCharacter;
    public GameObject markerr;
    public Transform gameCharacterTransform, target;
    public List<Transform> checkpoints = new List<Transform>();
    public int sandCost, mudCost;

    public List<Node> path = new List<Node>();

    public VariableGrid grid;
    public GameObject wayPointsParent;

    private void Start() {
        //NEED TO ASSIGN THE TYPE OF CHARACTER IN EACH SUBCLASS
        //gameCharacter = GetComponent<GameCharacter>();
        //gameCharacterTransform = gameCharacter.transform;

        wayPointsParent = GameObject.FindGameObjectWithTag("Waypoints Parent");
    }

    private void Awake() {
        //grid = GetComponent<VariableGrid>();
    }

    private void Update() {

        if (Input.GetKeyDown(KeyCode.Space)) {
            RecalculateFullPath();
        }
    }

    public void RecalculateFullPath() {

        path = new List<Node>();
        grid.CreateGrid();

        CalcPath(gameCharacterTransform.localPosition, checkpoints[0].position);
        for (int i = 0; i < checkpoints.Count - 1; i++) {
            CalcPath(checkpoints[i].position, checkpoints[i + 1].position);
        }
        CalcPath(checkpoints[checkpoints.Count - 1].localPosition, target.position);
        //CalcPath(target.localPosition, gameCharacterTransform.position);


        Debug.Log("Path count: " + path.Count);
        grid.path = path;
        gameCharacter.pathway = path;
        

        PlaceSpheres();
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
            Debug.Log("Diff: " + diff);
            for (int i = 0; i < diff; i++) {
                GameObject g = Instantiate(markerr, gameCharacterTransform.transform.position, q/*, wayPointsParent.transform*/);
                sph.Add(g);

            }

            // removes spheres to match the amount in the path
        } else if (diff != 0) {
            for (int i = sph.Count - 1; i > path.Count - 1; i--) {
                Destroy(sph[i]);
            }
        }

        // moves all spheres to their appropriate positions 
        for (int i = 0; i < path.Count - 1; i++) {
            sph[i].transform.position = path[i].worldPos;
        }
    }

    //A* Search Algorithm
    public void CalcPath(Vector3 startPos, Vector3 targetPos) {
        Node startNode = grid.GetNodePos(startPos);
        Node targetNode = grid.GetNodePos(targetPos);

        //lists of the open and closed nodes
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        //iterates through all the nodes in the open set
        while (openSet.Count > 0) {
            Node currentNode = openSet[0];

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
                RetracePath(startNode, targetNode);
                return;
            }


            foreach (Node neighbour in grid.GetNeighbourNodes(currentNode)) {
                if (!neighbour.traverseable || closedSet.Contains(neighbour)) {
                    continue;
                }

                //adjusts the cost of the node based on mask
                int currentToNeighbourDist = GetDistance(currentNode, neighbour);
                if (neighbour.layerMask == LayerMask.GetMask("Sand")) {
                    currentToNeighbourDist *= sandCost;
                } else if (neighbour.layerMask == LayerMask.GetMask("Mud")) {
                    currentToNeighbourDist *= mudCost;
                }
                int newMovementCostToNeighbour = currentNode.gCost + currentToNeighbourDist;

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
            }
        }
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
