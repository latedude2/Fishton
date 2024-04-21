using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishySpinner : MonoBehaviour
{
    [SerializeField]
    private float Speed = 1.0f;

    void Update()
    {
        transform.localScale = new Vector3(Mathf.Sin(Time.time * Speed), 1.0f, 1.0f);
    }
}
