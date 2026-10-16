using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool HasKey { get; private set; }
    public bool EscapeCompleted { get; private set; }

    // Contador usado en el Caso 16 (Accept Both): debe ser 1 al iniciar.
    public int InitializeCount { get; private set; }

    private string message;
    private float messageUntil;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        InitializeCount++;
        HasKey = false;
        EscapeCompleted = false;
        Debug.Log("GameManager inicializado. Llamadas: " + InitializeCount);

        if (InitializeCount > 1)
            Debug.LogError("Initialize() se ejecutó más de una vez.");

        ShowMessage("Encuentra la forma de salir. Mira alrededor (mouse), muévete (WASD) e interactúa (E).", 5f);
    }

    public void ObtainKey()
    {
        HasKey = true;
    }

    public void CompleteEscape()
    {
        EscapeCompleted = true;
        PlayerLook.InputLocked = true;
        PlayerLook.LockCursor(false);
    }

    public void ShowMessage(string text, float seconds = 3f)
    {
        message = text;
        messageUntil = Time.time + seconds;
    }

    public string CurrentMessage => Time.time < messageUntil ? message : null;
}
