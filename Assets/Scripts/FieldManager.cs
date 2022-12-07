using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Deals with the general running of the lightning field, including lightning bolts and destruction
public class FieldManager : MonoBehaviour
{
    private Terrain terrain;

    [SerializeField]
    private GameObject teslaCoil;

    [SerializeField]
    private GameObject lightning;

    private GameObject[][] field;

    private int[][] positions;

    private float[] timers;

    // Start is called before the first frame update
    void Start()
    {
        terrain = GameObject.Find("Terrain").GetComponent<Terrain>();
        // Spawns in all tesla coils on the plane, in their correct positions depending on the Y coordinate of the terrain
        field = new GameObject[19][];
        for(int i = 0; i < field.Length; i++)
        {
            GameObject parent = new GameObject("Row " + i);
            parent.transform.SetParent(transform);
            GameObject[] newArray = new GameObject[19];
            for(int j = 0; j < newArray.Length; j++)
            {
                Vector3 pos = new Vector3(50f + i * 50f, 0f, 50f + j * 50f);
                pos.y = Terrain.activeTerrain.SampleHeight(pos);
                GameObject coil = Instantiate(teslaCoil, terrain.transform.TransformPoint(pos), Quaternion.identity, parent.transform);
                coil.transform.position = new Vector3(coil.transform.position.x, coil.transform.position.y + 12.9f, coil.transform.position.z);
                newArray[j] = coil;
            }
            field[i] = newArray;
        }

        // Sets the initial positions of the lightning bounces, and their timers
        positions = new int[2][];
        for(int i = 0; i < positions.Length; i++)
        {
            int[] position = new int[2];
            position[0] = Random.Range(0, 18);
            position[1] = Random.Range(0, 18);
            field[position[0]][position[1]].GetComponent<TeslaLightning>().Activate();
            positions[i] = position;
        }
        timers = new float[2];
        timers[0] = 5f;
        timers[1] = 4f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 0; i < positions.Length; i++)
        {
            // Ticks timer down, and if it hits 0 then it moves the lightning bolt
            timers[i] -= Time.deltaTime;
            if (timers[i] <= 0)
            {
                GameObject prev = field[positions[i][0]][positions[i][1]];
                // Deactivates the current tesla, and finds the next un-destroyed, deactivated tesla coil in a random set of directions and activates it
                field[positions[i][0]][positions[i][1]].GetComponent<TeslaLightning>().Deactivate();
                do
                {
                    int direction = Random.Range(0, 3);
                    switch (direction)
                    {
                        case 0: //+1 to x
                            if (positions[i][0] + 1 == 19)
                            {
                                positions[i][0]--;
                            }
                            else
                            {
                                positions[i][0]++;
                            }
                            break;
                        case 1: //+1 to z
                            if (positions[i][1] + 1 == 19)
                            {
                                positions[i][1]--;
                            }
                            else
                            {
                                positions[i][1]++;
                            }
                            break;
                        case 2: //-1 to x
                            if (positions[i][0] - 1 == -1)
                            {
                                positions[i][0]++;
                            }
                            else
                            {
                                positions[i][0]--;
                            }
                            break;
                        case 3: //-1 to z
                            if (positions[i][1] - 1 == -1)
                            {
                                positions[i][1]++;
                            }
                            else
                            {
                                positions[i][1]--;
                            }
                            break;
                    }
                } while (!field[positions[i][0]][positions[i][1]] || field[positions[i][0]][positions[i][1]].GetComponent<TeslaLightning>().IsActive());
                GameObject current = field[positions[i][0]][positions[i][1]];
                StartCoroutine(Lightning(prev, current, i));
                field[positions[i][0]][positions[i][1]].GetComponent<TeslaLightning>().Activate();
                timers[i] += 5f;
            }
        }
    }

    // Shoots a lightning bolt between the tops of the two tesla coils, and correctly carries out explosions
    IEnumerator Lightning(GameObject prev, GameObject current, int i)
    {
        GameObject bolt = Instantiate(lightning, prev.transform.position, Quaternion.identity);
        bolt.transform.GetChild(0).position = new Vector3(prev.transform.position.x, prev.transform.position.y + 13.5f, prev.transform.position.z);
        bolt.transform.GetChild(1).position = new Vector3(current.transform.position.x, current.transform.position.y + 13.5f, current.transform.position.z);
        bolt.transform.GetChild(2).position = new Vector3((prev.transform.position.x + current.transform.position.x) / 2, (prev.transform.position.y + current.transform.position.y) / 2,
            (prev.transform.position.z + current.transform.position.z) / 2);
        yield return new WaitForSeconds(1);
        // After the bolt has gone off, checks for whether the condition for explosion has been met, and if so activates the nearest valid tesla coil
        if (current.GetComponent<TeslaLightning>().Explode())
        {
            do
            {
                int direction = Random.Range(0, 3);
                switch (direction)
                {
                    case 0: //+1 to x
                        if (positions[i][0] + 1 == 19)
                        {
                            positions[i][0]--;
                        }
                        else
                        {
                            positions[i][0]++;
                        }
                        break;
                    case 1: //+1 to z
                        if (positions[i][1] + 1 == 19)
                        {
                            positions[i][1]--;
                        }
                        else
                        {
                            positions[i][1]++;
                        }
                        break;
                    case 2: //-1 to x
                        if (positions[i][0] - 1 == -1)
                        {
                            positions[i][0]++;
                        }
                        else
                        {
                            positions[i][0]--;
                        }
                        break;
                    case 3: //-1 to z
                        if (positions[i][1] - 1 == -1)
                        {
                            positions[i][1]++;
                        }
                        else
                        {
                            positions[i][1]--;
                        }
                        break;
                }
            } while (!field[positions[i][0]][positions[i][1]] || field[positions[i][0]][positions[i][1]].GetComponent<TeslaLightning>().IsActive());
            field[positions[i][0]][positions[i][1]].GetComponent<TeslaLightning>().Activate();
        }
        Destroy(bolt);
    }
}
