using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class FishCounterUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text Text;
    
    [SerializeField]
    private UnityEvent OnUpdate;

    [SerializeField]
    private RectTransform SpawnPosition;

    [SerializeField]
    private AddedFishUI AddedFishPrefab;

    private int AmountAdded;
    private int TotalFish;

    void Start()
    {
        GameState.Instance.OnFishCounterUpdated += OnFishCountUpdate;
        StartCoroutine("CheckAdded");
    }

    void OnValidate()
    {
        if(Text == null)
            Text = GetComponent<TMP_Text>();
    }

    private void OnFishCountUpdate(int NewFishCount)
    {
        TotalFish = NewFishCount;
        AmountAdded++;
    }

    IEnumerator CheckAdded()
    {
        yield return new WaitForSeconds(1);
        if(AmountAdded > 0)
            UpdateFishCounter();

        AmountAdded = 0;
        StartCoroutine("CheckAdded");
    }

    private void UpdateFishCounter()
    {
        Text.text = TotalFish.ToString();
        OnUpdate.Invoke();

        AddedFishUI NewAddedFishUI = Instantiate(AddedFishPrefab);
        NewAddedFishUI.transform.SetParent(transform);
        NewAddedFishUI.gameObject.GetComponent<RectTransform>().position = SpawnPosition.position;
        NewAddedFishUI.SetAmount(AmountAdded);
    }
}
