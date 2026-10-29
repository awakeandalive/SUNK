using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class RealWaves : MonoBehaviour
{
    [Header("Wave Settings")]
    public int gridResolution = 30; // Detail density of the water grid
    public float size = 60f;        // Total width and length of the ocean
    public float waveHeight = 0.6f; // How high the waves rise and fall
    public float waveSpeed = 2f;    // Speed of the rolling waves
    public float waveFrequency = 0.3f; // Tightness of the waves

    private MeshFilter meshFilter;
    private Mesh mesh;
    private Vector3[] baseVertices;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        CreateGrid();
    }

    void CreateGrid()
    {
        mesh = new Mesh();
        mesh.name = "OceanMesh";
        meshFilter.mesh = mesh;

        int vertCount = (gridResolution + 1) * (gridResolution + 1);
        Vector3[] vertices = new Vector3[vertCount];
        Vector2[] uv = new Vector2[vertCount];
        int[] triangles = new int[gridResolution * gridResolution * 6];

        float stepSize = size / gridResolution;

        for (int i = 0, z = 0; z <= gridResolution; z++)
        {
            for (int x = 0; x <= gridResolution; x++)
            {
                vertices[i] = new Vector3((x * stepSize) - (size / 2f), 0, (z * stepSize) - (size / 2f));
                uv[i] = new Vector2((float)x / gridResolution, (float)z / gridResolution);
                i++;
            }
        }

        int tris = 0;
        for (int z = 0; z < gridResolution; z++)
        {
            for (int x = 0; x < gridResolution; x++)
            {
                int start = z * (gridResolution + 1) + x;
                triangles[tris + 0] = start;
                triangles[tris + 1] = start + gridResolution + 1;
                triangles[tris + 2] = start + 1;

                triangles[tris + 3] = start + 1;
                triangles[tris + 4] = start + gridResolution + 1;
                triangles[tris + 5] = start + gridResolution + 2;

                tris += 6;
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        baseVertices = (Vector3[])vertices.Clone();
    }

    void Update()
    {
        Vector3[] vertices = mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 v = baseVertices[i];

            // Combine multiple Sine and Cosine waves for realistic ocean movement
            float wave1 = Mathf.Sin(Time.time * waveSpeed + (v.x * waveFrequency) + (v.z * waveFrequency));
            float wave2 = Mathf.Cos(Time.time * (waveSpeed * 0.7f) + (v.x * (waveFrequency * 1.3f) - (v.z * waveFrequency)));

            v.y = (wave1 + wave2) * waveHeight;
            vertices[i] = v;
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals(); // Updates lighting reflection dynamically on every wave
    }
}