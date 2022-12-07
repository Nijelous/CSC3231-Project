using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Updates the FPS counter text
public class FPSCounter : MonoBehaviour
{
    [SerializeField]
    Text counter;

    private float[] framerates;
    private int frameIndex = 0;

    private void Start()
    {
        framerates = new float[50];
    }

    // Update is called once per frame
    void Update()
    {
        framerates[frameIndex] = 1 / Time.deltaTime;
        frameIndex = (frameIndex + 1) % framerates.Length;

        counter.text = "FPS: " + Mathf.RoundToInt(GetFPS()).ToString();
    }

    // Calculates the framerate based on an average of all the framerates in the past 50 updates, in order to get a smooth framerate displayed
    private float GetFPS()
    {
        float total = 0f;
        foreach(float f in framerates)
        {
            total += f;
        }
        return total / framerates.Length;
    }
}
