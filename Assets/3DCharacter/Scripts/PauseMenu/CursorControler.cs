using UnityEngine;

public class CursorControler : MonoBehaviour
{
    [SerializeField] private bool isCursorLocked = true; // Boolean to check if the cursor is locked.
    [SerializeField] private bool isCursorVisible = false; // Boolean to check if the cursor is visible.

    void Start()
    {
        UpdateCursorState(); // Updates the cursor state at the start of the game.
    }
    [ContextMenu("Toggle Cursor Lock")]
    public void UpdateCursorState()
    {
        Cursor.lockState = isCursorLocked ? CursorLockMode.Locked : CursorLockMode.None; // Sets the cursor lock state based on isCursorLocked.
        Cursor.visible = !isCursorLocked || isCursorVisible; // Sets the cursor visibility based on isCursorLocked and isCursorVisible.
    }
}
