using UnityEngine;
using System.Collections;

/// <summary>
/// 关卡类型
/// </summary>
public enum LevelType
{
    Startup = 0,
    Loading = 1,
    City = 2,
    Play = 3
}

public class GameController : MonoBehaviour
{
    public static GameConfig config;

    public static string levelName;

    private static Level _level;

    private WindowManager _wndManager;
    private TimerManager _timerManager;

    void Awake()
    {
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(Camera.main);

        _timerManager = new TimerManager();

        config = new GameConfig();
        config.LoadConfigs();

        UserData.Instance.LoadRecords();

        _wndManager = new WindowManager();
        WindowManager.Open(UIMenu.LoginWnd);

        // 初始化音量
        var soundVolume = PlayerPrefs.GetFloat("SoundVolume", 1f);
        SoundManager.SetVolumeSFX(soundVolume);
        var voiceVolume = PlayerPrefs.GetFloat("VoiceVolume", 1f);
        SoundManager.SetVolumeMusic(voiceVolume);

        PlayerPrefs.SetString("Language", config.GetLanguages()[0]);
    }

    void OnLevelWasLoaded(int level)
    {
        if (level == (int)LevelType.Loading)
        {
            WindowManager.Open(UIMenu.LoadingWnd);
        }
        else if (level == (int)LevelType.City)
        {
            WindowManager.Open(UIMenu.ChooseLevelWnd);
        }
        else if (level >= (int)LevelType.Play && level < 6)
        {
            if (Level.recordId == -1)
                _level = new Level(0);
            else
                _level = new Level(_level.praise);
        }
    }

    void Update()
    {
        // 每一帧的增量时间
        float dt = Time.deltaTime;

        _timerManager.Update(dt);

        if (_level != null)
        {
            _level.Update(dt);
        }
    }

    public static void LevelEnd()
    {
        _level.Clear();
        _level = null;
        WindowManager.Close(UIMenu.PlayWnd);
        Application.LoadLevelAsync("City");
    }

    public static int GetLevelPraise()
    {
        return _level.praise;
    }

    public static void SetLevelPraise(int praise)
    {
        _level.praise = praise;
    }
}
