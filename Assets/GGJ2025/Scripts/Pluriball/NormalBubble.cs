public class NormalBubble : Bubble
{
    override protected void InternalOnAwake()
    {
        bubbleType = EBubbleType.Normal;
        base.InternalOnAwake();
    }
}
