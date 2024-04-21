using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor.Rendering;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AnimateCaughtFish : MonoBehaviour
{
    public GameObject CatchingPlayer;
    private float lerpPercentage = 0.0f;

    private float curvature = 2.0f;
    private Vector3 startPosition = new Vector3();
    private Vector3 endPosition = new Vector3();

    private bool hasMovingTarget = false;
    private Transform movingTransformStart;
    private Transform movingTransformEnd;


    private void Start()
    {
        startPosition = transform.position;
        endPosition = CatchingPlayer.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, 60 * Time.deltaTime);
        transform.position = GetPointOnTrajectory(lerpPercentage);
        lerpPercentage++;

        if (lerpPercentage >= 100)
        {
            Destroy(gameObject);
        }
    }

    public Vector3 GetIntermediatePoint()
    {
        Vector3 startPoint = startPosition;
        Vector3 endPoint = endPosition;

        if (hasMovingTarget)
        {
            startPoint = movingTransformStart.position;
            endPoint = movingTransformEnd.position;
        }

        Vector3 intermediatePoint = startPoint + (endPoint - startPoint) / 2;
        intermediatePoint.y += curvature * (Mathf.Abs(Mathf.Abs(endPoint.y) - Mathf.Abs(startPoint.y)) + 1);
        return intermediatePoint;
    }

    /// <summary>
    /// Returns the position along the trajectory corresponding to the given 0-100 percentage
    /// </summary>
    /// <param name="percentage">Ranging from 0 to 100</param>
    public Vector3 GetPointOnTrajectory(float percentage)
    {
        if (percentage > 100) percentage = 100;

        percentage = percentage / 100;

        Vector3 intermediatePoint = GetIntermediatePoint();
        Vector3 startPoint = startPosition;
        Vector3 endPoint = endPosition;

        if (hasMovingTarget)
        {
            startPoint = movingTransformStart.position;
            endPoint = movingTransformEnd.position;
        }

        //Calculate the steps of the two sides
        Vector3 upPoint = startPoint + (intermediatePoint - startPoint) * percentage;
        Vector3 downPoint = intermediatePoint + (endPoint - intermediatePoint) * percentage;

        Vector3 intermediateLine = downPoint - upPoint;

        return upPoint + intermediateLine * percentage;
    }
}
