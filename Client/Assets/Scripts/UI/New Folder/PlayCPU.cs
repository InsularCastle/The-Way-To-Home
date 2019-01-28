using UnityEngine;
using UnityEngine.UI;

public class PlayCPU : MonoBehaviour
{
	public Transform ZhiZheng;
	public Text text;
	public int CurrentChoose = 0;
	private int LastChoose = 0;
	public float CD = 2.5f;
	private float CountDown = 0f;
	private float PlayTime = 1f;
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
				CountDown = 0;
				CurrentChoose = Random.Range(8, -8);
				PlayTime = 1f;
			}
		}
		if (LastChoose != CurrentChoose && PlayTime >= 0f)
		{
			PlayTime -= Time.deltaTime;
			ZhiZheng.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Lerp(CurrentChoose * 10, LastChoose * 10, PlayTime) * -1));
		}
		else
		{
			LastChoose = CurrentChoose;
			text.text = string.Format("<color=#ff3300ff>{0}</color>%", ((int)6.25f * (LastChoose + 8)));
			PlayTime = 1f;
		}
	}
	public bool IsTrueChoose()
	{
		return LastChoose <= -3;
	}
	public void SetTrueChoose()
	{
		CanAction = false;
		CurrentChoose = -8;
		LastChoose = 3;
	}
}
