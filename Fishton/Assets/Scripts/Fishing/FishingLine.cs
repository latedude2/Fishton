using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class FishingLine : MonoBehaviour
{
    [SerializeField] private Transform _startTransform;
    [SerializeField] private Transform _endTransform;

    public void InitializeLine(Transform start, Transform end)
    {

    }
}
