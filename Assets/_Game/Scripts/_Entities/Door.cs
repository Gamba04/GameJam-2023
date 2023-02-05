using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    [SerializeField]
    private LevelTag targetLevel;

    [Space]
    [SerializeField]
    private UnityEvent doorEvent;

    private void OnTriggerEnter(Collider other)
    {
        doorEvent?.Invoke();
    }
}