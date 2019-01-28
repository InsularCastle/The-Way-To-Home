using System;
using System.Collections.Generic;


public class GameTimer
{
    private float _elapsedTime = 0f;

    private Action _act;

    private float _delay = 0f;

    private bool _end = false;

    public bool end
    {
        get
        {
            return _end;
        }
    }

    public static void Invoke(Action act, float delay)
    {
        GameTimer timer = new GameTimer(delay, act);
        TimerManager.AddTimer(timer);
    }

    public GameTimer(float delay, Action act)
    {
        _delay = delay;
        _act = act;
        _end = false;
    }

    public void Update(float dt)
    {
        if (_elapsedTime >= _delay)
        {
            _act();
            _elapsedTime = 0;
            _end = true;
        }
        else
        {
            _elapsedTime += dt;
        }
    }
}

public class TimerManager
{
    private static List<GameTimer> _timers = new List<GameTimer>();

    public void Update(float dt)
    {
        for (int i = 0; i < _timers.Count; i++)
        {
            GameTimer timer = _timers[i];
            if (timer.end)
            {
                _timers.Remove(timer);
            }
            else
            {
                timer.Update(dt);
            }
        }
    }

    public static void AddTimer(GameTimer timer)
    {
        _timers.Add(timer);
    }
}
