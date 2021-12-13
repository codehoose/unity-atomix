using System;
using UnityEngine;

public class Clicked : MonoBehaviour
{
    public Vector3 direction;
    public event EventHandler<Vector3> Click;
    
    private void OnMouseUpAsButton()
    {
        Click?.Invoke(this, direction);
    }
}