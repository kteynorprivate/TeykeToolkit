using UnityEngine;
using System.Collections;

public class GoalBasedPath : MonoBehaviour 
{
    public Map map;
    public Tile target;
    public int[,] heatmap;
    public Vector2[,] vectorField;

    public int maxHeatValue
    {
        get
        {
            int max = 0;
            foreach (int i in heatmap)
            {
                if (i > max) max = i;
            }
            return max;
        }
    }

    public Vector3 FollowPath(Vector3 position)
    {
        Tile current = map.ClosestTile(position);
        return new Vector3(vectorField[current.XIndex, current.YIndex].x, 0, vectorField[current.XIndex, current.YIndex].y);
    }

    // TODO: profile + optimize the heatmap generation. takes too long!
    public void GeneratePath()
    {
        if (target == null) return;

        generateHeatmap();
        generateVectorField();
    }
    private void generateHeatmap()
    {
        heatmap = new int[map.Width, map.Height];
        for (int r = 0; r < map.Height; r++)
        {
            for (int c = 0; c < map.Width; c++)
            {
                heatmap[c, r] = -1;
            }
        }
        setHeatmapValue(target.XIndex, target.YIndex, 0);
    }
    private void setHeatmapValue(int c, int r, int currentValue)
    {
        // no tile
        if (c < 0 || r < 0 || c >= map.Width || r >= map.Height) return;
        // leave unpathable tiles as -1
        if (map.Tiles_MultiDim[c, r].pathingType == PathingType.UnPathable) return;
        // if the tile has been visited by a shorter path return
        if (heatmap[c, r] != -1 && heatmap[c, r] <= currentValue) return;

        heatmap[c, r] = currentValue;

        // set surrounding tiles
        setHeatmapValue(c - 1, r - 1, currentValue + 2);    // up-left
        setHeatmapValue(c, r - 1, currentValue + 1);        // up
        setHeatmapValue(c + 1, r - 1, currentValue + 2);    // up-right
        setHeatmapValue(c - 1, r, currentValue + 1);        // left
        setHeatmapValue(c + 1, r, currentValue + 1);        // right
        setHeatmapValue(c - 1, r + 1, currentValue + 2);    // down-left
        setHeatmapValue(c, r + 1, currentValue + 1);        // down
        setHeatmapValue(c + 1, r + 1, currentValue + 2);    // down-right
    }
    private void generateVectorField()
    {
        vectorField = new Vector2[map.Width, map.Height];
        for (int r = 0; r < map.Height; r++)
        {
            for (int c = 0; c < map.Width; c++)
            {
                if (heatmap[c, r] == -1) continue;

                int left_dist = heatmap[c <= 0 ? c : c - 1, r];
                if (left_dist == -1) left_dist = heatmap[c, r];
                int right_dist = heatmap[c >= map.Width - 1 ? c : c + 1, r];
                if (right_dist == -1) right_dist = heatmap[c, r];
                int top_dist = heatmap[c, r <= 0 ? r : r - 1];
                if (top_dist == -1) top_dist = heatmap[c, r];
                int bot_dist = heatmap[c, r >= map.Height - 1 ? r : r + 1];
                if (bot_dist == -1) bot_dist = heatmap[c, r];

                vectorField[c, r].x = left_dist - right_dist;
                vectorField[c, r].y = top_dist - bot_dist;

                vectorField[c, r].Normalize();
            }
        }
    }
}
