#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Xml.Serialization;
using System.IO;

[ExecuteInEditMode]
public class ImportTangoMesh : MonoBehaviour {
    private string _loadedMeshName = null;

    public void LoadMesh() {
        var path = EditorUtility.OpenFilePanel("choose mesh file", "", "");

        // load mesh info
        var meshInfo = _DeserializeMesh(path);

        // create mesh
        var mesh = _AreaDescriptionMeshToUnityMesh(meshInfo);

        // add components to render
        var filter = gameObject.AddComponent<MeshFilter>();
        filter.mesh = mesh;
        var renderer = gameObject.AddComponent<MeshRenderer>();

        var pathComponents = path.Split('/');
        _loadedMeshName = pathComponents[pathComponents.Length - 1];
    }

    public void SaveMesh() {
        // save mesh and prefab
        var filter = gameObject.GetComponent<MeshFilter>();
        if (filter != null && filter.mesh != null) {
            var mesh = filter.mesh;
            _SaveMesh(mesh, _loadedMeshName);
        }
    }

    public void ClearObject() {
        var filter = gameObject.GetComponent<MeshFilter>();
        var renderer = gameObject.GetComponent<MeshRenderer>();
        DestroyImmediate(filter);
        DestroyImmediate(renderer);
    }

    public void Die() {
        DestroyImmediate(this);
    }

    [MenuItem("Assets/Import tango mesh")]
    public static void Import() {
        var filePath = EditorUtility.OpenFilePanel("choose mesh file", "", "");
        
        // load mesh info
        var meshInfo = _DeserializeMesh(filePath);

        // create mesh
        var mesh = _AreaDescriptionMeshToUnityMesh(meshInfo);

        // save mesh and prefab
        var pathComponents = filePath.Split('/');
        var filename = pathComponents[pathComponents.Length - 1];
        _SaveMesh(mesh, filename);
    }

    //-- private helpers
    static private AreaDescriptionMesh _DeserializeMesh (string path) {
        XmlSerializer serializer = new XmlSerializer(typeof(AreaDescriptionMesh));
        FileStream file = File.Open(path, FileMode.Open);
        var areaDescriptionMesh = serializer.Deserialize(file) as AreaDescriptionMesh;
        file.Close();

        return areaDescriptionMesh;
    }

    static private Mesh _AreaDescriptionMeshToUnityMesh(AreaDescriptionMesh savedMesh) {
        Mesh mesh = new Mesh();
        mesh.vertices = savedMesh.m_vertices;
        mesh.triangles = savedMesh.m_triangles;
        mesh.RecalculateNormals();
        return mesh;
    }

    static private void _SaveMesh(Mesh mesh, string name) {
        var savePath = EditorUtility.SaveFilePanelInProject("choose save location", name,"asset","");
        AssetDatabase.CreateAsset(mesh, savePath);
        AssetDatabase.SaveAssets();
    }
    
    /// <summary>
    /// Xml container for vertices and triangles from extracted mesh and linked Area Description.
    /// </summary>
    [XmlRoot("AreaDescriptionMesh")]
    public class AreaDescriptionMesh {
        /// <summary>
        /// The UUID of the linked Area Description.
        /// </summary>
        public string m_uuid;

        /// <summary>
        /// The mesh vertices.
        /// </summary>
        public Vector3[] m_vertices;

        /// <summary>
        /// The mesh triangles.
        /// </summary>
        public int[] m_triangles;
    }
}

#endif
