using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{

    GridSystem GS;
    public Transform startPosition;
    public Transform endPosition;
    public bool pathDebounce;

    private void Awake()
    {
        GS = GetComponent<GridSystem>();
        FindPath(startPosition.position, endPosition.position);
    }

    private void Update()
    {
    }

    void FindPath(Vector3 startPos, Vector3 endPos)
    {
        GridNode StartNode = GS.nodeFromPoint(startPos);
        GridNode TargetNode = GS.nodeFromPoint(endPos);

        List<GridNode> openList = new List<GridNode>();
        HashSet<GridNode> closedList = new HashSet<GridNode>();

        openList.Add(StartNode);

        while (openList.Count > 0)
        {
            GridNode currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].FCost < currentNode.FCost || openList[i].FCost == currentNode.FCost && openList[i].hCost < currentNode.hCost)
                {
                    currentNode = openList[i];
                }
            }
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == TargetNode)
            {
                GetFinalPath(StartNode, TargetNode);
            }

            foreach (GridNode nextNode in GS.GetNextNodes(currentNode))
            {
                if (!nextNode.isWall || closedList.Contains(nextNode))
                {
                    continue;
                }
                int moveCost = currentNode.gCost + getDistance(currentNode, nextNode);

                if (moveCost < nextNode.gCost || !openList.Contains(nextNode))
                {
                    nextNode.gCost = moveCost;
                    nextNode.hCost = getDistance(nextNode, TargetNode);
                    nextNode.NodeParent = currentNode;

                    if (!openList.Contains(nextNode))
                    {
                        openList.Add(nextNode);
                    }
                }
            }

        }
    }
    void GetFinalPath(GridNode startNode, GridNode endNode)
    {
        List<GridNode> finalPath = new List<GridNode>();
        GridNode currentNode = endNode;

        while (currentNode != startNode)
        {
            finalPath.Add(currentNode);
            currentNode = currentNode.NodeParent;
        }

        finalPath.Reverse();

        GS.finalPath = finalPath;
    }

    int getDistance(GridNode nodeA, GridNode nodeB)
    {
        int ix = Mathf.Abs(nodeA.gX - nodeB.gX);
        int iy = Mathf.Abs(nodeA.gY - nodeB.gY);

        return ix + iy;
    }
}
