using UnityEditor;
using UnityEngine;

namespace Teyke
{
    [System.Serializable]
    public class GridmapCell
    {
        public Vector3 center;
        public float width, depth;
        public Vector3[] SceneVerts;
        public int x, z;

        public bool valid;

        public enum CellState
        {
            Open,
            Buildable,
            Occupied
        }
        public CellState state = CellState.Open;

        public int treeID = 0;

        private static Vector3[] DefaultQuadVerts = new Vector3[4] {
        new Vector3(-0.5f, 0, -0.5f),   // all edges length 1
        new Vector3(-0.5f, 0, 0.5f),
        new Vector3(0.5f, 0, 0.5f),
        new Vector3(0.5f, 0, -0.5f)};

        public void Initialize(Vector3 c, float w = 1, float d = 1)
        {
            center = c;
            width = w;
            depth = d;

            valid = true;

            SetSceneVerts();
        }
        public void SetSceneVerts()
        {
            if (SceneVerts == null) SceneVerts = new Vector3[4];
            Vector3 scale = new Vector3(width, 0, depth);
            for (int i = 0; i < DefaultQuadVerts.Length; i++)
            {
                SceneVerts[i] = DefaultQuadVerts[i];
                SceneVerts[i].Scale(scale);
                SceneVerts[i] += center;
            }
        }
        public void SetSceneVerts(Terrain t)
        {
            if (SceneVerts == null) SceneVerts = new Vector3[4];
            Vector3 scale = new Vector3(width, 1, depth);

            float tl = t.SampleHeight(center + new Vector3(-width / 2.0f, 0, depth / 2.0f));
            float tr = t.SampleHeight(center + new Vector3(width / 2.0f, 0, depth / 2.0f));
            float bl = t.SampleHeight(center + new Vector3(-width / 2.0f, 0, -depth / 2.0f));
            float br = t.SampleHeight(center + new Vector3(width / 2.0f, 0, -depth / 2.0f));

            SceneVerts[0] = center + new Vector3(-width / 2.0f, bl - center.y, -depth / 2.0f);
            SceneVerts[1] = center + new Vector3(-width / 2.0f, tl - center.y, depth / 2.0f);
            SceneVerts[2] = center + new Vector3(width / 2.0f, tr - center.y, depth / 2.0f);
            SceneVerts[3] = center + new Vector3(width / 2.0f, br - center.y, -depth / 2.0f);
        }

        public override bool Equals(object o)
        {
            if (!(o is GridmapCell)) return false;

            return (o as GridmapCell).x == x && (o as GridmapCell).z == z;
        }
    }
}