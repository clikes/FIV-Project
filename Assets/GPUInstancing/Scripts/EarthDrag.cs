using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthDrag : MonoBehaviour
{
    public Transform target;
    Camera cam;
    bool dragging;

    float d;
	void Start ()
    {
        cam = Camera.main;
	}
	
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {

            var ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                d = hit.distance;
                dragging = true;
            }
        }
        if(dragging)
        {
            var ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            target.position = Vector3.Lerp(target.position, ray.GetPoint(d), Time.deltaTime * 15);
            if (Input.GetMouseButtonUp(0))
            {
                dragging = false;
            }
        }
     
	}
}
