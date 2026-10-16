using UnityEngine;

// Rotación con el mouse (cuerpo en Y, cámara en X) para poder apuntar a los objetos.
public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float sensitivity = 2f;

    private float pitch;

    public static bool InputLocked { get; set; }

    private void Start()
    {
        LockCursor(true);
    }

    private void Update()
    {
        if (InputLocked || playerCamera == null)
            return;

        float mx = Input.GetAxis("Mouse X") * sensitivity;
        float my = Input.GetAxis("Mouse Y") * sensitivity;

        transform.Rotate(0f, mx, 0f);
        pitch = Mathf.Clamp(pitch - my, -80f, 80f);
        playerCamera.localEulerAngles = new Vector3(pitch, 0f, 0f);
    }

    public static void LockCursor(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }
}
