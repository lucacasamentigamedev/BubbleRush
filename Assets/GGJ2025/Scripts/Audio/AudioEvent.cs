using FMOD.Studio;
using FMODUnity;

public enum AudioCategory {
    SFX,
    UI,
    Environment,
    Music,
    Player
}

public class AudioEvent {
    public string Path { get; private set; }
    public AudioCategory Category { get; private set; }

    public AudioEvent(string path, AudioCategory category) {
        Path = path;
        Category = category;
    }

    public EventInstance CreateInstance() {
        var instance = RuntimeManager.CreateInstance(Path);
        return instance;
    }
}