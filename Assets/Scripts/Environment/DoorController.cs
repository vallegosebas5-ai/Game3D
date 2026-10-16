using UnityEngine;

public class DoorController : MonoBehaviour, IInteractable
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform hinge;   // Usado si no hay Animator

    private bool opened;

    public void Interact()
    {
        if (!GameManager.Instance.HasKey)
        {
            Debug.Log("Necesitas la llave.");
            GameManager.Instance.ShowMessage("Necesitas la llave.");
            return;
        }

        if (opened) return;
        opened = true;

        if (animator != null)
        {
            animator.SetTrigger("Open");
        }
        else if (hinge != null)
        {
            hinge.localRotation = Quaternion.Euler(0f, -100f, 0f);
        }

        Debug.Log("Puerta abierta. ¡Escape completado!");
        GameManager.Instance.CompleteEscape();
    }
}
