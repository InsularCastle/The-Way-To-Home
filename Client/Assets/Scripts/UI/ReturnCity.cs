using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReturnCity : MonoBehaviour
{
    public Transform confirmBtn;
    public Transform cancelBtn;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        UIEventListener.Get(confirmBtn.gameObject).onPointerClick = (PointerEventData ped) => 
        {
            WindowManager.Close(UIMenu.ReturnCityWnd);
            WindowManager.Close(UIMenu.GameWnd);
            GameController.LevelEnd();
        };
        UIEventListener.Get(cancelBtn.gameObject).onPointerClick = (PointerEventData ped) =>
        {
            WindowManager.Close(UIMenu.ReturnCityWnd);
            NotificationCenter.DefaultCenter.PostNotification("SetAllowPlayerMove", true);
        };
    }
}
