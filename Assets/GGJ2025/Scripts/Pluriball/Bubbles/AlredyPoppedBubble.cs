public class AlredyPoppedBubble : Bubble
{
    override protected void InternalOnAwake()
    {
        bubbleType = EBubbleType.AlredyPopped;
        isAlive = false;
    }
}
