using UnityEngine;
using System.Collections;

namespace Teyke
{
    public class TowerBuilder : MonoBehaviour
    {
        private GridmapCell selected;

        public GameEntity[] towers;

        // Use this for initialization
        void Start()
        {
            Messenger<GameEntity>.RegisterListener("GameEntityPressed", x => { selected = null; });
            Messenger<Gridmap>.RegisterListener("MapPressed", SetSelected);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnGUI()
        {
            if (selected == null) return;

            DrawBuildGUI();
            DrawOutline();
        }
        private void DrawBuildGUI()
        {
            if (towers == null) return;

            GUILayout.BeginArea(new Rect(0, Screen.height - 200, 200, 200));
            for (int i = 0; i < towers.Length; i++)
            {
                if (GUILayout.Button("Build " + towers[i].TypeName))
                {
                    Debug.Log("Building " + towers[i].TypeName);
                    GameObject tower = Instantiate(towers[i], selected.center, Quaternion.identity) as GameObject;
                    tower.GetComponent<TileBound>().SetPosition(selected.center);
                }
            }
            GUILayout.EndArea();
        }
        private void DrawOutline()
        {
            GL.Begin(GL.LINES);
            GL.Color(Color.magenta);
            GL.Vertex3(selected.SceneVerts[0].x, selected.SceneVerts[0].y, selected.SceneVerts[0].z);
            GL.Vertex3(selected.SceneVerts[1].x, selected.SceneVerts[1].y, selected.SceneVerts[1].z);
            GL.Vertex3(selected.SceneVerts[1].x, selected.SceneVerts[1].y, selected.SceneVerts[1].z);
            GL.Vertex3(selected.SceneVerts[2].x, selected.SceneVerts[2].y, selected.SceneVerts[2].z);
            GL.Vertex3(selected.SceneVerts[2].x, selected.SceneVerts[2].y, selected.SceneVerts[2].z);
            GL.Vertex3(selected.SceneVerts[3].x, selected.SceneVerts[3].y, selected.SceneVerts[3].z);
            GL.Vertex3(selected.SceneVerts[3].x, selected.SceneVerts[3].y, selected.SceneVerts[3].z);
            GL.Vertex3(selected.SceneVerts[0].x, selected.SceneVerts[0].y, selected.SceneVerts[0].z);
            GL.End();
        }

        private void SetSelected(Gridmap map)
        {
            RaycastHit hitinfo;
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hitinfo))
            {
                selected = map.GetCellFromPoint(hitinfo.point);
                if (selected.state != GridmapCell.CellState.Buildable) selected = null;
            }
        }
    }
}