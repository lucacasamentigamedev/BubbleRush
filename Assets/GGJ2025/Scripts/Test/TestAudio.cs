using UnityEngine;
using System.Collections;

public class TestAudio : MonoBehaviour {

    private string soundName = "TestLoop";
    // private float interval = 2;

    private void Start() {
        // play sound every 2 seconds
        //InvokeRepeating("PlayTestSound", 0f, interval);
        StartCoroutine(WaitAndExecute());
    }

    private void PlayTestSound() {
        /*AudioManager.PlayOneShotSound(soundName, new FMODParameter[] {
            new FMODParameter("BUBBLE_POP_TYPE", 2.0f)
        });*/
    }

    IEnumerator WaitAndExecute() {
        yield return new WaitForSeconds(3.0f);
        AudioManager.PlayBackgroundMusic(soundName);
    }
}
