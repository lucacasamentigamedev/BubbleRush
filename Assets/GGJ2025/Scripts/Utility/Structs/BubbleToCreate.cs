using System;

[Serializable]
public struct BubbleToCreate
{
    public EBubbleType type;
    public uint min_Spawn;
    public uint max_Spawn;
    public uint min_Pop;
    public uint max_Pop;
}