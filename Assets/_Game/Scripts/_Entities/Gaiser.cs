using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gaiser : MonoBehaviour
{
    [SerializeField]
    private Vector3 direction;

    private bool state = true;

    public void SetState(bool state)
    {
        this.state = state; 
    }
    public bool GetState() 
    { 
        return state; 
    }

    private void GaiserImpulse(Collider other)
    {
        IGaiserInteractable interactable = other.GetComponentInParent<IGaiserInteractable>();

        if (interactable != null && state)
        {
            interactable.Impulse(direction.normalized);
        }
    }

    private void OnTriggerEnter(Collider other)
    {   
        GaiserImpulse(other);
    }

    private void OnTriggerStay(Collider other)
    {
        GaiserImpulse(other);
    }
}
