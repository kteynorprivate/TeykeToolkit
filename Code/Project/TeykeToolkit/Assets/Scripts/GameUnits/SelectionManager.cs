using UnityEngine;
using System.Collections;

// TODO: figure out a good way to implement this.
// need an elegant way to render/handle different things based on what was pressed-- dynamically.
// should let developers choose what happens when an entity is selected.
namespace Teyke
{
    public class SelectionManager : MonoBehaviour
    {
        GameEntity currentSelection;

        // Use this for initialization
        void Start()
        {
            Messenger<GameEntity>.RegisterListener("GameEntityPressed", x => { currentSelection = x; });
            Messenger<Gridmap>.RegisterListener("MapPressed", t => { currentSelection = null; });
        }

        void OnGUI()
        {
            if (currentSelection == null) return;

            currentSelection.ShowControlGUI();
        }

        void OnPostRender()
        {
            if (currentSelection == null) return;
            ShowSelectionSquare();
        }

        void ShowSelectionSquare()
        {
            // show selection square
            float h = currentSelection.transform.position.y;

            Vector3 p0 = currentSelection.gameObject.renderer.bounds.min;
            Vector3 p2 = currentSelection.gameObject.renderer.bounds.max;

            p0 -= currentSelection.transform.position;
            p2 -= currentSelection.transform.position;
            p0 *= 1.25f;
            p2 *= 1.25f;
            p0 += currentSelection.transform.position;
            p2 += currentSelection.transform.position;

            Vector3 p1 = new Vector3(p0.x, 0, p2.z);
            Vector3 p3 = new Vector3(p2.x, 0, p0.z);

            GL.Begin(GL.LINES);
            GL.Color(Color.white);
            GL.Vertex3(p0.x, h, p0.z);
            GL.Vertex3(p1.x, h, p1.z);
            GL.Vertex3(p1.x, h, p1.z);
            GL.Vertex3(p2.x, h, p2.z);
            GL.Vertex3(p2.x, h, p2.z);
            GL.Vertex3(p3.x, h, p3.z);
            GL.Vertex3(p0.x, h, p0.z);
            GL.Vertex3(p3.x, h, p3.z);
            GL.End();
        }
    }
}