using UnityEngine;
using System.Collections;

namespace Teyke
{
    public class TileBound : MonoBehaviour
    {
        // TODO: Implement larger than 1x1 footprint.
        public int footprintX = 1;
        public int footprintZ = 1;

        public Gridmap map;
        public GridmapCell[] occupiedTiles;

        private Vector3 center
        {
            get
            {
                if (occupiedTiles == null) return Vector3.zero;

                Vector3 c = Vector3.zero;

                for (int i = 0; i < occupiedTiles.Length; i++)
                    c += occupiedTiles[i].center;

                float s = 1.0f / (float)occupiedTiles.Length;
                c.Scale(new Vector3(s, s, s));
                return c;
            }
        }

        // Use this for initialization
        void Start()
        {
            if (map == null) map = GameObject.FindObjectOfType<Gridmap>();

            if (SetPosition(gameObject.transform.position))
            {
                gameObject.transform.position = new Vector3(center.x, center.y + gameObject.transform.renderer.bounds.size.y / 2, center.z);
            }
        }

        /// <summary>
        /// Sets the position of the TileBound object.
        /// </summary>
        /// <param name="position">The position of the tile which the object will be centered around</param>
        /// <returns>True if the position was valid or false if it was not.</returns>
        public bool SetPosition(Vector3 position)
        {
            GridmapCell center = map.GetCellFromPoint(position);
            GridmapCell[] cells = new GridmapCell[footprintX * footprintZ];

            for (int x = 0; x < footprintX; x++)
            {
                for (int z = 0; z < footprintZ; z++)
                {
                    GridmapCell n = map[x + center.x, z + center.z];
                    if (n == null || n.state != GridmapCell.CellState.Buildable) return false;

                    cells[(x * footprintZ) + z] = n;
                }
            }

            //TODO: Fix for 1-sized footprints
            //for (int x = -(footprintX / 2) + 1 - (footprintX % 2); x < footprintX / 2; x++)
            //{
            //    for (int z = -(footprintZ / 2) + 1 - (footprintZ % 2); z < footprintZ / 2; z++)
            //    {
            //        Debug.Log("x:" + x + "\tz:" + z);

            //        // TODO: cache map[x,z] maybe
            //        if (map[x, z] == null || map[x, z].state == GridmapCell.CellState.Occupied)
            //            return false;
            //        else
            //        {
            //            cells[(x * footprintZ) + z] = map[x, z];
            //        }
            //    }
            //}

            occupiedTiles = new GridmapCell[footprintX * footprintZ];
            cells.CopyTo(occupiedTiles, 0);
            
            for (int i = 0; i < footprintX * footprintZ; i++)
                occupiedTiles[i].state = GridmapCell.CellState.Occupied;

            return true;
        }
    }
}