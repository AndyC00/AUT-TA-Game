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
        if (mainBgmInstance.isValid()) return;

        mainBgmInstance = RuntimeManager.CreateInstance("event:/MainBGM");
        mainBgmInstance.start();
    }

    public void StopMainBGM()
    {
        if (mainBgmInstance.isValid())
        {
            mainBgmInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            mainBgmInstance.release();
            mainBgmInstance.clearHandle();
        }
    }

    public void PauseMainSong()
    {
        mainBgmInstance.setPaused(true);
    }

    // last stage BGM
    public void PlayLastStageBGM()
    {
        if (lastStageBGM.isValid()) return;

        lastStageBGM = RuntimeManager.CreateInstance("event:/LastBGM");
        lastStageBGM.start();
    }

    public void StopLastStageBGM()
    {
        if (lastStageBGM.isValid())
        {
            lastStageBGM.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            lastStageBGM.release();
            lastStageBGM.clearHandle();
        }
    }

    // music handle for card game
    public void ResumeMainBGM()
    {
        var currentState = GameManager.instance.GetCurrentState();
        if (currentState != GameManager.GameState.ThirdStage)
        {
            mainBgmInstance.setPaused(false);
        }
        else
        {
            lastStageBGM.setPaused(false);
        }
    }

    public void PauseMainBGM()
    {
        if (mainBgmInstance.isValid())
            mainBgmInstance.setPaused(true);

        if (lastStageBGM.isValid())
            lastStageBGM.setPaused(true);
    }
}
