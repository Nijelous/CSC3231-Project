using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The orbit physics and maths for the spheres around the tesla coils
public class Orbit : MonoBehaviour
{
    private float speed;
    private float diameter;
    private float timer;
    private float startX;
    private float startZ;

    private void Start()
    {
        speed = 5;
        diameter = 6f;
        startX = transform.position.x;
        startZ = transform.position.z;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.deltaTime * speed;

        // Moves the sphere in a circle, with the start position being the origin, using cos and sin
        transform.position = new Vector3(startX + Mathf.Cos(timer) * diameter, transform.position.y, startZ + Mathf.Sin(timer) * diameter);
    }
}
