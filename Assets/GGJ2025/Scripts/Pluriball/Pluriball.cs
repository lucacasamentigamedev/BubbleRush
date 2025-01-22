using System;
using UnityEngine;

public class Pluriball : MonoBehaviour {
    private int rows;
    private int columns;
    private Bubble[] bubbles;
    private int remainingBubbles;

    public void Generate(int rows, int columns /*pattern??*/)
    {
        //clear array
        Array.Clear(bubbles, 0, bubbles.Length);
        //TODO: generate rows * columns Bubble object get from Pooler and by pattern choosing the right Bubble
        remainingBubbles = rows * columns;
        //register on every bubble popped
        foreach (Bubble bubble in bubbles)
        {
            bubble.OnDestroy += OnBubbleDestroy;
        }
    }

    private void Update() {
        if (InputManager.Player_Left_Mouse_Click) {
            this.CheckPlayerCLick();
        }
    }

    private void CheckPlayerCLick()
    {
            Debug.Log("Player has clicked position" + InputManager.Player_Mouse_Position);
            /*TODOD check if has clicked a real cell
             what cell based on position
            then check on what type of weapon player has
            and switch for specific*/
    }

    public void OnBubbleDestroy()
    {
        remainingBubbles--;
        if (remainingBubbles <= 0) {
            Debug.Log("Hai vinto il livello");
            //TODO make things
        } else {
            
        }
    }
}
