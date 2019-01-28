using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this);
    }
}

public class UIMaskLayer : MonoBehaviour
{
    void Start()
    {
        int width = Screen.width;
        int height = Screen.height;
        int designWidth = 1920;//开发时分辨率宽
        int designHeight = 1080;//开发时分辨率高
        float s1 = (float)designWidth / (float)designHeight;
        float s2 = (float)width / (float)height;
        if (s1 < s2)
        {
            designWidth = (int)Mathf.FloorToInt(designHeight * s2);
        }
        else if (s1 > s2)
        {
            designHeight = (int)Mathf.FloorToInt(designWidth / s2);
        }
        float contentScale = (float)designWidth / (float)width;
        RectTransform rectTransform = this.transform as RectTransform;
        if (rectTransform != null)
        {
            rectTransform.sizeDelta = new Vector2(designWidth, designHeight);
        }
    }
}

public enum UIMenu
{
    ChooseLevelWnd,
    LoadingWnd,
    LoginWnd,
    PlayWnd,
    ReturnCityWnd,
    GameWnd,
    EndingWnd,
    TicketWnd
}

public class WindowManager
{
    private static Dictionary<UIMenu, GameObject> _windows = new Dictionary<UIMenu, GameObject>();

    private static Transform _canvas;

    public WindowManager()
    {
        Object obj = Resources.Load("UI/UI");
        Transform uiTran = (GameObject.Instantiate(obj) as GameObject).transform;
        uiTran.gameObject.AddComponent<DontDestroyOnLoad>();

        _canvas = uiTran.Find("Canvas");
    }

    /// <summary>
    /// 打开界面
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static GameObject Open(UIMenu menu)
    {
        if (_windows.ContainsKey(menu))
        {
            return _windows[menu];
        }
        else
        {
            var wnd = InitWnd(menu);
            _windows.Add(menu, wnd);

            return wnd;
        }
    }

    /// <summary>
    /// 关闭界面
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static void Close(UIMenu menu)
    {
        if (_windows.ContainsKey(menu))
        {
            GameObject.Destroy(_windows[menu]);
            _windows.Remove(menu);
        }
    }

    /// <summary>
    /// 获取界面
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static GameObject GetWnd(UIMenu menu)
    {
        if (_windows.ContainsKey(menu))
        {
            return _windows[menu];
        }
        else
        {
            return null;
        }
    }

    private static GameObject InitWnd(UIMenu menu)
    {
        var obj = Resources.Load("UI/" + menu);
        var go = (GameObject.Instantiate(obj) as GameObject);
        go.name = menu.ToString();
        go.transform.parent = _canvas;
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = Vector3.one;
        go.AddComponent<UIMaskLayer>();

        return go;
    }
}

