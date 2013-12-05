using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class PotentialFieldGenerator 
{
    // the static map-- which parts of it are pathable, what is blocked. same for all units regardless of team.
    // collection of dynamic obstacles (can be added/removed at runtime). same for all units regardless of team.
    // dynamic list of obstacles for each team:
    //      each individual list will be dynamic at run time, where the itmes on each list are dependent on the team.
    //      the number of lists will be compile-time constant. (will not change as the game is running).


    // three layers of a field:
    // layers are placed over eachother when generating the potential field.
    //      static layer      - non-moveable and statically placed game objects. generated only at startup. (terrain)
    //      semi-static layer - non-moveable but dynamically placed game objects. generated at runtime & when a member of the field is created/destroyed (buildings, destructable terrain)
    //      dynamic layer     - moveable and dynamically placed game objects. generated once every frame. (units)

    public static Map map;
    private static PotentialField staticLayer;

    /// <summary>
    /// Generate and return the potential field for the static layer (terrain) of the game.
    /// If the static layer has already been generated, re-generate or simply return.
    /// </summary>
    /// <param name="forceGeneration">force regeneration of static layer if it was already generated.</param>
    /// <returns>the potential field for the static layer of the map</returns>
    public static PotentialField GenerateStaticLayer(bool forceGeneration = false)
    {
        // return static layer if it already exists and we don't need to regenerated it
        if (staticLayer != null && !forceGeneration) return staticLayer;

        if (map == null) map = (GameObject.FindObjectOfType(typeof(Map)) as GameObject).GetComponent<Map>();
        if (map == null) throw new UnityException("No map object found. Cannot generate the StaticLayer potential field");

        staticLayer = new PotentialField() { Width = map.Width, Height = map.Height };

        for (int r = 0; r < map.Height; r++)
        {
            for (int c = 0; c < map.Width; c++)
            {
                staticLayer.Values[r, c] = Tile.PotentialFieldWeight(map.GetTile(r,c).GetComponent<Tile>().pathingType);
            }
        }

        return staticLayer;
    }
}
