using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FishCounterUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text Text;

    void Start()
    {
        GameState.Instance.OnFishCounterUpdated += OnFishCountUpdate;
    }

    void OnValidate()
    {
        Text = GetComponent<TMP_Text>();
    }

    private void OnFishCountUpdate(int NewFishCount)
    {
        Text.text = NewFishCount.ToString();
    }
}
