using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AddedFishUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform Trans;

    [SerializeField]
    private TMP_Text Text;

    [SerializeField]
    private float Speed;

    [SerializeField]
    private float LifeTime = 3.0f;

    [SerializeField]
    private AnimationCurve SpeedCurve;

    private float StartTime;

    void OnValidate()
    {
        Trans = GetComponent<RectTransform>();
        Text = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        StartTime = Time.time;
    }

    private void Update()
    {
        float TimeSinceStart = Time.time - StartTime;
        float CurvedSpeed = SpeedCurve.Evaluate(TimeSinceStart) * Speed;
        Trans.anchoredPosition += new Vector2(0, Time.deltaTime * CurvedSpeed);

        if(TimeSinceStart >= LifeTime)
            Destroy(gameObject);
    }

    public void SetAmount(int Amount)
    {
        Text.text = $"+{Amount}";
    }
}
