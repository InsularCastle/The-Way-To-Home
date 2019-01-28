using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameWnd : MonoBehaviour
{
    public Slider slider;
    public Text text;

	void Start()
    {
        NotificationCenter.DefaultCenter.AddObserver(this, "GameWndSetSlider", SetSlider);
        NotificationCenter.DefaultCenter.AddObserver(this, "GameWndSetText", SetText);
    }

    void OnDestroy()
    {
        NotificationCenter.DefaultCenter.RemoveObserver(this, "GameWndSetSlider");
        NotificationCenter.DefaultCenter.RemoveObserver(this, "GameWndSetText");
    }

    private void SetSlider(object[] parms)
    {
        var num = (float)parms[0];
        slider.value = num / 45;
    }

    private void SetText(object[] parms)
    {
        var num = (int)parms[0];
        text.text = num.ToString();
    }
}
