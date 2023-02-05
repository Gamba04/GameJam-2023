using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaiserButton : MonoBehaviour, IInteractable
{
    [SerializeField] 
    private Gaiser gaiser;

    public void Interact()
    {
        gaiser.SetState(!gaiser.GetState());
    }
}