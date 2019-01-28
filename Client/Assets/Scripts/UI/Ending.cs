using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Ending : MonoBehaviour
{
    public Sprite[] sprites;
    public Image Image;

    void Start()
    {
        StartCoroutine(RefreshImage());

        GetComponent<RectTransform>().sizeDelta = new Vector2(1920f, 1080f);
    }

    IEnumerator RefreshImage()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            Image.sprite = sprites[i];
            Image.DOFade(1, 2f);
            yield return new WaitForSeconds(6f);
            Image.DOFade(0, 2f);
            yield return new WaitForSeconds(2f);
        }

        WindowManager.Close(UIMenu.ReturnCityWnd);
        WindowManager.Close(UIMenu.GameWnd);
        WindowManager.Close(UIMenu.EndingWnd);
        GameController.LevelEnd();
    }
}
