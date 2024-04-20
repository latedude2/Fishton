using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingWorldComponent : MonoBehaviour
{   
    [SerializeField]
    private float PotentialHitRadius;

    [SerializeField]
    private FishingBob FishingBobPrefab;

    public EventManager Events { get; private set; }

    private Vector3 FinalHitPosition;

    private Vector3? StartTracePosition;
    private float StartTraceTime;

    private void Awake()
    {
        Events = EventManager.Get(gameObject);
        Events.OnFishingStateChanged += OnStateChanged;
    }

    private void OnStateChanged(FishEncounterState NewState)
    {
        if(NewState != FishEncounterState.Throwing)
            return;

        StartCoroutine("StartThrow");
    }

    private IEnumerator StartThrow()
    {
        Vector3 PlayerPosition = transform.position;
        Vector3 HighPosition = PlayerPosition + Vector3.up * 5;
        Vector3 HighPositionOffset = HighPosition + transform.forward * PotentialHitRadius * 1.5f;
        Vector3 NormalizedOffset = new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f));
        Vector3 Offset = NormalizedOffset * PotentialHitRadius;
        Vector3 StartPosition = HighPositionOffset + Offset;
        
        // Debug gizmo
        StartTracePosition = HighPositionOffset;
        StartTraceTime = Time.time;
    
        Ray ray = new Ray()
        {
            origin = StartPosition,
            direction = Vector3.down,
        };
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.magenta, 5.0f);

        RaycastHit Hit = new RaycastHit();
        bool HitAnything = Physics.Raycast(ray.origin, ray.direction * 10, out Hit);

        if(!HitAnything)
        {
            Debug.LogError("Couldn't find a suitable fishing location");
            yield break;
        }
        
        FinalHitPosition = Hit.point;

        yield return new WaitForSeconds(0.5f);

        FishingBob FishingBob = Instantiate(FishingBobPrefab, FinalHitPosition, Quaternion.identity);
        FishingBob.Initialize(Events);
    }

    private void OnDrawGizmos()
    {
        if(!StartTracePosition.HasValue || Time.time > StartTraceTime + 5.0f)
            return;

        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(StartTracePosition.Value, PotentialHitRadius);
    }
}
