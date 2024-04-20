using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void OnFishingMinigameFinishedDelegate(bool WonGame);

public class FishingMinigame : MonoBehaviour
{
    [SerializeField]
    private Image Scrollbar;

    [SerializeField]
    private Image FishImage;

    [SerializeField]
    private Image ProgressBar;

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

    [SerializeField]
    private float FishSize = 0.1f;

    [SerializeField]
    private float StartProgress = 0.5f;

    [SerializeField]
    private float GainPointsSpeed = 2.0f;

    [SerializeField]
    private float LosePointsSpeed = 3.0f;

    public FishDefinition Fish { get; set; }
    public OnFishingMinigameFinishedDelegate OnGameFinished;

    private bool ShouldUpdateFishPosition => Time.time >= LastFishPositionUpdate + FishPositionUpdateWaitDuration;

    private RectTransform HandleRect => Scrollbar.rectTransform;
    private float HandleMinPos => HandleRect.anchorMin.y;
    private float HandleMaxPos => HandleRect.anchorMax.y;

    private RectTransform FishRect => FishImage.rectTransform;
    private float FishMinPos => FishRect.anchorMin.y;
    private float FishMaxPos => FishRect.anchorMax.y;

    private bool IsWithinFish =>
        // Handle is below, but overlapping fish
        HandleMinPos < FishMinPos && HandleMaxPos > FishMinPos ||
        // Handle is above fish, but being overlapped
        HandleMaxPos > FishMinPos && HandleMinPos < FishMaxPos;

    private float HandleSize;
    private float HandlePosition;
    private float FishPosition;
    
    private float Velocity;
    private float FishTargetPosition;
    private float LastFishPositionUpdate;
    private float FishPositionUpdateWaitDuration;
    private float RequiredPoints;
    private float CurrentPoints;

    private void Awake()
    {
        InitializeData();
    }

    private void InitializeData()
    {
        HandleSize = GetHandleScaleFromDifficulty();
        HandlePosition = 0.5f;
        Velocity = 0;
        RequiredPoints = GetRequiredPoints();
        CurrentPoints = RequiredPoints * StartProgress;
        SelectNewFishPosition();
    }

    private void Update()
    {
        HandleAcceleration();
        UpdatePosition();
        SetPosition(HandlePosition, HandleRect, HandleSize);

        FishPosition = Mathf.Lerp(FishPosition, FishTargetPosition, FishSpeed * Time.deltaTime);
        SetPosition(FishPosition, FishRect, FishSize);

        if(ShouldUpdateFishPosition)
            SelectNewFishPosition();

        HandleGrantPoints();
    }

    private void HandleAcceleration()
    {
        float Acceleration = 0.0f;
        if (Input.GetMouseButton(0))
        {
            Acceleration = InputAcceleration;
        }
        else
        {
            if(HandlePosition <= float.Epsilon)
            {
                Velocity = 0.0f;
                return;
            }

            if(1.0f - HandlePosition <= float.Epsilon)
                Velocity = 0.0f;

            Acceleration = NoInputAcceleration;
        }

        Velocity += Acceleration * Time.deltaTime;
        Velocity = Mathf.Clamp(Velocity, MaxVelocityNeg, MaxVelocityPos);
    }

    private void UpdatePosition()
    {
        HandlePosition = Mathf.Clamp(HandlePosition + Velocity, 0.0f, 1.0f);
    }

    private void SetPosition(float PercentagePosition, RectTransform RectTrans, float Size)
    {
        PercentagePosition = Mathf.Clamp(PercentagePosition, 0.0f, 1.0f);

        // Percentage of the bar that we can work with
        float WorkArea = Mathf.Clamp(1.0f - Size, 0.0f, 1.0f);
        float Offset = WorkArea * PercentagePosition;
        
        Vector2 Min = RectTrans.anchorMin;
        Min.y = Offset;
        RectTrans.anchorMin = Min;

        Vector2 Max = RectTrans.anchorMax;
        Max.y = Offset + Size;
        RectTrans.anchorMax = Max;

        RectTrans.anchoredPosition = Vector2.zero;
    }

    private void SelectNewFishPosition()
    {
        FishTargetPosition = Random.Range(0.0f, 1.0f);
        LastFishPositionUpdate = Time.time;
        FishPositionUpdateWaitDuration = Random.Range(MinFishPosInterval, MaxFishPosInterval);
    }

    private void HandleGrantPoints()
    {
        if(IsWithinFish)
        {
            CurrentPoints += Time.deltaTime * GainPointsSpeed;
        }
        else
        {
            CurrentPoints -= Time.deltaTime * LosePointsSpeed;
        }
        
        ProgressBar.fillAmount = CurrentPoints / RequiredPoints;

        if(1.0f - ProgressBar.fillAmount <= float.Epsilon)
            HandleGameFinished(true);
        else if(ProgressBar.fillAmount <= float.Epsilon)
            HandleGameFinished(false);    
    }

    private void HandleGameFinished(bool Won)
    {
        OnGameFinished.Invoke(Won);
    }

    private float GetHandleScaleFromDifficulty()
    {
        return 0.3f;
    }

    private float GetRequiredPoints()
    {
        return 10;
    }
}
