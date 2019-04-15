﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataIO;
using UnityEngine.Jobs;
using Unity.Collections;
using Unity.Jobs;

public class DataVisualizorG : MonoBehaviour
{
    // Start is called before the first frame update

    List<GameObject> dataobjects = new List<GameObject>();
    public ParticleSystem ps;
    NativeArray<PointData> points;
    NativeArray<ParticleSystem.Particle> particlesNative;
    ParticleSystem.Particle[] particles;

    Vector3[] dataPositions;
    float[] xvalues;
    float[] yvalues;
    float[] zvalues;
    public void LoadData(string xcolname, string ycolname, string zcolname, DataExtractor data)
    {
        dataPositions = new Vector3[data.dataLength];
        xvalues = new float[data.dataLength];
        yvalues = new float[data.dataLength];
        zvalues = new float[data.dataLength];
        float[][] threeAxies = { xvalues, yvalues, zvalues };


        string[] colnames = { xcolname, ycolname, zcolname };

        for (int i = 0; i < colnames.Length; i++)
        {
            string colname = colnames[i];
            if (data.GetDataType(colname) == typeof(string))
            {
                List<string> strs = data.GetStrColumn(colname);
                //TODO for axis setting the right string value to it
                List<string> strValue = new List<string>();
                ///list of string method indexof return the first value equal to the string
                /// so cannot use indexof to indentify which index it is.
                for (int j = 0; j < strs.Count; j++)
                {
                    string str = strs[j];
                    if (!strValue.Contains(str))
                    {
                        strValue.Add(str);
                    }
                    threeAxies[i][j] = strValue.IndexOf(str);
                }

            }
            else
            {
                List<float> datas = data.GetColumn(colname);
                for (int j = 0; j < datas.Count; j++)
                {
                    threeAxies[i][j] = 1;
                    threeAxies[i][j] = datas[j];
                }
            }
        }

        for (int i = 0; i < data.dataLength; i++)
        {
            dataPositions[i] = new Vector3(xvalues[i], yvalues[i], zvalues[i]);
        }

        //foreach (var vector3 in dataPositions)
        //{
        //    //dataobjects.Add(Instantiate(dataPoint, vector3, dataPoint.transform.rotation));
        //}
        ParticleProcess();
    }

    public void ParticleProcess()
    {
        if (points.Length != 0)
        {
            points.Dispose();
            particlesNative.Dispose();
        }
        

        particles = new ParticleSystem.Particle[dataPositions.Length];

        points = new NativeArray<PointData>(dataPositions.Length, Allocator.Persistent);
        var pointsData = new PointData[dataPositions.Length];
        for (int i = 0; i < pointsData.Length; i++)
        {
            var data = new PointData();
            data.position = dataPositions[i];
            data.size = 1;
            pointsData[i] = data;
        }

        for (int i = 0; i < particles.Length; i++)
        {
            var part = particles[i];
            part.rotation3D = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            part.position = dataPositions[i];
            particles[i] = part;
        }

        particlesNative = new NativeArray<ParticleSystem.Particle>(particles, Allocator.Persistent);

        points.CopyFrom(pointsData);


    }
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void OnApplicationQuit()
    {
        if (points != null)
        {
            points.Dispose();
        }
        if (particlesNative != null)
        {
            particlesNative.Dispose();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (dataPositions != null)
        {
            var job = new PointJobParticleJob();
            //job.dataPositions = dataPositions;
            job.particles = particlesNative;
            job.points = points;

            var handle = job.Schedule(points.Length, points.Length / 8);
            handle.Complete();

            job.particles.CopyTo(particles);
            ps.SetParticles(particles, particles.Length);
            Debug.Log(ps.isPlaying);
            //ps.Play();
        }
    }
}

[System.Serializable]
public struct PointData
{
    public float size;
    public Vector3 position;

    public PointData(float size, Vector3 pos)
    {
        this.size = size;
        this.position = pos;
    }



    public Vector3 Process(float size, Vector3 pos)
    {
        this.size = size;
        this.position = pos;
        //var v = planet.position - position;
        //var s = planet.size / 5;
        //if (v.sqrMagnitude < s * s)
        //{
        //    velocity = Vector3.zero;
        //    return position;
        //}
        //var f = G * ((planet.mass * mass) / v.sqrMagnitude);
        //velocity += (v.normalized * f * dt) / mass;
        return position;
    }
}


public struct PointJobParticleJob : IJobParallelFor
{
    public NativeArray<ParticleSystem.Particle> particles;
    public NativeArray<PointData> points;
    //public Vector3[] dataPositions;
    //public float[] size;
    //[ReadOnly] public NativeArray<PointData> suns;

    public float dt, G, colorRange;
    public Color colorA, colorB;

    public void Execute(int index)
    {
        var part = particles[index];
        var point = points[index];
        //var dl = dataPositions.Length;
        //for (int i = 0; i < dl; i++)
        //{
        //    if (size == null)
        //    {
        //        point.Process(1, dataPositions[i]);
        //    }
        //    else
        //    {
        //        point.Process(size[i], dataPositions[i]);
        //    }
           
        //}
       // planet.position += planet.velocity * dt;
        part.position = point.position;
        part.velocity = Vector3.zero;
        part.remainingLifetime = 5;
        part.startSize = point.size;
        //part.startColor = Color.Lerp(colorA, colorB, planet.velocity.sqrMagnitude / (colorRange * colorRange));
        //part.startColor = colorA;
        part.startColor = Color.red;
        //part.rotation3D = part.rotation3D + (new Vector3(0, dt, 0) * 15);
        points[index] = point;
        particles[index] = part;
    }
}
