using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingWorldComponent : MonoBehaviour
{   
    [SerializeField]
    private float PotentialHitRadius;

    [SerializeField] private FishingBob FishingBobPrefab;
    [SerializeField] private GameObject _fishingLinePrefab;
    [SerializeField] float _lineAppearDelay = 0.3f;

    [SerializeField] Transform _fishingRodEndPoint;

    public EventManager Events { get; private set; }

    GameObject _spawnedBobObject;
    FishingLine _fishingLine;

    private Vector3 FinalHitPosition;

    private Vector3? StartTracePosition;
    private float StartTraceTime;

    private void Awake()
    {
        Events = EventManager.Get(gameObject);
        Events.OnFishingStateChanged += OnStateChanged;
        Events.OnFishingMinigameFinished += OnMinigameFinished;
    }

    private void OnDestroy()
    {
        if (_spawnedBobObject != null)
            Destroy(_spawnedBobObject);

        if (_fishingLine != null)
            Destroy(_fishingLine.gameObject);
    }

    private void OnMinigameFinished(bool DidSucceed)
    {
        //Delete the bob
        Destroy(_spawnedBobObject);
        Destroy(_fishingLine?.gameObject);
    }

    private void OnStateChanged(FishEncounterState NewState)
    {
        if ((NewState & FishEncounterState.Finished) == NewState)
        {
            //Delete the bob
            Destroy(_spawnedBobObject);
            Destroy(_fishingLine?.gameObject);
        }

        if (NewState != FishEncounterState.Throwing)
            return; 
        StartCoroutine(StartThrow());
    }

    private IEnumerator StartThrow()
    {
        if(_spawnedBobObject)
            Destroy(_spawnedBobObject);
        Vector3 PlayerPosition = transform.position;
        Vector3 HighPosition = PlayerPosition + Vector3.up * 5;
        Vector3 HighPositionOffset = HighPosition + transform.forward * PotentialHitRadius * 3.0f;
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

        yield return new WaitForSecondsRealtime(_lineAppearDelay);

        //Spawn a line
        GameObject lineObj = Instantiate(_fishingLinePrefab);
        _fishingLine = lineObj.GetComponent<FishingLine>();

        //Throw the bob into water along a trajectory
        float progress = 0f;
        while (progress <= 100f)
        {
            Vector3 startPosition = _fishingRodEndPoint.position;
            Vector3 endPosition = FinalHitPosition;

            _fishingLine.ExtendLine(startPosition, endPosition, progress);

            progress += _fishingLine.speed * Time.deltaTime;
            yield return null;
        }

        FishingBob FishingBob = Instantiate(FishingBobPrefab, FinalHitPosition, Quaternion.identity);
        FishingBob.Initialize(Events);
                
        //Keep track of the object so we can delete it later when the encounter is done
        _spawnedBobObject = FishingBob.gameObject;
        _fishingLine.StartTracking(_fishingRodEndPoint, _spawnedBobObject.transform);
        yield return new WaitForSeconds(0.5f);

    }

    private void OnDrawGizmos()
    {
        if(!StartTracePosition.HasValue || Time.time > StartTraceTime + 5.0f)
            return;

        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(StartTracePosition.Value, PotentialHitRadius);
    }
}
