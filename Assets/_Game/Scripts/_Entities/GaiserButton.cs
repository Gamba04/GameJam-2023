using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaiserButton : MonoBehaviour, IInteractable
{
    [SerializeField] 
    private Gaiser gaiser;

    public bool Enabled => true;

    public void Interact(Player player)
    {
        gaiser.SetState(!gaiser.GetState());
    }
}