using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

	public bool traverseable;
	public LayerMask layerMask;
	public Vector3 worldPos;
	public int gridX;
	public int gridY;

	public Node parent;

	public int gCost;
	public int hCost;
	public int fCost {
		get {
			return gCost + hCost;
		}
	}

	public Node (bool traverseable, LayerMask layerMask, Vector3 worldPos, int gridX, int gridY) {
		this.traverseable = traverseable;
		this.worldPos = worldPos;
		this.gridX = gridX;
		this.gridY = gridY;
	}

	
}
