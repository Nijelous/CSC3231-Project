using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Profiling;
using UnityEngine.UI;

// Updates the memory counter text, using ProfilerRecorder
public class MemoryCounter : MonoBehaviour
{
    [SerializeField]
    Text counter;

    ProfilerRecorder totalMemory;
    ProfilerRecorder usedMemory;

    private void Start()
    {
        totalMemory = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Reserved Memory");
        usedMemory = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "System Used Memory");

    }


    // Update is called once per frame
    void Update()
    {
        if (usedMemory.Valid && totalMemory.Valid)
        {
            try
            {
                // Converts the memory usage as a percentage of used memory to total memory
                counter.text = "Memory Usage: " + Mathf.RoundToInt(usedMemory.LastValue / totalMemory.LastValue * 10) + "%";
            }
            catch // Can error right at the start when there's no total memory loaded in
            {
                Debug.Log("Starting up, error caused, move along");
            }
        }
    }
}
