using UnityEngine;

public class PlayXingHao : MonoBehaviour
{

	public GameObject[] XingHao;
	public int CurrentXingHao = 0;
	public float CD = 0.5f;
	private float CountDown = 0;
	// Use this for initialization
	public bool CanAction = true;
	private int oldXingHao = 0;
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
				CurrentXingHao = Random.Range(0, 4);
			}
		}
		if (oldXingHao != CurrentXingHao)
		{
			oldXingHao = CurrentXingHao;
			for (int i = 0; i < XingHao.Length; i++)
			{
				XingHao[i].SetActive(i < oldXingHao);
			}
		}
	}
	public bool IsTrueChoose()
	{
		return CurrentXingHao >= 3;
	}
	public void SetTrueChoose()
	{
		CanAction = false;
		CurrentXingHao = 4;
		oldXingHao = 0;
	}
}
