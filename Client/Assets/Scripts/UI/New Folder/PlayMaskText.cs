using UnityEngine;

public class PlayMaskText : MonoBehaviour
{

	public RectTransform Content;
	public int Step = 24;
	public float CD = 1.8f;
	private float CountDown = 0f;
	public int CurrentChoose = 0;
	private int LastChoose = 0;
	public float PlayTime = 1f;
	public bool CanAction = true;
	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (CanAction)
		{
			if (CountDown <= CD)
			{
				CountDown += Time.deltaTime;
			}
			else
			{
				CountDown = 0f;
				CurrentChoose = Random.Range(-5, 6);
				PlayTime = 1f;
			}
		}
		if (LastChoose != CurrentChoose && PlayTime >= 0)
		{
			PlayTime -= Time.deltaTime;
			Vector3 oldVec = Content.anchoredPosition3D;
			oldVec.y = Mathf.Lerp(CurrentChoose * 24, LastChoose * 24, PlayTime);
			Content.anchoredPosition3D = oldVec;
		}
		else
		{
			LastChoose = CurrentChoose;
			PlayTime = 1f;
		}
	}
	public bool IsTrueChoose()
	{
		return LastChoose == -1 && LastChoose == CurrentChoose;
	}
	public void SetTrueChoose()
	{
		CanAction = false;
		CurrentChoose = -1;
		LastChoose = 4;
	}
}
