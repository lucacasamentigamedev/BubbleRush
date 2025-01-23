using System;
using UnityEngine;
public static class InputManager
{
    private static Inputs input;

    static InputManager()
    {
        input = new Inputs();
        input.Menu.Enable();
    }

    public static Inputs.PlayerActions Player { get { return input.Player; } }
    public static Inputs.MenuActions Menu { get { return input.Menu; } }
    public static bool Player_Left_Mouse_Click { get { return input.Player.Interact.IsPressed(); } }
    public static Vector2 Player_Mouse_Position { get { return input.Player.Position.ReadValue<Vector2>(); } }
}
