using UnityEditor;
using UnityEngine;
using System.Collections;

[CanEditMultipleObjects]
[CustomEditor(typeof(Attack))]
public class AttackEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Attack attack = (Attack)targets[0];

        // Range
        attack.range = EditorGUILayout.FloatField("Range", attack.range);

        // Damage
        EditorGUILayout.LabelField("Damage");
        attack.minDamage = EditorGUILayout.FloatField("Min", attack.minDamage);//, GUILayout.Width(200));
        attack.maxDamage = EditorGUILayout.FloatField("Max", attack.maxDamage);//, GUILayout.Width(200));
        
        if(targets.Length > 1)
        {
            if(GUILayout.Button("Apply to All"))
            for (int i = 0; i < targets.Length; i++)
            {
                (targets[i] as Attack).range = attack.range;
                (targets[i] as Attack).minDamage = attack.minDamage;
                (targets[i] as Attack).maxDamage = attack.maxDamage;
            }
        }

        SceneView.RepaintAll();
    }

    public void OnSceneGUI()
    {
        Attack attack = (Attack)target;
        Handles.color = Color.red;
        Vector3 pos = attack.gameObject.transform.position;
        pos.y = 0;
        Handles.DrawWireDisc(pos, attack.gameObject.transform.up, attack.range);
    }
}
