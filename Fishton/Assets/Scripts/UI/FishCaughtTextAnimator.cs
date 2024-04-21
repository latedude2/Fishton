using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FishCaughtTextAnimator : MonoBehaviour
{
    [SerializeField]
    private Gradient ColorGradient;

    [SerializeField]
    private float Offset;

    [SerializeField]
    private float Speed;

    [SerializeField]
    private TMP_Text Text;


    private void OnValidate()
    {
        Text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        float LeftTime = (Mathf.Sin(Time.time * Speed) + 1.0f) / 2.0f;
        float RightTime  = (Mathf.Sin(Time.time * Speed + Offset) + 1.0f) / 2.0f;

        Color LeftColor = ColorGradient.Evaluate(LeftTime);
        Color RightColor = ColorGradient.Evaluate(RightTime);

        var Gradient = Text.colorGradient;
        Gradient.topLeft = LeftColor;
        Gradient.bottomLeft = LeftColor;
        Gradient.topRight = RightColor;
        Gradient.bottomRight = RightColor;
        Text.colorGradient = Gradient;
    }
}
