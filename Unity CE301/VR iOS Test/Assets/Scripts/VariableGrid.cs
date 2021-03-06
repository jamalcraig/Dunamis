using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableGrid : MonoBehaviour {
	// Start is called before the first frame update
	public Transform npcPos;
	public Player playerClass;
	//public NPCPathfind npcPathFindClass;
	public TankPathFind npcPathFindClass;
	public LayerMask blockedPathMask;
	public Vector2 gridWorldSize;
	public float nodeRadius {
        get {
			//return Mathf.Max(playerClass.transform.localScale.x, playerClass.transform.localScale.z);
			return nodeDiameter / 2;
		}
    }
	public List<LayerMask> layerMaskList;
	Node[,] grid;

	float nodeDiameter {
		get {
			//return nodeRadius * 2;
			return Mathf.Max(playerClass.transform.localScale.x, playerClass.transform.localScale.z);
		}
    }
	int gridSizeX, gridSizeY;

	private void Start() {
		//uses the values set in the editor to define the size of the grid
		//npcPos = FindObjectOfType<BasicNPC>().transform;
		//nodeDiameter = nodeRadius * 2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
		Debug.Log("Grid size: " + gridSizeX + " " + gridSizeY);
		CreateGrid();
		Debug.Log("Grid Len: " + grid.Length);
	}

	//creates the grid that will be used for the A* search
	public void CreateGrid() {
		grid = new Node[gridSizeX, gridSizeY];
		Vector3 worldBottomLeftCornerPos = transform.position - (new Vector3(1, 0, 0) * gridWorldSize.x / 2) - (new Vector3(0, 0, 1) * gridWorldSize.y / 2);

		for (int x = 0; x < gridSizeX; x++) {
			for (int y = 0; y < gridSizeY; y++) {
				Vector3 worldPoint = worldBottomLeftCornerPos + new Vector3(1, 0, 0) * (x * nodeDiameter + nodeRadius) + new Vector3(0, 0, 1) * (y * nodeDiameter + nodeRadius);
				bool traverseable = !(Physics.CheckSphere(worldPoint, nodeRadius, blockedPathMask));
				Node node = new Node(traverseable, LayerMask.GetMask("Default"), worldPoint, x, y);
				foreach (LayerMask lm in layerMaskList) {
					if ((Physics.CheckSphere(worldPoint, nodeRadius, lm))) {
						node.layerMask = lm;
						break;

					}
				}

				grid[x, y] = node;
			}

		}
	}

	//returns a list of the neighbour nodes a node has
	public List<Node> GetNeighbourNodes(Node node) {
		List<Node> neighbours = new List<Node>();

		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if (x == 0 && y == 0) {
					continue;
				}

				int checkX = node.gridX + x;
				int checkY = node.gridY + y;

				if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {
					//Debug.Log("Grid checkXY " + checkX + " " + checkY);
					neighbours.Add(grid[checkX, checkY]);
				}
			}
		}

		return neighbours;
	}

	//returns the grid position of a node
	public Node GetNodePos(Vector3 worldPos) {
		float percentX = Mathf.Clamp01((worldPos.x + gridWorldSize.x / 2) / gridWorldSize.x);
		float percentY = Mathf.Clamp01((worldPos.z + gridWorldSize.y / 2) / gridWorldSize.y);

		int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
		if (x < 0) { x = 0; }
		if (y < 0) { y = 0; }
		//Debug.Log("XY: " + x + " " + y);
		//Debug.Log("Grid XY: " + grid[x, y].gridX + " " + grid[x, y].gridY);
		return grid[x, y];
	}



	public List<Node> path;

    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid != null) {
            Node playerNode = GetNodePos(playerClass.transform.localPosition);
            //Node npcNode = GetNodePos(npcPos.position);
            Node npcNode = GetNodePos(npcPos.localPosition);
            foreach (Node n in grid) {
                if (n.traverseable) {
                    Gizmos.color = Color.white;
                } else {
                    Gizmos.color = Color.red;
                }

                if (n.layerMask == LayerMask.GetMask("Sand")) {
                    Gizmos.color = Color.yellow;
                } else if (n.layerMask == LayerMask.GetMask("Mud")) {
                    Gizmos.color = Color.blue;
                }

                if (n == playerNode) {
                    Gizmos.color = Color.cyan;
                }
				if (n == npcNode) {
					Color ccc = new Color(254, 40, 20);
					Gizmos.color = Color.blue;
				}

				if (npcPathFindClass.path.Count > 0) {
                    if (npcPathFindClass.path.Contains(n)) {
                        Gizmos.color = Color.black;
                    }
                }

                //Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiameter - .1f));
                Gizmos.DrawCube(n.worldPos, new Vector3(nodeDiameter - .1f, -1, nodeDiameter - .1f));
            }
        }
    }

    private void Update() {
//		Debug.Log("Node Diameter: " + nodeDiameter);
		if (Input.GetKeyDown(KeyCode.G)) {
			CreateGrid();
        }
    }
}
