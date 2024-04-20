using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] string _animateOutTriggerName = "AnimateOut";

    Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void AnimateIn()
    {

    }

    public void AnimateOut()
    {
        _animator.SetTrigger(_animateOutTriggerName);
    }
}
