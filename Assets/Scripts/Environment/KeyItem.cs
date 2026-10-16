using UnityEngine;

public class KeyItem : MonoBehaviour, IInteractable
{
    private void Update()
    {
        transform.Rotate(0f, 90f * Time.deltaTime, 0f, Space.World);
    }

    public void Interact()
    {
        GameManager.Instance.ObtainKey();
        gameObject.SetActive(false);
        Debug.Log("Llave obtenida.");
        GameManager.Instance.ShowMessage("Llave obtenida.");
    }
}
