using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private string correctCode = "1234";
    [SerializeField] private ChestController chest;

    public bool ValidateCode(string code)
    {
        bool valid = code == correctCode;

        if (valid)
        {
            Debug.Log("Código correcto.");
            GameManager.Instance.ShowMessage("Código correcto. El cofre se desbloqueó.");

            if (chest != null)
            {
                chest.Unlock();
            }
        }
        else
        {
            Debug.Log("Código incorrecto.");
            GameManager.Instance.ShowMessage("Código incorrecto.");
        }

        return valid;
    }
}
