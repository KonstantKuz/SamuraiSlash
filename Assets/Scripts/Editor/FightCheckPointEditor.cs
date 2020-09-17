using System;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(FightCheckPoint))]
public class FightCheckPointEditor : Editor
{
    // List/Array property name in base class
    private string arrayPropertyName = "pointEnemies";
    private ReorderableDrawer arrayDrawer;
    private void OnEnable()
    {
        arrayDrawer = new ReorderableDrawer(ReorderableType.WithRemoveButtons, false);
        arrayDrawer.SetUp(serializedObject, arrayPropertyName);
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawPropertiesExcluding(serializedObject,  arrayPropertyName);
        serializedObject.ApplyModifiedProperties();
        
        arrayDrawer.Draw(serializedObject, target);
        serializedObject.ApplyModifiedProperties();
    }
}
