using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Teyke
{
    public class gridmap : MonoBehaviour
    {
        public float cellWidth = 1;
        public float cellDepth = 1;
        public float cliffHeight = 0.5f;

        private int countx;
        private int countz;

        private Vector3 halfCellSize
        {
            get { return new Vector3(cellWidth / 2.0f, 0, cellDepth / 2.0f); }
        }

        [SerializeField]
        public GridmapCell[] cells;
        private GridmapCell this[int x, int z]
        {
            get
            {
                if (x < 0 || z < 0 || x >= countx || z >= countz) return null;
                return cells[(x * countz) + z];
            }
        }

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void GenerateGrid()
        {
            if (gameObject.GetComponent<Terrain>() != null)
                GenerateGrid_Terrain();

            else return;


        }

        private void GenerateGrid_Terrain()
        {
            countx = (int)(gameObject.collider.bounds.size.x / cellWidth);
            countz = (int)(gameObject.collider.bounds.size.z / cellDepth);

            Terrain terrain = gameObject.GetComponent<Terrain>() as Terrain;

            cells = new GridmapCell[countx * countz];

            for (int x = 0; x < countx; x++)
            {
                for (int z = 0; z < countz; z++)
                {
                    cells[(x * countz) + z] = ScriptableObject.CreateInstance<GridmapCell>();
                    cells[(x * countz) + z].Initialize(new Vector3(x * cellWidth, 0, z * cellDepth) + halfCellSize + gameObject.transform.position, cellWidth, cellDepth);
                    cells[(x * countz) + z].center.y = terrain.SampleHeight(cells[(x * countz) + z].center);
                    cells[(x * countz) + z].SetSceneVerts();
                    cells[(x * countz) + z].valid = CellIsSmooth(cells[(x * countz) + z], terrain);
                    cells[(x * countz) + z].x = x;
                    cells[(x * countz) + z].z = z;
                }
            }
        }

        private bool CellIsSmooth(GridmapCell c, Terrain t)
        {
            // grab 4-corners + center heights
            float ch = t.SampleHeight(c.center);
            float tl = t.SampleHeight(c.center + new Vector3(-cellWidth / 2.0f, 0, cellDepth / 2.0f));
            float tr = t.SampleHeight(c.center + new Vector3(cellWidth / 2.0f, 0, cellDepth / 2.0f));
            float bl = t.SampleHeight(c.center + new Vector3(-cellWidth / 2.0f, 0, -cellDepth / 2.0f));
            float br = t.SampleHeight(c.center + new Vector3(cellWidth / 2.0f, 0, -cellDepth / 2.0f));

            // check if center diff is too big:
            if (Mathf.Abs(ch - tl) > cliffHeight ||
                Mathf.Abs(ch - tr) > cliffHeight ||
                Mathf.Abs(ch - bl) > cliffHeight ||
                Mathf.Abs(ch - br) > cliffHeight)
                return false;

            // check if top-left diff is too big:
            if (Mathf.Abs(tl - tr) > cliffHeight ||
                Mathf.Abs(tl - bl) > cliffHeight)
                return false;

            // check if bot-right diff is too big:
            if (Mathf.Abs(br - tr) > cliffHeight ||
                Mathf.Abs(br - bl) > cliffHeight)
                return false;

            return true;
        }

        public GridmapCell GetCellFromPoint(Vector3 point)
        {
            // new Vector3(x * cellWidth, 0, z * cellDepth) + halfCellSize + gameObject.transform.position
            int x = (int)((point.x - gameObject.transform.position.x) / cellWidth);
            int z = (int)((point.z - gameObject.transform.position.z) / cellDepth);
            
            if (x >= countx || x < 0 || z >= countz || z < 0) return null;

            return cells[(x * countz) + z];
        }

        public List<Vector3> FindPath(Vector3 start, Vector3 end, bool checkValidity)
        {
            GridmapCell current = GetCellFromPoint(start);
            GridmapCell target = GetCellFromPoint(end);

            return FindPath(current, target, checkValidity);
        }
        public List<Vector3> FindPath(GridmapCell start, GridmapCell end, bool checkValidity)
        {
            if (start == null || end == null) return new List<Vector3>();
            if ((start.treeID != end.treeID) && checkValidity) return new List<Vector3>();

            return PathingHelper.FindPath(start, end, this, checkValidity);
        }

        private sealed class PathingHelper
        {
            private sealed class AStarRecord
            {
                public GridmapCell cell;
                public AStarRecord parent;
                public float linkCost;
                public float totalCost;
                public float estimatedFinalCost;

                public static Vector3 finalTarget;

                public AStarRecord(GridmapCell c, AStarRecord p, float cost)
                {
                    if (c == null) throw new UnityException("Cannot create an AStarRecord with a null node.");

                    cell = c;
                    parent = p;
                    linkCost = cost;

                    totalCost = (parent != null) ? parent.totalCost + linkCost : linkCost;
                    estimatedFinalCost = totalCost + (finalTarget - cell.center).magnitude;
                }
                public void Reparent(AStarRecord newP, float cost)
                {
                    parent = newP;
                    linkCost = cost;

                    totalCost = (parent != null) ? parent.totalCost + linkCost : linkCost;
                    estimatedFinalCost = totalCost + (finalTarget - cell.center).magnitude;
                }
            }

            private static List<AStarRecord> openList = new List<AStarRecord>();
            private static List<AStarRecord> closedList = new List<AStarRecord>();
            private static bool checkValiditiy;

            public static List<Vector3> FindPath(GridmapCell start, GridmapCell end, gridmap map, bool check)
            {
                List<Vector3> path = new List<Vector3>();
                if (start == null || end == null) return path;

                openList.Clear();
                closedList.Clear();
                checkValiditiy = check;
                AStarRecord.finalTarget = end.center;

                openList.Add(new AStarRecord(start, null, 0));

                while (openList.Count > 0)
                {
                    AStarRecord current = openList[0];
                    if (current.cell.Equals(end)) break;

                    bool up_ok =    PlaceInCorrectList(map[current.cell.x, current.cell.z + 1], current, 1.0f);
                    bool left_ok =  PlaceInCorrectList(map[current.cell.x - 1, current.cell.z], current, 1.0f);
                    bool right_ok = PlaceInCorrectList(map[current.cell.x + 1, current.cell.z], current, 1.0f);
                    bool down_ok =  PlaceInCorrectList(map[current.cell.x, current.cell.z - 1], current, 1.0f);

                    if (up_ok && left_ok) PlaceInCorrectList(map[current.cell.x - 1, current.cell.z + 1], current, 1.4f);
                    if (up_ok && right_ok) PlaceInCorrectList(map[current.cell.x + 1, current.cell.z + 1], current, 1.4f);
                    if (down_ok && left_ok) PlaceInCorrectList(map[current.cell.x - 1, current.cell.z - 1], current, 1.4f);
                    if (down_ok && right_ok) PlaceInCorrectList(map[current.cell.x + 1, current.cell.z - 1], current, 1.4f);

                    closedList.Add(current);
                    openList.RemoveAt(0);

                    openList.Sort((l1, l2) => l1.estimatedFinalCost.CompareTo(l2.estimatedFinalCost));
                }

                if (openList.Count == 0) return path;   // no path found.

                AStarRecord r = openList[0];
                do
                {
                    path.Add(r.cell.center);
                    r = r.parent;
                } while (r != null && !r.cell.Equals(start));
                path.Reverse();
                return path;
            }
            private static bool PlaceInCorrectList(GridmapCell cell, AStarRecord parent, float cost)
            {
                if (cell == null) return false;
                if (!cell.valid && checkValiditiy) return false;

                AStarRecord olinstance = openList.FirstOrDefault(r => r.cell.Equals(cell));
                if (olinstance != null)  // it's on the open list.
                {
                    if (olinstance.totalCost > parent.totalCost + cost)  // this new path is shorter
                        olinstance.Reparent(parent, cost);
                    return true;
                }

                AStarRecord clinstance = closedList.FirstOrDefault(r => r.cell.Equals(cell));
                if (clinstance != null) // it's in the closed list
                {
                    return true;
                }

                // add it to the open list
                openList.Add(new AStarRecord(cell, parent, cost));
                return true;
            }
        }  
    }
}