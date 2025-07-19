using FMOD.Studio;
using FMODUnity;

public class AudioEvent {
    public string Path { get; private set; }
    public EAudioCategory Category { get; private set; }

    public AudioEvent(string path, EAudioCategory category) {
        Path = path;
        Category = category;
    }

    public EventInstance CreateInstance() {
        var instance = RuntimeManager.CreateInstance(Path);
        return instance;
    }
}