using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{

    public Transform StartPosition;
    public LayerMask WallMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public float nodeDistance;

    GridNode[,] nodeArray;
    public List<GridNode> finalPath;


    float nodeDiameter;
    int gridSizeX, gridSizeY;

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    void CreateGrid()
    {
        nodeArray = new GridNode[gridSizeX, gridSizeY];

        Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool Wall = true;

                if (Physics.CheckSphere(worldPoint, nodeRadius, WallMask))
                {
                    Wall = false;
                }

                nodeArray[x, y] = new GridNode(Wall, worldPoint, x, y);
            }
        }
    }

    public List<GridNode> GetNextNodes(GridNode nextNode)
    {
        List<GridNode> nextList = new List<GridNode>();
        int checkX;
        int checkY;

        checkX = nextNode.gX + 1;
        checkY = nextNode.gY;
        if (checkX >= 0 && checkX < gridSizeX)
        {
            if (checkY >= 0 && checkY < gridSizeY)
            {
                nextList.Add(nodeArray[checkX, checkY]);
            }
        }

        checkX = nextNode.gX - 1;
        checkY = nextNode.gY;

        if (checkX >= 0 && checkX < gridSizeX)
        {
            if (checkY >= 0 && checkY < gridSizeY)
            {
                nextList.Add(nodeArray[checkX, checkY]);
            }
        }

        checkX = nextNode.gX;
        checkY = nextNode.gY + 1;

        if (checkX >= 0 && checkX < gridSizeX)
        {
            if (checkY >= 0 && checkY < gridSizeY)
            {
                nextList.Add(nodeArray[checkX, checkY]);
            }
        }

        checkX = nextNode.gX;
        checkY = nextNode.gY - 1;

        if (checkX >= 0 && checkX < gridSizeX)
        {
            if (checkY >= 0 && checkY < gridSizeY)
            {
                nextList.Add(nodeArray[checkX, checkY]);
            }
        }

        return nextList;
    }

    public GridNode nodeFromPoint(Vector3 worldPos)
    {
        float xPos = ((worldPos.x + gridWorldSize.x / 2) / gridWorldSize.x);
        float yPos = ((worldPos.z + gridWorldSize.y / 2) / gridWorldSize.y);

        xPos = Mathf.Clamp01(xPos);
        yPos = Mathf.Clamp01(yPos);

        int x = Mathf.RoundToInt((gridSizeX - 1) * xPos);
        int y = Mathf.RoundToInt((gridSizeY - 1) * yPos);

        return nodeArray[x, y];
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (nodeArray != null)
        {
            foreach (GridNode n in nodeArray)
            {
                if (n.isWall)
                {
                    Gizmos.color = Color.white;
                }
                else
                {
                    Gizmos.color = Color.green;
                }


                if (finalPath != null)
                {
                    if (finalPath.Contains(n))
                    {
                        Gizmos.color = Color.red;
                    }

                }

                Gizmos.DrawCube(n.pos, Vector3.one * (nodeDiameter - nodeDistance));
            }
        }
    }
}
