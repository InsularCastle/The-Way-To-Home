using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public Transform loginBtn;

    void Start()
    {
        Init();
        GameSoundManager.Instance.PlayCustomBGMConnnection("Winter");
    }

    public void Init()
    {
        UIEventListener.Get(loginBtn.gameObject).onPointerClick = OnLogin;
    }

    private void OnLogin(PointerEventData ped)
    {
        WindowManager.Close(UIMenu.LoginWnd);
        Application.LoadLevelAsync("City");
    }
}
