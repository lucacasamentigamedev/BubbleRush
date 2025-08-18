using UnityEngine;
using System.Linq;

public class LevelHubMenu : BaseUI {
    private void OnEnable() {
        Debug.Log("LevelScores: " + string.Join(", ", LevelManager.Get().LevelScores.Select(kvp => $"{kvp.Key}:{kvp.Value}")));
    }
}