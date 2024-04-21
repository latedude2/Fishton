using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class AnimateCaughtFish : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve animationCurve;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, animationCurve.Evaluate(Time.time), transform.position.z);
    }
}
