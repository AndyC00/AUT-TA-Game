using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class MusicController : MonoBehaviour
{
    private EventInstance mainBgmInstance;
    private EventInstance lastStageBGM;

    // singleton pattern
    public static MusicController Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMainBGM();
    }

    // main BGM
    public void PlayMainBGM()
    {
        mainBgmInstance = RuntimeManager.CreateInstance("event:/MainBGM");
        mainBgmInstance.start();
    }

    public void StopMainBGM()
    {
        mainBgmInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        mainBgmInstance.release();
    }

    // last stage BGM
    public void PlayLastStageBGM()
    {
        lastStageBGM = RuntimeManager.CreateInstance("event:/LastBGM");
        lastStageBGM.start();
    }

    public void StopLastStageBGM()
    {
        lastStageBGM.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        lastStageBGM.release();
    }
}
