using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(TerrainGenerator))]
public class TerrainGeneratorEditor : Editor {
    
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        //TerrainGenerator s = (TerrainGenerator )target;
        //s.GenerateTerrain();
        
    }
}
