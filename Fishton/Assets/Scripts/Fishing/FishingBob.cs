using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingBob : MonoBehaviour
{
    private EventManager Events;
    private GameObject RendererObject;

    public void Initialize(EventManager OwningEventManager)
    {
        Events = OwningEventManager;
        Events.OnFishEncounterFinished += OnFinished;
    }

    private void Awake()
    {
        RendererObject = GetComponentInChildren<Renderer>().gameObject;
    }

    private void OnFinished()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Events.OnFishEncounterFinished -= OnFinished;
    }

    private void Update()
    {
        RendererObject.transform.localPosition = new Vector3(0.0f, Mathf.Sin(Time.time) / 6.0f, 0.0f);
    }
}
