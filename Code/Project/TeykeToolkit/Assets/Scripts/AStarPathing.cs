using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarConnection
{
    public AStarNode connector;
    public float weight;

    public AStarConnection(AStarNode con, float w)
    {
        connector = con;
        weight = w;
    }
}
public class AStarNode
{
    public float heuristic = 0;
    public Vector3 position;
    public AStarConnection[] connections;



    public float GetDistance(AStarNode end)
    {
        return (end.position - position).magnitude;
    }
}
public class AStarPathConnection
{
    public AStarNode node;
    public AStarPathConnection parent;
    public float costSoFar;
    public float estimatedTotal;
}
public class AStarPath
{
    public List<AStarNode> nodes;

    public Vector3 FollowPath(Vector3 position)
    {
        if (nodes.Count == 0)
            return Vector3.zero;

        if ((position - nodes[0].position).sqrMagnitude <= 0.5f)
        {
            nodes.RemoveAt(0);
            return FollowPath(position);
        }

        return (position - nodes[0].position).normalized;
    }
}