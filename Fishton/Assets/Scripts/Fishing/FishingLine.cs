using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class FishingLine : MonoBehaviour
{
    [SerializeField] float _extendCurvature;
    [SerializeField] int _smoothness = 10;
    public float speed = 300f;

    Trajectory _trajectory;
    LineRenderer _lineRenderer;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();   
    }

    public void ExtendLine(Vector3 start, Vector3 end, float progress)
    {
        if (_trajectory == null)
        {
            _trajectory = new Trajectory(start, end, _extendCurvature);
        }

        Vector3 endPoint = _trajectory.GetPointOnTrajectory(progress);

        _lineRenderer.SetPositions(new Vector3[] { 
            start,
            endPoint
        });
    }

    public void StartTracking()
    {

    }
}
