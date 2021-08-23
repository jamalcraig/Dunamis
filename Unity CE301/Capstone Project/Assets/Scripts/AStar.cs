using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour {

    public Player player;
    public GameObject dot;
    public Transform playerTransform, target;
    public List<List<Transform>> checkpoints = new List<List<Transform>>();
    public List<Transform> checkpointPositions;
    public List<Transform> checkpointPositions2;
    public List<Transform> checkpointPositions3;
    public List<Transform> checkpointPositions4;
    public List<List<Node>> pathways = new List<List<Node>>();
    public int sandCost, mudCost;

    List<Node> path = new List<Node>();

    Grid grid;

    private void Awake() {
        grid = GetComponent<Grid>();
    }

    private void Update() {
        if (player.findPath) {
            checkpoints.Add(checkpointPositions);
            checkpoints.Add(checkpointPositions2);
            checkpoints.Add(checkpointPositions3);
            checkpoints.Add(checkpointPositions4);
            pathways.Clear();
            for (int index = 0; index < 4; index++) {


                path = new List<Node>();
                grid.CreateGrid();
                
                //CalcPath(playerTransform.position, target.position);
                CalcPath(playerTransform.position, checkpoints[index][0].position);
                for (int i = 0; i < checkpoints[index].Count - 1; i++) {
                    CalcPath(checkpoints[index][i].position, checkpoints[index][i + 1].position);
                }
                CalcPath(checkpoints[index][checkpoints[index].Count - 1].position, target.position);
                path.Reverse();
                pathways.Add(path);
                //Debug.Log("index: " + index + " - path count: " + path.Count);
                //Debug.Log("---");
                //if (grid.path == null) {
                //    grid.path = path;
                //}

                //Debug.Log("Grid count: " + grid.path.Count + " Path Count: " + path.Count);
                //if (grid.path.Count > path.Count) {
                //    grid.path = path;
                //}
            }
            List<Node> shortestPath = pathways[2];
            int ok = 2;
            //Debug.Log("Shortest (only) Path: " + shortestPath.Count);
            
            //foreach (List<Node> p in pathways) {
            //    Debug.Log("p.Count: " + p.Count + " path.Count: " + path.Count + " shortest path: " + shortestPath.Count);
            //    if (p.Count < path.Count) {
            //        shortestPath = path;
            //    }
            //}

            for (int i = 0; i < pathways.Count; i++) {
                //Debug.Log("pathways[" + i + "].Count: " + pathways[i].Count + " shortestPath.Count: " + shortestPath.Count);
                if (pathways[i].Count < shortestPath.Count) {

                    //Debug.Log("SP Was: " + shortestPath.Count);
                    shortestPath = pathways[i];
                    ok = i;
                    //Debug.Log("Shortest path is now (["+i+"])" + shortestPath.Count + " " + pathways[i].Count);
                }
            }
            //Debug.Log("SP [" + ok + "] : " + shortestPath.Count + " " + pathways[ok].Count);
            
            grid.path = pathways[ok];
            Quaternion q = player.transform.rotation;
            GameObject[] spheres = GameObject.FindGameObjectsWithTag("Sphere");
            foreach (GameObject g in spheres) {
                Destroy(g);
            }
            foreach (Node n in pathways[ok]) {

                Instantiate(dot, n.worldPos, q);
            }
            //Debug.Log("----------------------");

            
        }
        
    }

    //A* Search Algorithm
    void CalcPath(Vector3 startPos, Vector3 targetPos) {
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
    void RetracePath(Node startNode, Node endNode) {
        //List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        //path.Reverse();

        //grid.path = path;
    }

    //calculates the distance between two nodes
    int GetDistance(Node nodeA, Node nodeB) {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (distX > distY) {
            return (14 * distY) + (10 * (distX - distY));
        } else {
            return (14 * distX) + (10 * (distY - distX));
        }
    }
}
