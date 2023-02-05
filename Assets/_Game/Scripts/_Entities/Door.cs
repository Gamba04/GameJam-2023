using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    [SerializeField]
    private LevelTag targetLevel;

    public event Action<LevelTag> onLoadLevel;

    private bool activated;

    private void OnTriggerEnter(Collider other)
    {
        if (activated) return;

        Player target = other.GetComponentInParent<Player>();

        if (target != null)
        {
            activated = true;

            onLoadLevel?.Invoke(targetLevel);
        }
    }
}