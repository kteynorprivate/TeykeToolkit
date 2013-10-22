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

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void GeneratePath()
    {
        if (target == null) return;

        generateHeatmap();
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

        Debug.Log(string.Format("x:{0}, y:{1}", target.XIndex, target.YIndex));
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
        setHeatmapValue(c - 1, r - 1, currentValue + 1);    // up-left
        setHeatmapValue(c, r - 1, currentValue + 1);        // up
        setHeatmapValue(c + 1, r - 1, currentValue + 1);    // up-right
        setHeatmapValue(c - 1, r, currentValue + 1);        // left
        setHeatmapValue(c + 1, r, currentValue + 1);        // right
        setHeatmapValue(c - 1, r + 1, currentValue + 1);    // down-left
        setHeatmapValue(c, r + 1, currentValue + 1);        // down
        setHeatmapValue(c + 1, r + 1, currentValue + 1);    // down-right
    }
}
