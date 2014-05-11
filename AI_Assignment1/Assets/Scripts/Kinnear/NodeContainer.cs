using UnityEngine;
using System.Collections;

public enum AbleToWalkOn
{
	WALKABLE = 0,
	UNWALKABLE
};

// Open Closed List | 0 == not yet checked / undefined | 1 == open list | 2 == closed list
public enum OpenClosedCheck 
{
	OPEN = 0,
	CLOSED,
	UNDEFINED
};

public class NodeContainer : MonoBehaviour {

	public AbleToWalkOn walkable;

	public OpenClosedCheck nodeType;

	public int F;
	public int G;
	public int H;

	// Use this for initialization
	void Awake () {
		F = G = H = 0;
		walkable = AbleToWalkOn.UNWALKABLE;
		nodeType = OpenClosedCheck.UNDEFINED;
	}
}
