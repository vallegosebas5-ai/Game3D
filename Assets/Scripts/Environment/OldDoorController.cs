using UnityEngine;

// Controlador antiguo de puerta. Se conserva SOLO para el Caso 09 (Delete/Modify):
// un integrante lo elimina y otro lo modifica. No está asignado en la escena.
public class OldDoorController : MonoBehaviour
{
    public bool isOpen;

    public void Toggle()
    {
        isOpen = !isOpen;
        Debug.Log("OldDoorController: puerta " + (isOpen ? "abierta" : "cerrada"));
    }
}
