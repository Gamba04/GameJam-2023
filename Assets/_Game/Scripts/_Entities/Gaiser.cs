using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gaiser : MonoBehaviour
{
    private bool state = true;

    public void SetState(bool state)
    {
        this.state = state; 
    }
    public bool GetState() 
    { 
        return state; 
    }
    private void GayserImpulse(Collider other)
    {
        IGaiserInteractable interactable = other.GetComponentInParent<IGaiserInteractable>();
        if (interactable != null && state)
        {
            interactable.Impulse();
        }
    }
    private void OnTriggerEnter(Collider other)
    {   
        GayserImpulse(other);
    }
    private void OnTriggerStay(Collider other)
    {
        GayserImpulse(other);
    }
}
