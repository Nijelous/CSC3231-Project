using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Deals with the tesla coils and their logic, providing many public getters and setters for mainly the FieldManager class
public class TeslaLightning : MonoBehaviour
{
    private int hits;
    private int maxHits;
    [SerializeField]
    private GameObject sphere;
    [SerializeField]
    private GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        maxHits = Random.Range(1, 5);
        hits = 0;
    }

    public bool IsActive()
    {
        return sphere.activeInHierarchy;
    }

    public void Activate() // When activated, hits is increased
    {
        sphere.SetActive(true);
        hits++;
    }

    public void Deactivate()
    {
        sphere.SetActive(false);
    }

    public bool Explode() // If hits are more than the max hits the tower can take, it explodes using a coroutine and returns true
    {
        if(hits >= maxHits)
        {
            StartCoroutine(Explosion());
            return true;
        }
        else
        {
            return false;
        }
    }

    // The handler for the explosion, which modifies the terrain to have a sphere embedded itno it by updating the terrain data
    IEnumerator Explosion()
    {
        Terrain t = GameObject.Find("Terrain").GetComponent<Terrain>();
        float startY = Mathf.RoundToInt(transform.position.y)-13.5f;
        int width = 30;
        int height = 30;
        int startX = Mathf.RoundToInt(transform.position.x);
        int startZ = Mathf.RoundToInt(transform.position.z);
        // Gets the current heights of the terrain in a square from the centre of the start positions
        // Start positions are moved back depending on where they are on the field in order to centre them as best as they can be
        float[,] heights = t.terrainData.GetHeights((startX / 2) - (15 - Mathf.RoundToInt(startX/75)), (startZ/2)-(15 - Mathf.RoundToInt(startZ / 75)), width, height);
        // Loops through all coordinates inside of the square specified, updating it if the circle equation is both lower than the terrain and not null (using imaginary numbers)
        for(int z = 0; z < width; z++)
        {
            for(int x = 0; x < width; x++)
            {
                int newX = x > width / 2 ? width - x : x;
                int newZ = z > width / 2 ? width - z : z;
                float circleEq = (startY - Mathf.Sqrt(Mathf.Pow(width / 2, 2) - Mathf.Pow((width / 2) - newX, 2) - Mathf.Pow((width / 2) - newZ, 2))) / 600f;
                if (!float.IsNaN(circleEq) || circleEq > t.terrainData.GetHeight(startX + x - 14, startZ + z - 14))
                {
                    heights[x, z] = circleEq;
                }
                //heights[x, z] = (startY - Mathf.Sqrt(Mathf.Pow(width/2, 2) - Mathf.Pow((width/2) - newX, 2) - Mathf.Pow((width/2) - newZ, 2))) / 600f;
            }
        }
        t.terrainData.SetHeights((startX / 2)- (15 - Mathf.RoundToInt(startX / 75)), (startZ / 2) - (15 - Mathf.RoundToInt(startZ / 75)), heights);
        // Runs the explosion, which expands over three seconds
        explosion.SetActive(true);
        float timer = 3f;
        while(timer > 0)
        {
            explosion.transform.localScale = new Vector3(explosion.transform.localScale.x + 0.2f, explosion.transform.localScale.y + 0.2f, explosion.transform.localScale.z + 0.2f);
            explosion.GetComponentInChildren<Light>().range += 0.5f;
            timer -= Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
