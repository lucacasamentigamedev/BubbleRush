public class RockBubble : Bubble
{
    override protected void InternalOnAwake() {
        bubbleType = EBubbleType.Rock;
        base.InternalOnAwake();
    }
}
