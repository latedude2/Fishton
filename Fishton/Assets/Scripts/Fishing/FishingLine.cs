using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class FishingLine : MonoBehaviour
{
    [SerializeField] float _extendCurvature;
    [SerializeField] int _smoothness = 10;
    public float speed = 300f;

    Transform _trackingTargetStart;
    Transform _trackingTargetEnd;
    bool _isTracking = false;

    Trajectory _trajectory;
    LineRenderer _lineRenderer;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();   
    }

    private void Update()
    {
        if (_lineRenderer == null || _trackingTargetStart == null || _trackingTargetEnd == null)
        {
            Destroy(gameObject);
            return;
        }

        if (_isTracking)
        {
            _lineRenderer.SetPositions(new Vector3[] {
            _trackingTargetStart.position,
            _trackingTargetEnd.position
            });
        }
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

    public void StartTracking(Transform trackingTargetStart, Transform trackingTargetEnd)
    {
        _trackingTargetStart = trackingTargetStart;
        _trackingTargetEnd = trackingTargetEnd;
        _isTracking = true;
    }
}
