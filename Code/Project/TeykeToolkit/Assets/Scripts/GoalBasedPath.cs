using UnityEngine;
using System.Collections;

public class GoalBasedPath : MonoBehaviour 
{
    public Map map;
    public Tile target;
    public float[,] heatmap;
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
        if (vectorField == null) return Vector3.zero;
        
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
    public void GeneratePath(Tile t)
    {
        target = t;

        GeneratePath();
    }
    private void generateHeatmap()
    {
        heatmap = new float[map.Height, map.Width];
        for (int r = 0; r < map.Height; r++)
        {
            for (int c = 0; c < map.Width; c++)
            {
                heatmap[r, c] = -1;
            }
        }
        setHeatmapValue(target.YIndex, target.XIndex, 0);
    }
    private void setHeatmapValue(int r, int c, float currentValue)
    {
        // no tile
        if (c < 0 || r < 0 || c >= map.Width || r >= map.Height) return;
        // leave unpathable tiles as -1
        if (map.GetTile(r, c).GetComponent<Tile>().pathingType == PathingType.UnPathable) return;
        // if the tile has been visited by a shorter path return
        if (heatmap[r, c] != -1 && heatmap[r, c] <= currentValue) return;
        
        //if (currentValue > 15) return;

        heatmap[r, c] = currentValue;

        // set surrounding tiles
        setHeatmapValue(r - 1, c - 1, currentValue + 1.41f);    // up-left
        setHeatmapValue(r - 1, c, currentValue + 1);        // up
        setHeatmapValue(r - 1, c + 1, currentValue + 1.41f);    // up-right
        setHeatmapValue(r, c - 1, currentValue + 1);        // left
        setHeatmapValue(r, c + 1, currentValue + 1);        // right
        setHeatmapValue(r + 1, c - 1, currentValue + 1.41f);    // down-left
        setHeatmapValue(r + 1, c, currentValue + 1);        // down
        setHeatmapValue(r + 1, c + 1, currentValue + 1.41f);    // down-right
    }
    private void generateVectorField()
    {
        vectorField = new Vector2[map.Height, map.Width];
        for (int r = 0; r < map.Height; r++)
        {
            for (int c = 0; c < map.Width; c++)
            {
                #region old stuff, half working
                if (heatmap[r, c] == -1) continue;

                float left_dist = heatmap[r, c <= 0 ? c : c - 1];
                float right_dist = heatmap[r, c >= map.Width - 1 ? c : c + 1];
                float top_dist = heatmap[r <= 0 ? r : r - 1, c];
                float bot_dist = heatmap[r >= map.Height - 1 ? r : r + 1, c];

                //if (left_dist == -1) left_dist = right_dist;// heatmap[r, c];
                //if (right_dist == -1) right_dist = left_dist;// heatmap[r, c];
                //if (top_dist == -1) top_dist = bot_dist;// heatmap[r, c];
                //if (bot_dist == -1) bot_dist = top_dist;// heatmap[r, c];

                if (left_dist == -1) left_dist = heatmap[r, c];
                if (right_dist == -1) right_dist = heatmap[r, c];
                if (top_dist == -1) top_dist = heatmap[r, c];
                if (bot_dist == -1) bot_dist = heatmap[r, c];

                vectorField[r, c].x = left_dist - right_dist;
                vectorField[r, c].y = top_dist - bot_dist;

                vectorField[r, c].Normalize();
                #endregion

                // find the lowest valued neighbor (>= 0)
                // set the vector to the direction to that neighbor

                //if (heatmap[r, c] == -1) continue;

                //int lowest = heatmap[r, c];
                //Vector2 dir = Vector2.zero;
                //for (int roff = (r > 0 ? -1 : 0); roff <= (r < map.Height-1 ? 1 : 0); roff++)
                //{
                //    for (int coff = (c > 0 ? -1 : 0); coff <= (c < map.Width-1 ? 1 : 0); coff++)
                //    {
                //        //if (coff == 0 && roff == 0) continue;
                //        if (heatmap[r + roff, c + coff] < lowest && heatmap[r + roff, c + coff] >= 0)
                //        {
                //            lowest = heatmap[r + roff, c + coff];
                //            dir = new Vector2(coff, roff);
                //        }
                //    }
                //}
                //vectorField[r, c] = dir;
                //vectorField[r, c].Normalize();
            }
        }
    }
}
