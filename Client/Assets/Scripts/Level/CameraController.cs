using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera VirtualCamera;

    public CinemachineConfiner Confiner;

    void Awake()
    {
        DontDestroyOnLoad(this);
        NotificationCenter.DefaultCenter.AddObserver(this, "SetCameraFollow", SetFollow);
        NotificationCenter.DefaultCenter.AddObserver(this, "SetCameraConfiner", SetConfiner);
    }

    void OnDestroy()
    {
        NotificationCenter.DefaultCenter.RemoveObserver(this, "SetCameraFollow");
        NotificationCenter.DefaultCenter.RemoveObserver(this, "SetCameraConfiner");
    }

    public void SetFollow(object[] parms)
    {
        var target = parms[0] as Transform;
        VirtualCamera.Follow = target;
    }

    public void SetConfiner(object[] parms)
    {
        Confiner.InvalidatePathCache();
        var col = parms[0] as Collider2D;
        Confiner.m_BoundingShape2D = col;
    }
}
