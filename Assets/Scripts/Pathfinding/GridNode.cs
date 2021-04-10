using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode
{
    public int gX;
    public int gY;

    public bool isWall;
    public Vector3 pos;

    public GridNode NodeParent;

    public int gCost;
    public int hCost;

    public int FCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public GridNode(bool sisWall, Vector3 sPos, int sgX, int sgY)
    {
        isWall = sisWall;
        pos = sPos;
        gX = sgX;
        gY = sgY;
    }

}
