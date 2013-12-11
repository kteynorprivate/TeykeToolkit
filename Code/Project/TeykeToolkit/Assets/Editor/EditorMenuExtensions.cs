using UnityEditor;
using UnityEngine;
using System.Collections;
using Teyke;
using System.IO;

public class NewMapDialog : EditorWindow
{
	static bool isVisible;
	static bool showMapOverwriteWarning;
	
	[MenuItem("Teyke/Create Map", false, 0)]
	static void CreateMap ()
	{
		if (isVisible)
			return;  // dont allow a second dialog to be opened.
		
		NewMapDialog window = ScriptableObject.CreateInstance<NewMapDialog> ();
		window.position = new Rect (Screen.width / 2, Screen.height / 2, 250, 100);
		window.ShowPopup ();
		showMapOverwriteWarning = Map.ExistsInScene;
		isVisible = true;
	}
	
	void OnGUI ()
	{
		int width = EditorGUILayout.IntSlider ("Width: ", 16, 1, 64);
		int height = EditorGUILayout.IntSlider ("Height: ", 16, 1, 64);
		
		if (showMapOverwriteWarning) {
			EditorGUILayout.LabelField ("Warning: A map object has already been created. Creating a new one will overwrite the existing one.");
		}
		
		if (GUILayout.Button ("Create")) {
			GameObject map = new GameObject ("map");
			map.AddComponent<Map> ();
			map.GetComponent<Map> ().GenerateMap (width, height);
			isVisible = false;
			this.Close ();
		} else if (GUILayout.Button ("Cancel")) {
			isVisible = false;
			this.Close ();
		}
	}
}

public class NewUnitDialog : EditorWindow
{
    string unitName;

    // GameEntity fields
    int maxHP;
    int resourceCost;

    // attack fields
    bool hasAttack;
    bool autoAttack;
    Attack attack;

    // structure builder
    bool buildsStructures;

    // unit producer
    bool producesUnits;

    // upgradable
    bool hasUpgrade;

    [MenuItem("Teyke/New Unit Type", false, 12)]
    public static void DefineUnit()
    {
        var window = EditorWindow.GetWindow<NewUnitDialog>(true, "New Unit Type");
        window.minSize = new Vector2(300, 320);
        window.unitName = "new_unit";
        window.maxHP = 10;
        window.resourceCost = 0;
        window.hasAttack = false;
        window.autoAttack = false;
        window.attack = new Attack();
        window.buildsStructures = false;
        window.producesUnits = false;
        window.hasUpgrade = false;
    }

    void OnGUI()
    {
        EditorGUIUtility.labelWidth = 70;

        unitName = EditorGUILayout.TextField("Name", unitName.Replace(" ", ""));
        maxHP = EditorGUILayout.IntField("Max HP", maxHP);
        resourceCost = EditorGUILayout.IntField("Cost", resourceCost);

        #region Behaviors
        hasAttack = EditorGUILayout.ToggleLeft("Has Attack", hasAttack);
        if (hasAttack)
        {
            EditorGUIUtility.labelWidth = 35;

            autoAttack = EditorGUILayout.ToggleLeft("Auto attack", autoAttack, new GUIStyle() { margin = new RectOffset(15, 0, 0, 0) });

            EditorGUILayout.BeginHorizontal(new GUIStyle() { margin = new RectOffset(15, 0, 0, 0) });
            EditorGUILayout.LabelField("Damage", GUILayout.Width(60), GUILayout.ExpandWidth(false));
            attack.minDamage = EditorGUILayout.FloatField("Min", attack.minDamage);
            attack.maxDamage = EditorGUILayout.FloatField("Max", attack.maxDamage);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal(new GUIStyle() { margin = new RectOffset(15, 0, 0, 0) });
            EditorGUILayout.LabelField("Range", GUILayout.Width(60), GUILayout.ExpandWidth(false));
            attack.minRange = EditorGUILayout.FloatField("Min", attack.minRange);
            attack.maxRange = EditorGUILayout.FloatField("Max", attack.maxRange);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal(new GUIStyle() { margin = new RectOffset(15, 0, 0, 0) });
            EditorGUIUtility.labelWidth = 45;
            attack.attackSpeed = EditorGUILayout.FloatField("Speed", attack.attackSpeed);
            EditorGUILayout.EndHorizontal();
        }

        buildsStructures = EditorGUILayout.ToggleLeft("Builds Structures", buildsStructures);
        if (buildsStructures)
        {
            // todo: allow user to define which structures 
        }

        producesUnits = EditorGUILayout.ToggleLeft("Produces Units", producesUnits);
        if (producesUnits)
        {
            // todo: allow user to define which units
        }

        hasUpgrade = EditorGUILayout.ToggleLeft("Has Upgrade", hasUpgrade);
        if (hasUpgrade)
        {
            // todo: allow user to define what it upgrades into
        }
        #endregion

        EditorGUILayout.Space();
        if (GUILayout.Button("Create"))
        {
            if (string.IsNullOrEmpty(unitName))
            {
                EditorUtility.DisplayDialog("Error", "The unit needs to have a name.", "Ok");
                return;
            }

            string unitPath = "Assets/GameEntities/Units/" + unitName + ".prefab";
            if (Resources.LoadAssetAtPath<Object>(unitPath))
            {
                if (EditorUtility.DisplayDialog("Warning", "A prefab with that name already exists. Do you want to overwrite it?", "Yes", "No"))
                {
                    CreateUnitPrefab(unitPath);
                }
            }
            else CreateUnitPrefab(unitPath);

            this.Close();
        }

    }

    void CreateUnitPrefab(string path)
    {
        GameObject newUnit = new GameObject(unitName);
        GameUnit unit = newUnit.AddComponent<GameUnit>();
        unit.maxHP = maxHP;
        unit.currentHP = maxHP;

        var collider = newUnit.AddComponent<CapsuleCollider>();
        collider.center = new Vector3(collider.center.x, 0, collider.center.z);
        collider.radius = 1;

        newUnit.AddComponent<CharacterController>();

        // create a base object to get default mesh/material from (TODO: define custom mesh/material so instantiating a primitive isn't necessary)
        GameObject capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);

        // set the mesh filter's mesh to a default one (capsule).
        var mesh = newUnit.AddComponent<MeshFilter>();
        mesh.sharedMesh = capsule.GetComponent<MeshFilter>().sharedMesh;

        // add the default mesh renderer
        var renderer = newUnit.AddComponent<MeshRenderer>();
        renderer.sharedMaterial = capsule.renderer.sharedMaterial;

        // remove the base object from the scene
        DestroyImmediate(capsule);

        #region Add Components
        if (hasAttack)
        {
            var atk = newUnit.AddComponent<Attack>();
            atk = attack;

            if (autoAttack)
            {
                newUnit.AddComponent<AutoAttacker>();
            }
        }

        if (buildsStructures)
        {
            newUnit.AddComponent<StructureBuilder>();
        }
        if (producesUnits)
        {
            newUnit.AddComponent<UnitProducer>();
        }
        if (hasUpgrade)
        {
            newUnit.AddComponent<Upgrader>();
        }
        #endregion

        if (!Directory.Exists(Application.dataPath + "/GameEntities"))
            AssetDatabase.CreateFolder("Assets", "GameEntities");
        if (!Directory.Exists(Application.dataPath + "/GameEntities/Units"))
            AssetDatabase.CreateFolder("Assets/GameEntities", "Units");
        PrefabUtility.CreatePrefab(path, newUnit);
        DestroyImmediate(newUnit);
    }
}

public class NewStructureDialog : EditorWindow
{
    string unitName;

    // GameEntity fields
    int maxHP;
    int resourceCost;

    // attack fields
    bool hasAttack;
    bool autoAttack;
    Attack attack;

    // structure builder
    bool buildsStructures;

    // unit producer
    bool producesUnits;

    // upgradable
    bool hasUpgrade;

    [MenuItem("Teyke/New Structure Type", false, 12)]
    public static void DefineUnit()
    {
        var window = EditorWindow.GetWindow<NewStructureDialog>(true, "New Structure Type");
        window.minSize = new Vector2(300, 320);
        window.unitName = "new_unit";
        window.maxHP = 10;
        window.resourceCost = 0;
        window.hasAttack = false;
        window.autoAttack = false;
        window.attack = new Attack();
        window.buildsStructures = false;
        window.producesUnits = false;
        window.hasUpgrade = false;
    }

    void OnGUI()
    {
        EditorGUIUtility.labelWidth = 70;

        unitName = EditorGUILayout.TextField("Name", unitName.Replace(" ", ""));
        maxHP = EditorGUILayout.IntField("Max HP", maxHP);
        resourceCost = EditorGUILayout.IntField("Cost", resourceCost);

        #region Behaviors
        hasAttack = EditorGUILayout.ToggleLeft("Has Attack", hasAttack);
        if (hasAttack)
        {
            EditorGUIUtility.labelWidth = 35;

            autoAttack = EditorGUILayout.ToggleLeft("Auto attack", autoAttack, new GUIStyle() { margin = new RectOffset(15, 0, 0, 0) });

            EditorGUILayout.BeginHorizontal(new GUIStyle() { margin = new RectOffset(15, 0, 0, 0) });
            EditorGUILayout.LabelField("Damage", GUILayout.Width(60), GUILayout.ExpandWidth(false));
            attack.minDamage = EditorGUILayout.FloatField("Min", attack.minDamage);
            attack.maxDamage = EditorGUILayout.FloatField("Max", attack.maxDamage);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal(new GUIStyle() { margin = new RectOffset(15, 0, 0, 0) });
            EditorGUILayout.LabelField("Range", GUILayout.Width(60), GUILayout.ExpandWidth(false));
            attack.minRange = EditorGUILayout.FloatField("Min", attack.minRange);
            attack.maxRange = EditorGUILayout.FloatField("Max", attack.maxRange);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal(new GUIStyle() { margin = new RectOffset(15, 0, 0, 0) });
            EditorGUIUtility.labelWidth = 45;
            attack.attackSpeed = EditorGUILayout.FloatField("Speed", attack.attackSpeed);
            EditorGUILayout.EndHorizontal();
        }

        buildsStructures = EditorGUILayout.ToggleLeft("Builds Structures", buildsStructures);
        if (buildsStructures)
        {
            // todo: allow user to define which structures 
        }

        producesUnits = EditorGUILayout.ToggleLeft("Produces Units", producesUnits);
        if (producesUnits)
        {
            // todo: allow user to define which units
        }

        hasUpgrade = EditorGUILayout.ToggleLeft("Has Upgrade", hasUpgrade);
        if (hasUpgrade)
        {
            // todo: allow user to define what it upgrades into
        }
        #endregion

        EditorGUILayout.Space();
        if (GUILayout.Button("Create"))
        {
            if (string.IsNullOrEmpty(unitName))
            {
                EditorUtility.DisplayDialog("Error", "The unit needs to have a name.", "Ok");
                return;
            }

            string unitPath = "Assets/GameEntities/Structures/" + unitName + ".prefab";
            if (Resources.LoadAssetAtPath<Object>(unitPath))
            {
                if (EditorUtility.DisplayDialog("Warning", "A prefab with that name already exists. Do you want to overwrite it?", "Yes", "No"))
                {
                    CreateUnitPrefab(unitPath);
                }
            }
            else CreateUnitPrefab(unitPath);

            this.Close();
        }

    }

    void CreateUnitPrefab(string path)
    {
        GameObject newStructure = new GameObject(unitName);
        GameUnit unit = newStructure.AddComponent<GameUnit>();
        unit.maxHP = maxHP;
        unit.currentHP = maxHP;

        var collider = newStructure.AddComponent<CapsuleCollider>();
        collider.center = new Vector3(collider.center.x, 0, collider.center.z);
        collider.radius = 1;

        newStructure.AddComponent<TileBound>();

        // create a base object to get default mesh/material from (TODO: define custom mesh/material so instantiating a primitive isn't necessary)
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // set the mesh filter's mesh to a default one (capsule).
        var mesh = newStructure.AddComponent<MeshFilter>();
        mesh.sharedMesh = cube.GetComponent<MeshFilter>().sharedMesh;

        // add the default mesh renderer
        var renderer = newStructure.AddComponent<MeshRenderer>();
        renderer.sharedMaterial = cube.renderer.sharedMaterial;

        // remove the base object from the scene
        DestroyImmediate(cube);

        #region Add Components
        if (hasAttack)
        {
            var atk = newStructure.AddComponent<Attack>();
            atk.minDamage = attack.minDamage;
            atk.maxDamage = attack.maxDamage;
            atk.minRange = attack.minRange;
            atk.maxRange = attack.maxRange;
            atk.attackSpeed = attack.attackSpeed;

            if (autoAttack)
            {
                newStructure.AddComponent<AutoAttacker>();
            }
        }

        if (buildsStructures)
        {
            newStructure.AddComponent<StructureBuilder>();
        }
        if (producesUnits)
        {
            newStructure.AddComponent<UnitProducer>();
        }
        if (hasUpgrade)
        {
            newStructure.AddComponent<Upgrader>();
        }
        #endregion

        if (!Directory.Exists(Application.dataPath + "/GameEntities"))
            AssetDatabase.CreateFolder("Assets", "GameEntities");
        if (!Directory.Exists(Application.dataPath + "/GameEntities/Structures"))
            AssetDatabase.CreateFolder("Assets/GameEntities", "Structures");
        PrefabUtility.CreatePrefab(path, newStructure);
        DestroyImmediate(newStructure);
    }
}















































