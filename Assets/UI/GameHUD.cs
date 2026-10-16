using UnityEngine;
using UnityEngine.SceneManagement;

// HUD simple con OnGUI (sin dependencias de paquetes de UI).
public class GameHUD : MonoBehaviour
{
    [SerializeField] private PlayerInteraction playerInteraction;

    private void OnGUI()
    {
        GameManager gm = GameManager.Instance;
        if (gm == null) return;

        // Mira central
        GUIStyle cross = new GUIStyle(GUI.skin.label) { fontSize = 20, alignment = TextAnchor.MiddleCenter };
        GUI.Label(new Rect(Screen.width / 2f - 10, Screen.height / 2f - 10, 20, 20), "+", cross);

        if (!PlayerLook.InputLocked && playerInteraction != null && playerInteraction.IsLookingAtInteractable())
        {
            GUIStyle hint = new GUIStyle(GUI.skin.label) { fontSize = 16, alignment = TextAnchor.MiddleCenter };
            GUI.Label(new Rect(0, Screen.height / 2f + 20, Screen.width, 30), "[E] Interactuar", hint);
        }

        GUI.Label(new Rect(10, 10, 300, 25), "Llave: " + (gm.HasKey ? "Sí" : "No"));

        string msg = gm.CurrentMessage;
        if (!string.IsNullOrEmpty(msg))
        {
            GUIStyle box = new GUIStyle(GUI.skin.box) { fontSize = 16, wordWrap = true, alignment = TextAnchor.MiddleCenter };
            GUI.Box(new Rect(Screen.width / 2f - 300, Screen.height - 100, 600, 60), msg, box);
        }

        if (gm.EscapeCompleted)
        {
            GUIStyle win = new GUIStyle(GUI.skin.box) { fontSize = 36, alignment = TextAnchor.MiddleCenter };
            Rect r = new Rect(Screen.width / 2f - 250, Screen.height / 2f - 90, 500, 120);
            GUI.Box(r, "¡ESCAPE COMPLETADO!", win);
            if (GUI.Button(new Rect(Screen.width / 2f - 70, r.yMax + 10, 140, 35), "Reiniciar"))
            {
                PlayerLook.InputLocked = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
