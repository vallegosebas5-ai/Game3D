using UnityEngine;

// Pista: una nota en la pared que revela el código.
public class ClueNote : MonoBehaviour, IInteractable
{
    [SerializeField] [TextArea] private string clueText = "\"Cuenta del uno al cuatro y la salida estará más cerca.\"";

    public void Interact()
    {
        GameManager.Instance.ShowMessage("Pista: " + clueText, 5f);
        Debug.Log("Pista leída.");
    }
}
