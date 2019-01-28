using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public Slider slider;

    private AsyncOperation _op;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        StartCoroutine(LoadLevel());
    }

    private IEnumerator LoadLevel()
    {
        _op = Application.LoadLevelAsync(GameController.levelName);
        yield return new WaitForEndOfFrame();
        WindowManager.Close(UIMenu.LoadingWnd);
    }

    void Update()
    {
        if (_op != null)
        {
            slider.value = _op.progress;
        }
    }
}
