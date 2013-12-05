using UnityEngine;
using System.Collections;

namespace Teyke
{
    public class StructureBuilder : MonoBehaviour
    {
        /// <summary>
        /// Array of all the structures this GameEntity can build.
        /// </summary>
        public GameStructure[] StructuresBuilt;

        /// <summary>
        /// Instantiate a structure from the array of buildable ones at a given location for the player.
        /// </summary>
        /// <param name="sIndex">the index into this builder's array of buildable structures to specify which structure.</param>
        /// <param name="cell">which cell on the map the structure will be built on</param>
        /// TODO: IMPLEMENT MULTI-CELL FOOTPRINTS FOR STRUCTURES
        /// <returns>true if the structure was built succesfully, otherwise false.</returns>
        public bool BuildStructure(int sIndex, GridmapCell cell)
        {
            if (cell.state != GridmapCell.CellState.Buildable) return false;

            try
            {
                GameStructure structure = Instantiate(StructuresBuilt[sIndex], cell.center, Quaternion.identity) as GameStructure;
                structure.owner = gameObject.GetComponent<GameEntity>().owner;
                cell.state = GridmapCell.CellState.Occupied;

                return true;
            }
            catch (System.Exception) { return false; }
        }
    }
}