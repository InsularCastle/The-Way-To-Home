using UnityEngine;
using System.Collections;

public class Level
{
    public static int recordId;

    public int praise;

    private bool _playedBgm;

    private Transform _playerTransform;

    public Level(int lastPraise)
    {
        praise = lastPraise;
        Object obj = Resources.Load("Buses/Bus");
        _playerTransform = (GameObject.Instantiate(obj) as GameObject).transform;
        if (Application.loadedLevelName == "Level")
            _playerTransform.localPosition = new Vector2(-293f, -15f);
        else if (Application.loadedLevelName == "Level2")
            _playerTransform.localPosition = new Vector2(-485f, -15f);
        else if (Application.loadedLevelName == "Level3")
            _playerTransform.localPosition = new Vector2(-337f, -15f);
        NotificationCenter.DefaultCenter.PostNotification("SetCameraFollow", _playerTransform);
        var bg = GameObject.Find("Bg");
        var bgCol = bg.GetComponent<Collider2D>();
        NotificationCenter.DefaultCenter.PostNotification("SetCameraConfiner", bgCol);
        _playedBgm = false;
    }

    public void Update(float dt)
    {
        if (!_playedBgm)
        {
            if (Application.loadedLevelName == "Level")
                GameSoundManager.Instance.PlayCustomBGMConnnection("Mission1");
            else if (Application.loadedLevelName == "Level2")
                GameSoundManager.Instance.PlayCustomBGMConnnection("Mission2");
            else if (Application.loadedLevelName == "Level3")
                GameSoundManager.Instance.PlayCustomBGMConnnection("Mission3");
            _playedBgm = true;
        }
    }

    public void Clear()
    {
        ResetPraise();
    }

    public void RefreshPraise(int levelPraise)
    {
        praise = levelPraise;
    }

    public void ResetPraise()
    {
        praise = 0;
    }
}
