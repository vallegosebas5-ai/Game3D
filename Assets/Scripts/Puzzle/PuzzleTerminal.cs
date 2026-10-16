using UnityEngine;

// Panel con teclado numérico: al interactuar abre la ventana para escribir el código.
public class PuzzleTerminal : MonoBehaviour, IInteractable
{
    [SerializeField] private PuzzleManager puzzleManager;

    private bool open;
    private bool solved;
    private string input = "";

    public void Interact()
    {
        if (solved)
        {
            GameManager.Instance.ShowMessage("El panel ya fue resuelto.");
            return;
        }

        open = true;
        input = "";
        PlayerLook.InputLocked = true;
        PlayerLook.LockCursor(false);
    }

    private void Close()
    {
        open = false;
        PlayerLook.InputLocked = false;
        PlayerLook.LockCursor(true);
    }

    private void Update()
    {
        if (!open) return;

        if (Input.GetKeyDown(KeyCode.Escape)) { Close(); return; }
        if (Input.GetKeyDown(KeyCode.Backspace) && input.Length > 0) input = input.Substring(0, input.Length - 1);
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) Submit();

        foreach (char c in Input.inputString)
        {
            if (char.IsDigit(c) && input.Length < 4) input += c;
        }
    }

    private void Submit()
    {
        if (puzzleManager != null && puzzleManager.ValidateCode(input))
        {
            solved = true;
            Close();
        }
        else
        {
            input = "";
        }
    }

    private void OnGUI()
    {
        if (!open) return;

        float w = 260f, h = 230f;
        Rect r = new Rect((Screen.width - w) / 2f, (Screen.height - h) / 2f, w, h);
        GUI.Box(r, "PANEL DE CÓDIGO");

        GUIStyle big = new GUIStyle(GUI.skin.label) { fontSize = 32, alignment = TextAnchor.MiddleCenter };
        GUI.Label(new Rect(r.x, r.y + 30, w, 50), input.PadRight(4, '_'), big);

        float bx = r.x + 20, by = r.y + 90, bw = 70, bh = 28;
        for (int i = 1; i <= 9; i++)
        {
            int col = (i - 1) % 3, row = (i - 1) / 3;
            if (GUI.Button(new Rect(bx + col * (bw + 5), by + row * (bh + 4), bw, bh), i.ToString()) && input.Length < 4)
                input += i;
        }
        if (GUI.Button(new Rect(bx, by + 3 * (bh + 4), bw, bh), "Borrar")) input = "";
        if (GUI.Button(new Rect(bx + bw + 5, by + 3 * (bh + 4), bw, bh), "0") && input.Length < 4) input += "0";
        if (GUI.Button(new Rect(bx + 2 * (bw + 5), by + 3 * (bh + 4), bw, bh), "OK")) Submit();
    }
}
