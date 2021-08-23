using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

	public static int inst = 0;
	public int thisInst;

	public bool traverseable;
	public LayerMask layerMask;
	public Vector3 worldPos;
	public int gridX;
	public int gridY;

	public Node parent;

	public int gCost;
	public int hCost;
	/*public int fCost {
		get {
			return gCost + hCost;
		}
	} */
	public int fCost;

	public Node (bool traverseable, LayerMask layerMask, Vector3 worldPos, int gridX, int gridY) {
		this.traverseable = traverseable;
		this.layerMask = layerMask;
		this.worldPos = worldPos;
		this.gridX = gridX;
		this.gridY = gridY;
		this.thisInst = ++inst;
	}

	public void SetCosts(int gCost, int hCost) {
		this.gCost = gCost;
		this.hCost = hCost;
		fCost = gCost + hCost;
	}

	
}
