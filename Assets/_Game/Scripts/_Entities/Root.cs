using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour, IInteractable
{
    [Header("Components")]
    [SerializeField]
    private List<Root> adjacentRoots;

    #region IInteractable

    public void Interact()
    {

    }

    #endregion

}