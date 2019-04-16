using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public class Water : MonoBehaviour
{
    public bool useJobs;
    public int resolution = 256;
    public float size = 100;
    public NoiseLayer[] layers;
    NativeArray<Vector3> verticesNative;

    Vector3[] vertices;
    Mesh mesh;

    private void Start()
    {
        mesh = new Mesh();
        var uvs = new List<Vector2>();
        var vertices = new List<Vector3>();
        var triangles = new List<int>();
        int i = 0;
        for (int x = 0; x < resolution; x++)
        {
            for (int y = 0; y < resolution; y++)
            {
                var pos = new Vector3(x * size, 0, y * size);
                vertices.Add(pos);
                if (x < resolution - 1 && y < resolution - 1)
                {
                    triangles.Add(i);
                    triangles.Add(i + 1);
                    triangles.Add(i + resolution + 1);
                    triangles.Add(i);
                    triangles.Add(i + resolution + 1);
                    triangles.Add(i + resolution);
                }
                i++;
            }
        }

        foreach (var v in vertices)
        {
            uvs.Add(new Vector2(v.x, v.z));
        }

        mesh.MarkDynamic();
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();
        mesh.SetUVs(0, uvs);
        GetComponent<MeshFilter>().mesh = mesh;
        this.vertices = mesh.vertices;
        verticesNative = new NativeArray<Vector3>(this.vertices, Allocator.Persistent);
    }

    private void OnDestroy()
    {
        verticesNative.Dispose();
    }

    private void Update()
    {
        if(useJobs)
        {
            JobHandle handle = default(JobHandle);
            for (int i = 0; i < layers.Length; i++)
            {
                var layer = layers[i];
                var job = new NoiseJob()
                {
                    vertices = verticesNative,
                    layer = layer,
                    t = Time.time,
                    add = i > 0
                };
                if (i == 0)
                    handle = job.Schedule(vertices.Length, vertices.Length/ 6);
                else handle = job.Schedule(vertices.Length, vertices.Length / 6, handle);

            }
            handle.Complete();
            verticesNative.CopyTo(vertices);
        }
        else
        {
            var t = Time.time;
            for (int i = 0; i < layers.Length; i++)
            {
                var layer = layers[i];
                var vCount = vertices.Length;
                for (int j = 0; j < vCount; j++)
                {
                    var vertex = vertices[j];
                    var noise = Mathf.PerlinNoise((vertex.x * layer.frequency) + t, (vertex.z * layer.frequency) + t) * layer.waveSize;
                    if (i > 0)
                        vertex.y += noise;
                    else vertex.y = noise;
                    vertices[j] = vertex;
                }
            }
        }
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    }

}
[System.Serializable]
public struct NoiseLayer
{
    [Range(0f, .3f)]
    public float frequency;
    public float waveSize;
    public float speed;
}

struct NoiseJob : IJobParallelFor
{
    public NativeArray<Vector3> vertices;

    public NoiseLayer layer;
    public bool add;
    public float t;

    public void Execute(int index)
    {
        var vertex = vertices[index];
        var noise = Mathf.PerlinNoise((vertex.x * layer.frequency) + t, (vertex.z * layer.frequency) + t) * layer.waveSize;
        if (add)
            vertex.y += noise;
        else vertex.y = noise;
        vertices[index] = vertex;
    }
}

