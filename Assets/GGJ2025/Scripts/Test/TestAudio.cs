using UnityEngine;

public class TestAudio : MonoBehaviour {

    [SerializeField]
    private string soundName;
    [SerializeField]
    private float interval;

    private void Start() {
        // play sound every 2 seconds
        InvokeRepeating("PlayTestSound", 0f, interval);
    }

    private void PlayTestSound() {
        AudioManager.PlayOneShotSound(soundName, new FMODParameter[] {
            new FMODParameter("BUBBLE_POP_TYPE", 2.0f)
        });
    }
}
