using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactionDistance = 2f;
    [SerializeField] private Camera playerCamera;

    private void Update()
    {
        if (PlayerLook.InputLocked)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    private void TryInteract()
    {
        if (playerCamera == null)
        {
            Debug.LogWarning("Player camera is not assigned.");
            return;
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
        {
            IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();

            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }

    // Usado por la UI para mostrar "Presiona E".
    public bool IsLookingAtInteractable()
    {
        if (playerCamera == null) return false;
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        return Physics.Raycast(ray, out RaycastHit hit, interactionDistance)
               && hit.collider.GetComponentInParent<IInteractable>() != null;
    }
}
