using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAlert : MonoBehaviour
{
    [SerializeField] GameObject _warningSign;

    void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
        transform.LookAt(Camera.main.transform.position);
        HideWarning();
    }

    public void ShowWarning()
    {
        _warningSign.SetActive(true);
    }

    public void HideWarning()
    {
        _warningSign.SetActive(false);
    }
}
