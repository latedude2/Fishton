using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Audio;

public class AudioPlayer : NetworkBehaviour
{
    private EventManager Events { get; set; }
    private FishEncounterState LastSeenState;


    public AudioSource Audio;
    public AudioClip FishCaught;
    public AudioResource FishHooked;
    public AudioClip FishFailed;
    public AudioClip StartFishing;

    public AudioMixerGroup MixerGroupForOthers;
    


    private void Awake()
    {
        Audio = GetComponent<AudioSource>();
        Events = EventManager.Get(gameObject);
        Events.OnFishingStateChanged += OnFishingStateChanged;
    }

    override public void OnNetworkSpawn()
    {
        if(IsLocalPlayer)
        {
            Audio.outputAudioMixerGroup = MixerGroupForOthers;
        }
    }

    private void OnFishingStateChanged(FishEncounterState NewState)
    {
        PlayClip(GetAudioFromState(NewState));
    }

    private AudioResource GetAudioFromState(FishEncounterState NewState)
    {
        LastSeenState = NewState;
        switch (NewState)
        {
            case FishEncounterState.Throwing:
                return StartFishing;
            case FishEncounterState.Idle:
                return null;
            case FishEncounterState.Hooked:
                return FishHooked;
            case FishEncounterState.Caught:
                return FishCaught;
            case FishEncounterState.Failed:
                return FishFailed;
            default:
                return null;
        }
    }

    private void PlayClip(AudioResource resource)
    {
        if(resource == null) return;
        if(resource.GetType() == typeof(AudioClip))
        {
            Audio.PlayOneShot((AudioClip)resource);
            return;
        }
        Audio.resource = resource;
        Audio.Play();
    }

}
