using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class RealWaves : MonoBehaviour
{
    public static RealWaves Instance { get; private set; }

    const int MaxWaves = 8;
    const float Gravity = 9.81f;

    [System.Serializable]
    public class Wave
    {
        [Tooltip("Half the wave height in metres.")]
        public float amplitude = 0.5f;

        [Tooltip("Distance in metres from one crest to the next.")]
        public float wavelength = 40f;

        [Tooltip("Direction the wave travels, in degrees. 0 = +X, 90 = +Z.")]
        public float direction = 0f;

        [Range(0f, 1f)]
        public float steepness = 0.12f;

        public Wave() { }

        public Wave(float amplitude, float wavelength, float direction, float steepness)
        {
            this.amplitude = amplitude;
            this.wavelength = wavelength;
            this.direction = direction;
            this.steepness = steepness;
        }
    }

    [Header("Ocean Mesh")]
    public int meshResolution = 250;
    public float size = 300f;
    public bool followCamera = true;
    public Transform followTarget;

    [Header("Waves (up to 8)")]
    public float timeScale = 1f;

    public Wave[] waves = new Wave[]
    {
        new Wave(0.70f, 48f,   25f, 0.16f),
        new Wave(0.35f, 31f,  -15f, 0.15f),
        new Wave(0.22f, 19f,   60f, 0.14f),
        new Wave(0.12f, 12f,  -55f, 0.12f),
        new Wave(0.07f, 9.5f, 110f, 0.08f),
        new Wave(0.04f, 7f,  -140f, 0.06f)
    };

    public static float WaveTime => Time.time;

    static readonly int WaveAId = Shader.PropertyToID("_SUNK_WaveA");
    static readonly int WaveBId = Shader.PropertyToID("_SUNK_WaveB");
    static readonly int WaveCountId = Shader.PropertyToID("_SUNK_WaveCount");
    static readonly int TimeId = Shader.PropertyToID("_SUNK_Time");

    private readonly Vector4[] waveA = new Vector4[MaxWaves];
    private readonly Vector4[] waveB = new Vector4[MaxWaves];
    private int waveCount;

    private Mesh mesh;
    private Camera cam;

    void Awake()
    {
        Instance = this;
        UpdateWaveData();
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
        if (mesh != null) Destroy(mesh);
    }

    void Start()
    {
        cam = Camera.main;
        CreateGrid();
        PushToShader();
    }

    void Update()
    {
        UpdateWaveData();
        PushToShader();
    }

    void LateUpdate()
    {
        Transform target = followTarget;
        if (target == null && followCamera && cam != null) target = cam.transform;
        if (target == null) return;

        float cell = size / Mathf.Max(1, meshResolution);
        Vector3 pos = transform.position;
        pos.x = Mathf.Round(target.position.x / cell) * cell;
        pos.z = Mathf.Round(target.position.z / cell) * cell;
        transform.position = pos;
    }

    void UpdateWaveData()
    {
        int count = 0;
        float steepSum = 0f;

        if (waves != null)
        {
            for (int i = 0; i < waves.Length && count < MaxWaves; i++)
            {
                Wave w = waves[i];
                if (w == null) continue;

                float k = 2f * Mathf.PI / Mathf.Max(0.5f, w.wavelength);
                float rad = w.direction * Mathf.Deg2Rad;
                float steep = Mathf.Max(0f, w.steepness);

                waveA[count] = new Vector4(Mathf.Cos(rad), Mathf.Sin(rad), steep, k);
                waveB[count] = new Vector4(w.amplitude, Mathf.Sqrt(Gravity * k), 0f, 0f);

                steepSum += steep;
                count++;
            }
        }

        float norm = steepSum > 1f ? 1f / steepSum : 1f;
        for (int i = 0; i < count; i++)
        {
            waveA[i].z *= norm;
        }

        for (int i = count; i < MaxWaves; i++)
        {
            waveA[i] = Vector4.zero;
            waveB[i] = Vector4.zero;
        }

        waveCount = count;
    }

    void PushToShader()
    {
        Shader.SetGlobalVectorArray(WaveAId, waveA);
        Shader.SetGlobalVectorArray(WaveBId, waveB);
        Shader.SetGlobalFloat(WaveCountId, waveCount);
        Shader.SetGlobalFloat(TimeId, WaveTime * timeScale);
    }

    public float GetHeight(float worldX, float worldZ)
    {
        float t = WaveTime * timeScale;
        float px = worldX;
        float pz = worldZ;

        for (int pass = 0; pass < 4; pass++)
        {
            float offX = 0f;
            float offZ = 0f;

            for (int i = 0; i < waveCount; i++)
            {
                Vector4 a = waveA[i];
                float theta = a.w * (a.x * px + a.y * pz) - waveB[i].y * t;
                float push = Mathf.Cos(theta) * (a.z / a.w);
                offX += a.x * push;
                offZ += a.y * push;
            }

            px = worldX - offX;
            pz = worldZ - offZ;
        }

        float y = 0f;
        for (int i = 0; i < waveCount; i++)
        {
            Vector4 a = waveA[i];
            float theta = a.w * (a.x * px + a.y * pz) - waveB[i].y * t;
            y += waveB[i].x * Mathf.Sin(theta);
        }

        return transform.position.y + y;
    }

    public float GetHeight(Vector3 worldPosition)
    {
        return GetHeight(worldPosition.x, worldPosition.z);
    }

    void CreateGrid()
    {
        int res = Mathf.Max(1, meshResolution);

        mesh = new Mesh();
        mesh.name = "OceanMesh";

        int vertCount = (res + 1) * (res + 1);
        if (vertCount > 65535)
        {
            mesh.indexFormat = IndexFormat.UInt32;
        }

        Vector3[] vertices = new Vector3[vertCount];
        int[] triangles = new int[res * res * 6];

        float stepSize = size / res;

        for (int i = 0, z = 0; z <= res; z++)
        {
            for (int x = 0; x <= res; x++)
            {
                vertices[i] = new Vector3((x * stepSize) - (size / 2f), 0f, (z * stepSize) - (size / 2f));
                i++;
            }
        }

        int tris = 0;
        for (int z = 0; z < res; z++)
        {
            for (int x = 0; x < res; x++)
            {
                int start = z * (res + 1) + x;
                triangles[tris + 0] = start;
                triangles[tris + 1] = start + res + 1;
                triangles[tris + 2] = start + 1;

                triangles[tris + 3] = start + 1;
                triangles[tris + 4] = start + res + 1;
                triangles[tris + 5] = start + res + 2;

                tris += 6;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.bounds = new Bounds(Vector3.zero, new Vector3(size, 100f, size));

        GetComponent<MeshFilter>().sharedMesh = mesh;
    }
}