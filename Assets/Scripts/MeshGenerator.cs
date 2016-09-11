using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class MeshGenerator {

    public static MeshData createMesh(int width, int height, float heightMultiplier, AnimationCurve heightCurve, float[,] heightMap) {
        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;
        MeshData meshData = new MeshData (width, height);

        int vertexIndex = 0;

        for(int y = 0; y < height; y++) {
            for(int x = 0; x < width; x++) {
                // Height at current position
                float currentHeight = heightMap [x, y];

                //  Defining vertices and uvs
                meshData.vertices [vertexIndex] = new Vector3 (topLeftX + x, heightCurve.Evaluate(currentHeight) * heightMultiplier, topLeftZ - y); //TODO: lek med multiplikationsordningen
                meshData.uvs [vertexIndex] = new Vector2 (x / (float)width, y / (float)height);

                // Defining triangles with the vertices
                if(x < width - 1 && y < height - 1) {   // guard to ignore right bottom edge and far right edge
                    meshData.AddTriangle(vertexIndex, vertexIndex + width + 1, vertexIndex + width);
                    meshData.AddTriangle(vertexIndex + width + 1, vertexIndex, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }

        return meshData;
    }

    // Internal class that holds the data for each mesh
    public class MeshData {
        public Vector3[] vertices;
        public int[] triangles;
        public Vector2[] uvs;

        int triangleIndex;

        public MeshData(int width, int height) {
            int nrOfVertices = width * height;
            int nrOfTriangles = (width - 1) * (height - 1) * 6;

            vertices = new Vector3[nrOfVertices];
            uvs = new Vector2[width * height];
            triangles = new int[nrOfTriangles];
        }

        public void AddTriangle(int a, int b, int c) {
            triangles [triangleIndex] = a;
            triangles [triangleIndex + 1] = b;
            triangles [triangleIndex + 2] = c;

            triangleIndex += 3;
        }

        public Mesh createMesh() {
            Mesh mesh = new Mesh ();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            mesh.RecalculateNormals ();
            return mesh;
        }

    }

}