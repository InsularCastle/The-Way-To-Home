using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class MainUI : MonoBehaviour
{
	public PlayXingHao Signal;
	public PlayCPU Cpu;
	public PlayMaskText LeftTicket;
	public PlayMaskText RightTicket;

	public GameObject Red;
	public GameObject Black;

    public Image Barrage;

	// Use this for initialization
	void Start()
	{
        Barrage.rectTransform.anchoredPosition = new Vector2(Screen.width * 0.5f + 400f, 0);
    }

	// Update is called once per frame
	void Update()
	{
		bool isTrue = Signal.IsTrueChoose() && Cpu.IsTrueChoose() && LeftTicket.IsTrueChoose() && RightTicket.IsTrueChoose();
		if (isTrue && Black.gameObject.activeSelf)
		{
			Black.gameObject.SetActive(false);
			Red.gameObject.SetActive(true);
		}
		else if (Red.gameObject.activeSelf && !isTrue)
		{
			Black.gameObject.SetActive(true);
			Red.gameObject.SetActive(false);
		}
		if (Input.GetKeyUp(KeyCode.Z))
		{
			Signal.SetTrueChoose();
		}
		if (Input.GetKeyUp(KeyCode.X))
		{
			Cpu.SetTrueChoose();
		}
		if (Input.GetKeyUp(KeyCode.C))
		{
			LeftTicket.SetTrueChoose();
		}
		if (Input.GetKeyUp(KeyCode.V))
		{
			RightTicket.SetTrueChoose();
		}
	}

	public void Fun_GetTicketClick()
	{
        StartCoroutine(BarrageMove());
    }

    IEnumerator BarrageMove()
    {
        Tweener tweener = DOTween.To(
                () => Barrage.rectTransform.anchoredPosition
                , x => Barrage.rectTransform.anchoredPosition = x
                , new Vector2(-Screen.width * 0.5f - 400f, 0), 2);
        tweener.SetUpdate(true);
        tweener.SetEase(Ease.Linear);
        yield return new WaitForSeconds(2f);

        Level.recordId = -1;
        WindowManager.Close(UIMenu.TicketWnd);
        WindowManager.Open(UIMenu.GameWnd);
        Application.LoadLevelAsync("Level");
    }
}
