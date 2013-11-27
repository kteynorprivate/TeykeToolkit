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
	int maxHP;
	bool hasAttack;
	float minRange;
	float maxRange;
	float minDamage;
	float maxDamage;
	
	[MenuItem("Teyke/New Unit Type", false, 11)]
	public static void NewUnit ()
	{
		var window = EditorWindow.GetWindow<NewUnitDialog> (true, "New Unit");
		window.minSize = new Vector2(300, 160);
		window.unitName = "new unit";
		window.maxHP = 10;
	}
	
	void OnGUI()
	{
		EditorGUIUtility.labelWidth = 70;

		unitName = EditorGUILayout.TextField ("Name", unitName).Replace(" ", "");
		maxHP = EditorGUILayout.IntField ("Max HP", maxHP);

		hasAttack = EditorGUILayout.ToggleLeft("Has Attack", hasAttack);
		if(hasAttack)
		{
			EditorGUIUtility.labelWidth = 35;

			EditorGUILayout.BeginHorizontal(new GUIStyle(){margin = new RectOffset(15,0,0,0)});
			EditorGUILayout.LabelField("Damage", GUILayout.Width(60), GUILayout.ExpandWidth(false));
			minDamage = EditorGUILayout.FloatField("Min", minDamage);
			maxDamage = EditorGUILayout.FloatField("Max", maxDamage);
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal(new GUIStyle(){margin = new RectOffset(15,0,0,0)});
			EditorGUILayout.LabelField("Range", GUILayout.Width(60), GUILayout.ExpandWidth(false));
			minRange = EditorGUILayout.FloatField("Min", minRange);
			maxRange = EditorGUILayout.FloatField("Max", maxRange);
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
		}

		EditorGUILayout.Space();
		if(GUILayout.Button("Create"))
		{
			if(string.IsNullOrEmpty(unitName))
			{
				EditorUtility.DisplayDialog("Error", "The unit needs to have a name.", "Ok");
				return;
			}

			string unitPath = "Assets/Units/" + unitName + ".prefab";
			Debug.Log(Resources.LoadAssetAtPath<Object>(unitPath) + " " + unitPath);
			if(Resources.LoadAssetAtPath<Object>(unitPath))
			{
				if(EditorUtility.DisplayDialog("Warning", "A prefab with that name already exists. Do you want to overwrite it?", "Yes", "No"))
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
		GameObject newUnit = new GameObject (unitName);
		GameUnit unit = newUnit.AddComponent<GameUnit>();
		unit.maxHP = maxHP;

		if(hasAttack) 
		{
			Attack attack = newUnit.AddComponent<Attack>();
			attack.minDamage = minDamage;
			attack.maxDamage = maxDamage;
			attack.minRange = minRange;
			attack.maxRange = maxRange;
		}

		if(!Directory.Exists(Application.dataPath + "/Units"))
		{
			Debug.Log("Creating Units folder.");
			AssetDatabase.CreateFolder("Assets", "Units");
		}
		PrefabUtility.CreatePrefab (path, newUnit);
		Debug.Log ("New unit prefab created: " + unitName);
		DestroyImmediate (newUnit);
	}
}















































