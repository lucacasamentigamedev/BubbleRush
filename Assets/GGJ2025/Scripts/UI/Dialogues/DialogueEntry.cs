using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue_Entry", menuName = "DialogueSystem/Dialogue_Entry", order = 1)]
public class DialogueEntry : ScriptableObject
{

    [SerializeField]
    private uint dialogue_ID;
    [SerializeField]
    private uint entry_ID;
    [SerializeField]
    private string dialogue_Text;
    [SerializeField]
    private int nextEntry_ID;

    public uint Dialogue_ID
    {
        get { return dialogue_ID; }
    }
    public uint Entry_ID
    {
        get { return entry_ID; }
    }
    public string Dialogue_Text
    {
        get { return dialogue_Text; }
    }
    public int NextEntry_ID
    {
        get { return nextEntry_ID; }
    }
}
