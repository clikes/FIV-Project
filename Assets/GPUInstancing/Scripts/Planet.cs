using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public bool isStatic;
    public float mass = 10;
    public float startSpeed = 100;
    public PlanetData data;
    Transform _transform;

    public void Init()
    {
        
        _transform = transform;
        data = new PlanetData(transform.localScale.x * transform.localScale.x, transform.localScale.x, _transform.position, _transform.forward * startSpeed);
        _transform.position = data.position;
    }

    public void ProcessGravity(PlanetData other, float G, float dt)
    {
        if (isStatic)
            return;
        data.Process(other, G, dt);
    }

    public void UpdateTransform()
    {
        _transform.position = data.position;
    }

}
