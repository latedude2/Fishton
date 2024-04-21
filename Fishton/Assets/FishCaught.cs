using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishCaught : MonoBehaviour
{
    [SerializeField]
    private GameObject fish;


    public void CatchFish(GameObject fish)
    {
        GameObject fishInstance = Instantiate(fish, new Vector3(0, 20, 0), Quaternion.identity);
    }
}
