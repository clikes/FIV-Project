using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;


public class Simulation : MonoBehaviour
{
    public Vector3 spread;
    public float massRandomness;
    public float startSpeedRandomness;
    public bool useJob;
    public bool useParticles;
    public Planet moon;
    public float minSize = 1;
    public float maxSize = 3;
    public Gradient gradient;
    public int simSize = 10000;
    public Planet[] suns;
    public float simulationSpeed;
    public float colorRange;
    public float gravityConstant = 9;
    public Color colorA, colorB;
    List<Planet> moons;

    NativeArray<PlanetData> planets;
    NativeArray<PlanetData> sunsNative;
    public ParticleSystem particleSystem;

    NativeArray<ParticleSystem.Particle> particlesNative;

    TransformAccessArray planetsAccess;
    ParticleSystem.Particle[] particles;

    public void ResetVelocity()
    {
        for (int i = 0; i < planets.Length; i++)
        {
            var p = planets[i];
            p.velocity = Vector3.zero;
            planets[i] = p;
        }
    }
    Vector3 randomVector
    {
        get { return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)); }
    }
    public void Ball1()
    {
        var ePos = suns[0].data.position;
        for (int i = 0; i < planets.Length; i++)
        {
            var p = planets[i];
            p.position = Vector3.zero;
            var pos = ePos + (Quaternion.AngleAxis(Random.Range(0f, 360), randomVector) * Vector3.forward * 100);
            
            p.velocity = Vector3.Cross(pos - ePos, randomVector).normalized * Random.Range(2f, 3f);
            p.position = pos;
            planets[i] = p;
        }
    }
    public void Ball3()
    {
        var ePos = suns[0].data.position;
        for (int i = 0; i < planets.Length; i++)
        {
            var p = planets[i];
            p.position = Vector3.zero;
            var pos = ePos + (Quaternion.AngleAxis(Random.Range(0f, 360f), randomVector) * Vector3.down * Random.Range(100f, 105f));

            p.velocity = Vector3.Cross(pos - ePos, randomVector).normalized * Random.Range(0.9f, 1f);
            p.position = pos;
            planets[i] = p;
        }
    }

    public void Disk2()
    {
        var ePos = suns[0].data.position;
        for (int i = 0; i < planets.Length; i++)
        {
            var p = planets[i];
            p.position = Vector3.zero;
            var pos = ePos + (Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.down) * Vector3.forward * Random.Range(0, 150f));
            pos += new Vector3(0, 180, 0);
            p.velocity = Vector3.down * 2;
            p.velocity += Vector3.Cross(pos - ePos, Vector3.down).normalized *  1f;
            p.position = pos;
            planets[i] = p;
        }
    }
    public void Disk()
    {
        var ePos = suns[0].data.position;
        for (int i = 0; i < planets.Length; i++)
        {
            var p = planets[i];
            p.position = Vector3.zero;
            var pos = ePos + (Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.down) * Vector3.forward * Random.Range(0, 155f));
            pos += new Vector3(0, 180, 0);
            p.velocity = Vector3.down * 2;
            p.position = pos;
            planets[i] = p;
        }
    }
    public void Ball4()
    {
        var ePos = suns[0].data.position;
        float a = 0;
        var step = 360f / simSize;
        for (int i = 0; i < planets.Length; i++)
        {
            var p = planets[i];
            p.position = Vector3.zero;
            var d = (Quaternion.AngleAxis(a += step, Vector3.up) * Vector3.forward * Random.Range(55f, 120f));
            var pos = ePos + d;

            p.velocity = Vector3.Cross(d.normalized, Vector3.up) * 3.5f;
            p.position = pos;
            planets[i] = p;
        }
    }
    public void Ball2()
    {
        var ePos = suns[0].data.position;
        for (int i = 0; i < planets.Length; i++)
        {
            var p = planets[i];
            p.position = Vector3.zero;
            var pos = ePos + (Quaternion.AngleAxis(Random.Range(0, 360), new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f))) * Vector3.forward * 100);

            p.velocity = Vector3.Cross(ePos - pos, Vector3.up).normalized * 4;
            p.position = pos;
            planets[i] = p;
        }
    }
    public void Explode()
    {
        var ePos = suns[0].data.position;
        for (int i = 0; i < planets.Length; i++)
        {
            var p = planets[i];
            var pos = ePos + Vector3.ClampMagnitude(new Vector3(Random.Range(-spread.x, spread.x), Random.Range(-spread.y, spread.y), Random.Range(-spread.z, spread.z)), 12);
            p.position = pos;
            p.velocity = Vector3.zero;
            planets[i] = p;
        }
    }

    private void Start()
    {
        foreach (var s in suns)
        {
            s.Init();
        }

        moon.Init();
      
        moon.gameObject.SetActive(false);
        var forward = moon.transform.forward;
        var pos = moon.transform.position;
       
        planets = new NativeArray<PlanetData>(simSize, Allocator.Persistent);
        var planetsData = new PlanetData[simSize];
        for (int i = 0; i < planetsData.Length; i++)
        {
            var data = new PlanetData();
            data.velocity = forward * (moon.startSpeed + Random.Range(-startSpeedRandomness, startSpeedRandomness));
            data.mass = moon.mass + Random.Range(-massRandomness, massRandomness);
            var p = pos + Vector3.ClampMagnitude(new Vector3(Random.Range(-spread.x, spread.x), Random.Range(-spread.y, spread.y), Random.Range(-spread.z, spread.z)), 25);
       
            data.position = p;
            data.size = Random.Range(minSize, maxSize);
            planetsData[i] = data;
        }
        planets.CopyFrom(planetsData);

        
        if (useParticles)
        {
            particles = new ParticleSystem.Particle[simSize];
            for (int i = 0; i < particles.Length; i++)
            {
                var part = particles[i];
                part.rotation3D = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
                particles[i] = part;
            }
            particlesNative = new NativeArray<ParticleSystem.Particle>(particles, Allocator.Persistent);
            sunsNative = new NativeArray<PlanetData>(suns.Length, Allocator.Persistent);
            for (int i = 0; i < suns.Length; i++)
            {
                sunsNative[i] = suns[i].data;
            }
        }
        else
        {
            moons = new List<Planet>(simSize);
         
          
            List<Transform> planetTransforms = new List<Transform>();

            for (int i = 0; i < simSize; i++)
            {
                var m = Instantiate(moon);
                m.data = planetsData[i];
                m.Init();
                if (useJob)
                    planetTransforms.Add(m.transform);
                moons.Add(m);
            }
            planetsAccess = new TransformAccessArray(planetTransforms.ToArray());
        }


        
    }

    private void OnDestroy()
    {

    }
    private void OnApplicationQuit()
    {
        planets.Dispose();
        if (useJob)
        {

            planetsAccess.Dispose();
        }
        if (useParticles)
        {
            particlesNative.Dispose();
            sunsNative.Dispose();
        }
           
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        for (int i = 0; i < suns.Length; i++)
        {
            suns[i].data.position = suns[i].transform.position;
            sunsNative[i] = suns[i].data;
        }

        var job = new PlanetParticleJob();
        job.particles = particlesNative;
        job.planets = planets;
        job.suns = sunsNative;
        job.G = gravityConstant;
        job.dt = Time.deltaTime * simulationSpeed;
        job.colorA = colorA;
        job.colorB = colorB;
        job.colorRange = colorRange;

        var handle = job.Schedule(planets.Length, planets.Length / 8);
        handle.Complete();

        job.particles.CopyTo(particles);
        GetComponent<ParticleSystem>().SetParticles(particles, particles.Length);
          
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResetVelocity();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Ball1();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Ball2();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Ball3();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Ball4();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Disk();
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Disk2();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Explode();
        }
    }

}

[System.Serializable]
public struct PlanetData
{
    public float mass;
    public float size;
    public Vector3 velocity, position;

    public PlanetData(float mass,  float size ,Vector3 pos, Vector3 vel)
    {
        this.size = size;
        this.mass = mass;
        this.velocity = vel;
        this.position = pos;
    }



    public Vector3 Process(PlanetData planet, float G, float dt)
    {
        var v = planet.position - position;
        //var s = planet.size / 5;
        //if (v.sqrMagnitude < s * s)
        //{
        //    velocity = Vector3.zero;
        //    return position;
        //}
        var f = G * ((planet.mass * mass) / v.sqrMagnitude);
        velocity += (v.normalized * f * dt) / mass;
        return position;
    }
}

//public struct PlanetJob : IJobParallelForTransform
//{
//    public NativeArray<PlanetData> planets;
//    public PlanetData sun;

//    public float dt, G;
//    public void Execute(int index, TransformAccess transform)
//    {
//        var d = planets[index];
//        transform.position = d.Process(sun, G, dt);
//        planets[index] = d;
//    }
//}

public struct PlanetParticleJob : IJobParallelFor
{
    public NativeArray<ParticleSystem.Particle> particles;
    public NativeArray<PlanetData> planets;
    [ReadOnly] public NativeArray<PlanetData> suns;

    public float dt, G, colorRange;
    public Color colorA, colorB;

    public void Execute(int index)
    {
        var part = particles[index];
        var planet = planets[index];
        var sl = suns.Length;
        for (int i = 0; i < sl; i++)
        {
            planet.Process(suns[i], G, dt);
        }
        planet.position += planet.velocity * dt;
        part.position = planet.position;
        part.velocity = Vector3.zero;
        part.remainingLifetime = 5;
        part.startSize = planet.size;
        part.startColor = Color.Lerp(colorA, colorB, planet.velocity.sqrMagnitude / (colorRange * colorRange));
        //part.startColor = colorA;
        //part.rotation3D = part.rotation3D + (new Vector3(0, dt, 0) * 15);
        planets[index] = planet;
        particles[index] = part;
    }
}
