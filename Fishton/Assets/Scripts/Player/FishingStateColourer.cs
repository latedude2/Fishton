using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class FishingStateColourer : NetworkBehaviour
{
    public SkinnedMeshRenderer Renderer;
    //networked int reflecting color
    private NetworkVariable<int> ColorHueInt = new NetworkVariable<int>(0);

    private void Start()
    {
        //Random nice color int
        if(!IsHost)
            return;

        ColorHueInt.Value = Random.Range(0, 255);
        OnColorChanged(0, ColorHueInt.Value);
    }

    override public void OnNetworkSpawn()
    {
        OnColorChanged(0, ColorHueInt.Value);
        if(IsLocalPlayer == false)
        {
            ColorHueInt.OnValueChanged += OnColorChanged;
        }
    }

    private void OnColorChanged(int OldValue, int NewValue)
    {
        SetColor(Color.HSVToRGB(NewValue / 255f, 1, 1));
    }
    

    private void SetColor(Color NewColor)
    {
        Renderer.material.color = NewColor;
    }
}
