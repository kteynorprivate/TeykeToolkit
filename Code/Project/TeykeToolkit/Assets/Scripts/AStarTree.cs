using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarTree : MonoBehaviour
{
    public Map map;
    private AStarNode[,] nodes;

    private void CreateNodes()
    {
        nodes = new AStarNode[map.Width, map.Height];
        for (int r = 0; r < map.Height; r++)
        {
            for (int c = 0; c < map.Width; c++)
            {
                nodes[c, r] = new AStarNode();
                nodes[c, r].position = map.Tiles[(r * map.Width) + c].transform.position;
                nodes[c, r].heuristic = map.Tiles[(r * map.Width) + c].GetComponent<Tile>().pathingType == PathingType.UnPathable ? int.MaxValue : 0;
            }
        }

        for (int r = 0; r < map.Height; r++)
        {
            for (int c = 0; c < map.Width; c++)
            {
                nodes[c, r].connections = new AStarConnection[8];

                nodes[c, r].connections[0] = (c > 0 && r > 0) ? new AStarConnection(nodes[c - 1, r - 1], 1.4f) : null;                          // up-left
                nodes[c, r].connections[1] = (r > 0) ? new AStarConnection(nodes[c, r - 1], 1.0f) : null;                                              // up
                nodes[c, r].connections[2] = (c < map.Width - 1 && r > 0) ? new AStarConnection(nodes[c + 1, r - 1], 1.4f) : null;              // up-right
                nodes[c, r].connections[3] = (c > 0) ? new AStarConnection(nodes[c - 1, r], 1.0f) : null;                                              // left
                nodes[c, r].connections[4] = (c < map.Width - 1) ? new AStarConnection(nodes[c + 1, r], 1.0f) : null;                                  // right
                nodes[c, r].connections[5] = (c > 0 && r < map.Height - 1) ? new AStarConnection(nodes[c - 1, r + 1], 1.4f) : null;             // bot-left
                nodes[c, r].connections[6] = (r < map.Height - 1) ? new AStarConnection(nodes[c, r + 1], 1.0f) : null;                                 // bot
                nodes[c, r].connections[7] = (c < map.Width - 1 && r < map.Height - 1) ? new AStarConnection(nodes[c + 1, r + 1], 1.4f) : null;  // bot-right
            }
        }
    }

    public AStarPath GeneratePath(Vector3 start, Vector3 target)
    {
        if (nodes == null)
            CreateNodes();

        AStarPath path = new AStarPath();

        Tile currentTile = map.ClosestTile(start);
        Tile endTile = map.ClosestTile(target);

        AStarNode startNode = nodes[currentTile.XIndex, currentTile.YIndex];
        AStarNode endNode = nodes[endTile.XIndex, endTile.YIndex];


        List<AStarPathConnection> openList = new List<AStarPathConnection>();
        List<AStarPathConnection> closedList = new List<AStarPathConnection>();

        AStarPathConnection first = new AStarPathConnection() { node = startNode, parent = null, costSoFar = 0, estimatedTotal = (endNode.position - startNode.position).magnitude };

        openList.Add(first);

        int currentIndex = 0;

        while (openList.Count > 0)
        {
            if (openList[currentIndex].node == endNode) break;
            AStarNode currentNode = openList[currentIndex].node;
            bool recordFound = false;

            #region Check Connections
            for (int c = 0; c < 8; c++)
            {
                if (currentNode.connections[c] == null) continue;

                // look for the connection on the open list.
                foreach (var conn in openList)
                {
                    if (conn.node == currentNode.connections[c].connector) // node was on the list
                    {
                        // see if this path was shorter than the one it took previously
                        float newCost = openList[currentIndex].costSoFar + (currentNode.GetDistance(conn.node));

                        if (newCost < conn.costSoFar)
                        {
                            conn.costSoFar = newCost;
                            conn.parent = openList[currentIndex];
                            conn.estimatedTotal = newCost + conn.node.heuristic + conn.node.GetDistance(endNode);
                        }

                        recordFound = true;
                        break;
                    }
                }
                if (recordFound) continue;

                for (int i = 0; i < closedList.Count; i++)
                {
                    if (closedList[i].node == currentNode.connections[c].connector)   // node was on the list
                    {
                        // check if it needs to be moved back to open list
                        float newCost = openList[currentIndex].costSoFar + (currentNode.GetDistance(closedList[i].node));

                        if (newCost < closedList[i].costSoFar)
                        {
                            closedList[i].costSoFar = newCost;
                            closedList[i].parent = openList[currentIndex];
                            closedList[i].estimatedTotal = newCost + closedList[i].node.heuristic + closedList[i].node.GetDistance(endNode);

                            openList.Add(closedList[i]);
                            closedList.RemoveAt(i);
                        }

                        recordFound = true;
                        break;
                    }
                }
                if (recordFound) continue;

                AStarPathConnection newRecord = new AStarPathConnection()
                {
                    node = currentNode.connections[c].connector,
                    parent = openList[currentIndex],
                    costSoFar = openList[currentIndex].costSoFar + currentNode.connections[c].connector.GetDistance(currentNode)
                };
                newRecord.estimatedTotal = newRecord.costSoFar + newRecord.node.GetDistance(endNode);
                openList.Add(newRecord);

            }
            #endregion Check Connections
            
            closedList.Add(openList[currentIndex]);
            openList.RemoveAt(currentIndex);

            float smallestEstimate = float.MaxValue;
            for (int i = 0; i < openList.Count; i++)
            {
                if (openList[i].estimatedTotal < smallestEstimate)
                {
                    smallestEstimate = openList[i].estimatedTotal;
                    currentIndex = i;
                }
            }
        }

        Debug.Log(currentIndex);

        // currentNode == endTile node
        AStarPathConnection current = openList[currentIndex];
        while (current.parent != null)
        {
            path.nodes.Insert(0, current.node);
            current = current.parent;
        }
        path.nodes.Insert(0, startNode);


        return path;
    }
}