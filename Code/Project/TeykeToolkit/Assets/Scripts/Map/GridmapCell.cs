using UnityEditor;
using UnityEngine;

namespace Teyke
{
    [System.Serializable]
    public class GridmapCell// : ScriptableObject
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
        public CellState state;

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

        public override bool Equals(object o)
        {
            if (!(o is GridmapCell)) return false;

            return (o as GridmapCell).x == x && (o as GridmapCell).z == z;
        }
    }
}