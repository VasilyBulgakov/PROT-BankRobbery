using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ImportTangoMesh))]
public class ImportMeshEditor : Editor {

    public override void OnInspectorGUI() {
        if (GUILayout.Button("choose mesh")) {
            ((ImportTangoMesh)serializedObject.targetObject).LoadMesh();
        }
        if (GUILayout.Button("save mesh")) {
            ((ImportTangoMesh)serializedObject.targetObject).SaveMesh();
        }
        if (GUILayout.Button("clear mesh")) {
            ((ImportTangoMesh)serializedObject.targetObject).ClearObject();
        }
        if (GUILayout.Button("remove this")) {
            ((ImportTangoMesh)serializedObject.targetObject).Die();
        }
    }
}
