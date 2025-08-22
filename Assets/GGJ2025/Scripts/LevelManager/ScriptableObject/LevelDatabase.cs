using UnityEngine;

[CreateAssetMenu(fileName = "Level_Databases", menuName = "LevlSystem/Level_Databases", order = 2)]
public class LevelDatabase : ScriptableObject
{
    [SerializeField]
    private LevelEntry[] entries;


    public LevelEntryStruct GetCurrentEntry(uint level)
    {
        for (int i = entries.Length - 1; i >= 0 ; i--)
        {
            if (level >= entries[i].Data.unlock_Lvl)
            {
                return entries[i].Data;
            }
        }

        return new LevelEntryStruct();
    }

    public LevelEntryStruct GetEndlessLevelEntry()
    {
        LevelEntryStruct levelEntryStruct = new LevelEntryStruct();
        levelEntryStruct.is_Timer_Activate = true;
        levelEntryStruct.timer_for_level = 0.0f;
        levelEntryStruct.stars_for_level = new float[]{ 0.0f, 0.0f, 0.0f };
        levelEntryStruct.unlock_Lvl =99;
        levelEntryStruct.grid_Size = new Vector2 (15,8);
        BubbleToCreate bubble = new BubbleToCreate();
        bubble.setFiller = true;
        bubble.type = EBubbleType.Normal;
        bubble.min_Pop = 1;
        bubble.max_Pop = 2;
        levelEntryStruct.bubbles= new BubbleToCreate[] { bubble };

        return levelEntryStruct;
    }
}