using System.Collections;
using UnityEngine;

public class GameSoundManager
{
    public static GameSoundManager Instance
    {
        get
        {
            if (sInstance == null)
                sInstance = new GameSoundManager();

            return sInstance;
        }
    }

    private static GameSoundManager sInstance;

    public void PlayCustomBGMConnnection(string name)
    {
        SoundManager.Instance.PlayCustomConnection(name);
    }

    public void PlayUISFX(string name)
    {
        SoundManager.PlaySFX(name, false, 0, SoundManager.GetVolumeSFX());
    }

    public void PauseBGM(bool pause)
    {
        SoundManager.Instance.mutedMusic = pause;
    }
}