using UnityEngine;
using UnityEngine.InputSystem;//DA RIMUOVERE E USARE L'INPUT MNG


public class PlayerTestClick : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 screenPoint = InputManager.Player_Mouse_Position;
        //screenPoint.z = 10;
        //Debug.Log(Camera.main.ScreenToWorldPoint(screenPoint));

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector3 screenPoint = InputManager.Player_Mouse_Position;
            screenPoint.z = 10;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Debug.Log(mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            if (hit.collider != null)
            {
                IClickable clickable = hit.collider.GetComponent<IClickable>();
                if (clickable != null)
                {
                    clickable.OnClick(mousePosition,1);
                }
            }
        }
    }
}
