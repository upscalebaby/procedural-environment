using UnityEngine;

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
                meshData.vertices [vertexIndex] = new Vector3 (topLeftX + x, heightCurve.Evaluate(currentHeight) * heightMultiplier, topLeftZ - y);
                meshData.uvs [vertexIndex] = new Vector2 (x / (float)width, y / (float)height);

                // Defining triangles
                if(x < width - 1 && y < height - 1) {   // guard to ignore right bottom edge and far right edge
                    meshData.AddTriangle(vertexIndex, vertexIndex + width + 1, vertexIndex + width);
                    meshData.AddTriangle(vertexIndex + width + 1, vertexIndex, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }

        return meshData;
    }

    public static MeshDataNoSharedVertices createMeshNoSharedVertices(int width, int height, float heightMultiplier, float[,] heightMap) {
        MeshDataNoSharedVertices meshData = new MeshDataNoSharedVertices (width, height);

        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;
        int vertexIndex = 0;

        for(int y = 0; y < height; y++) {
            for(int x = 0; x < width; x++) {
                // Height at current position
                float currentHeight = heightMap [x, y];

                //  Defining vertices and uvs
                meshData.vertices [vertexIndex] = new Vector3 (topLeftX + x, currentHeight * currentHeight * heightMultiplier, topLeftZ - y);
                meshData.vertices [vertexIndex + 1] = new Vector3 (topLeftX + x, currentHeight * currentHeight * heightMultiplier, topLeftZ - y);

                meshData.uvs [vertexIndex] = new Vector2 (x / (float )width, y / (float )height);
                meshData.uvs [vertexIndex + 1] = new Vector2 (x / (float )width, y / (float )height);

                // Defining triangles
                if(x < width - 1 && y < height - 1) {   // skip vertices at right and bottom edge
                    meshData.AddTriangle(vertexIndex, vertexIndex + 2*width  + 2, vertexIndex + 2*width);
                    meshData.AddTriangle(vertexIndex + 2*width + 3, vertexIndex + 1, vertexIndex + 2);
                }

                vertexIndex += 2;
            }
        }

        return meshData;
    }

    public class MeshDataNoSharedVertices {
        public Vector3[] vertices;
        public int[] triangles;
        public Vector2[] uvs;

        int triangleIndex;

        public MeshDataNoSharedVertices(int width, int height) {
            int nrOfVertices = (width - 1) * (height - 1) * 6;

            vertices = new Vector3[nrOfVertices];
            uvs = new Vector2[nrOfVertices];
            triangles = new int[nrOfVertices];
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