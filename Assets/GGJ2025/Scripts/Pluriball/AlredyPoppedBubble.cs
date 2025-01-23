public class AlredyPoppedBubble : Bubble
{
    override protected void InternalOnAwake()
    {
        bubbleType = EBubbleType.Normal;
        isAlive = false;
        spriteRenderer.sprite = poppedSprite;
    }
}
