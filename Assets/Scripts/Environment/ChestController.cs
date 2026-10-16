using UnityEngine;

public class ChestController : MonoBehaviour, IInteractable
{
    [SerializeField] private bool unlocked;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform lid;          // Usado si no hay Animator
    [SerializeField] private GameObject keyInside;   // Llave que aparece al abrir

    private bool opened;

    public void Unlock()
    {
        unlocked = true;
        Debug.Log("Cofre desbloqueado.");
    }

    public void Interact()
    {
        if (!unlocked)
        {
            Debug.Log("El cofre está cerrado.");
            GameManager.Instance.ShowMessage("El cofre está cerrado. Necesita un código.");
            return;
        }

        if (opened) return;
        opened = true;

        if (animator != null)
        {
            animator.SetTrigger("Open");
        }
        else if (lid != null)
        {
            lid.localRotation = Quaternion.Euler(110f, 0f, 0f);
        }

        if (keyInside != null)
        {
            keyInside.SetActive(true);
        }

        Debug.Log("Cofre abierto.");
        GameManager.Instance.ShowMessage("Cofre abierto. ¡Hay una llave dentro!");
    }
}
