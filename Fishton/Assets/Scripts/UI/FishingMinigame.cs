using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishingMinigame : MonoBehaviour
{
    [SerializeField]
    private Image Scrollbar;

    [SerializeField]
    private Image FishImage;

    [SerializeField]
    private float InputAcceleration = 0.5f;

    [SerializeField]
    private float NoInputAcceleration = -0.25f;

    [SerializeField]
    private float MaxVelocityPos = 0.1f;

    [SerializeField]
    private float MaxVelocityNeg = -0.1f;

    [SerializeField]
    private float FishSpeed = 2.0f;

    [SerializeField]
    private float MinFishPosInterval = 0.2f;

    [SerializeField]
    private float MaxFishPosInterval = 2.0f;

    public FishDefinition Fish { get; set; }

    private bool ShouldUpdateFishPosition => Time.time >= LastFishPositionUpdate + FishPositionUpdateWaitDuration;

    private float HandleSize;
    private float Position;
    
    private float Velocity;
    private float FishTargetPosition;
    private float LastFishPositionUpdate;
    private float FishPositionUpdateWaitDuration;

    private void Awake()
    {
        InitializeData();
    }

    private void InitializeData()
    {
        HandleSize = GetHandleScaleFromDifficulty();
        Position = 0.5f;
        Velocity = 0;
        SelectNewFishPosition();
    }

    private void Update()
    {
        HandleAcceleration();
        UpdatePosition();
        SetPosition(Position);
        SetFishPosition(FishTargetPosition);

        if(ShouldUpdateFishPosition)
            SelectNewFishPosition();
    }

    private void HandleAcceleration()
    {
        float Acceleration = 0.0f;
        if (Input.GetMouseButton(0))
        {
            Acceleration += InputAcceleration;
        }
        else
        {
            if(Position <= float.Epsilon)
            {
                Velocity = 0.0f;
                return;
            }

            Acceleration += NoInputAcceleration;
        }

        Velocity += Acceleration * Time.deltaTime;
        Velocity = Mathf.Clamp(Velocity, MaxVelocityNeg, MaxVelocityPos);
    }

    private void UpdatePosition()
    {
        Position = Mathf.Clamp(Position + Velocity, 0.0f, 1.0f);
    }

    private void SetPosition(float PercentagePosition)
    {
        PercentagePosition = Mathf.Clamp(PercentagePosition, 0.0f, 1.0f);

        // Percentage of the bar that we can work with
        float WorkArea = Mathf.Clamp(1.0f - HandleSize, 0.0f, 1.0f);
        float Offset = WorkArea * PercentagePosition;

        RectTransform RectTrans = Scrollbar.rectTransform;
        
        Vector2 Min = RectTrans.anchorMin;
        Min.y = Offset;
        RectTrans.anchorMin = Min;

        Vector2 Max = RectTrans.anchorMax;
        Max.y = Offset + HandleSize;
        RectTrans.anchorMax = Max;

        RectTrans.anchoredPosition = Vector2.zero;
    }

    private void SetFishPosition(float TargetPosition)
    {
        TargetPosition = Mathf.Clamp(TargetPosition, 0.0f, 1.0f);

        RectTransform RectTrans = FishImage.rectTransform;
        RectTransform ParentRect = RectTrans.parent.GetComponent<RectTransform>();

        float Height = ParentRect.rect.height - RectTrans.rect.height;

        float CurrentPos = RectTrans.anchoredPosition.y / Height; 
        float NewPos = Mathf.Lerp(CurrentPos, TargetPosition, FishSpeed * Time.deltaTime);

        Vector2 Pos = RectTrans.anchoredPosition;
        Pos.y = NewPos * Height;
        RectTrans.anchoredPosition = Pos;
    }

    private void SelectNewFishPosition()
    {
        FishTargetPosition = Random.Range(0.0f, 1.0f);
        LastFishPositionUpdate = Time.time;
        FishPositionUpdateWaitDuration = Random.Range(MinFishPosInterval, MaxFishPosInterval);
    }

    private float GetHandleScaleFromDifficulty()
    {
        return 0.2f;
    }
}
