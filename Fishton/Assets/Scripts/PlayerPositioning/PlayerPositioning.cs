using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositioning : MonoBehaviour
{
    public GameObject prefab; // Assign your GameObject in the Inspector
    public int numberOfObjects = 100;
    public float MajorAxis = 2f; 
    public float MinorAxis = 1f; 
    public int rows = 4; // Number of ellipses
    public float distanceBetweenRows = 1.3f; // Distance between ellipses
    
    public List<GameObject> SpawnPoints;
    public static PlayerPositioning instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        int ObjectsPerRow = numberOfObjects / rows; // Number of objects per ellipse
        for (int j = 0; j < rows; j++) // For 4 ellipses
        {
            for (int i = 0; i < ObjectsPerRow; i++)
            {
                float angle = i * Mathf.PI * 2 / ObjectsPerRow + j * 25f;
                Vector3 newPos = new Vector3(Mathf.Cos(angle) * MajorAxis, 0, Mathf.Sin(angle) * MinorAxis); // Position on the ellipse
                GameObject go = Instantiate(prefab, newPos, Quaternion.identity); // Instantiate a new GameObject
                PlayerPositioning.instance.SpawnPoints.Add(go);

                go.transform.LookAt(Vector3.zero); // Make the GameObject look at the center of the ellipse
            }
            MajorAxis = MajorAxis + distanceBetweenRows;
            MinorAxis = MinorAxis + distanceBetweenRows;
        }
    }


    //Server-only
    static public int GetFirstAvailableSpawnPoint()
    {
        for (int i = 0; i < PlayerPositioning.instance.SpawnPoints.Count; i++)
        {
            if (!PlayerPositioning.instance.SpawnPoints[i].GetComponent<SpawnPoint>().isOccupied)
            {
                return i;
            }
        }
        return -1;
    } 


    static public SpawnPoint GetSpawnPoint(int index)
    {
        return PlayerPositioning.instance.SpawnPoints[index].GetComponent<SpawnPoint>();
    }
}
