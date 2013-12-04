using UnityEngine;
using System.Collections;

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
    }
}