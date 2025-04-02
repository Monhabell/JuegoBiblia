using UnityEngine;

public class DoorScript : MonoBehaviour, IInteractable
{

    public Transform pivot;  

    public void Interact()
    {
        // Rotamos el pivote (y no la puerta directamente)
        pivot.Rotate(0, 90, 0); // Rota alrededor del eje Y del objeto vacío
        
    }

    
}
