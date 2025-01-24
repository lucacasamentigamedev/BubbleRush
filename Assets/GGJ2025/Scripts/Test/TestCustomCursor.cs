using UnityEngine;
using UnityEngine.InputSystem;

public class TestCustomCursor : MonoBehaviour
{
    [SerializeField]
    private RectTransform customCursorImage; // Reference all'Image del cursore

    void Start()
    {
        // Nasconde il cursore di sistema
        //Cursor.visible = false;
    }

    void Update()
    {
        // Sposta il cursore personalizzato alla posizione del mouse
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        customCursorImage.position = mousePosition;
    }
}
