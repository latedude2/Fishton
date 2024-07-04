using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostButtonHider : MonoBehaviour
{
    [SerializeField] List<GameObject> _objectsToHide = new List<GameObject>();

    private void Start()
    {
        //_SetAllObjectsTo(false);
    }

    private void _SetAllObjectsTo(bool state)
    {
        foreach (GameObject obj in _objectsToHide)
        {
            obj.SetActive(state);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H) && Input.GetKey(KeyCode.LeftControl))
        {
            //Show the host menu
            _SetAllObjectsTo(true);
        }
    }
}
