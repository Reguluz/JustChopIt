using System.Collections;
using System.Collections.Generic;
using GamePlayer;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

	public PlayerProperties PlayerProperties;
	public EasyTouchMove EasyTouchMove;

	public GameObject[] Skill;

	public GameObject Menu;

	public Text Pingshow;
	public Text Fps;
	public Text Killednum;

	
	//fps
	public float fpsMeasuringDelta = 2.0f;

	private float timePassed;
	private int m_FrameCount = 0;
	private float m_FPS = 0.0f;

	//public static UIController UiCanvas;

	// Use this for initialization
	void Start ()
	{
		StartCoroutine(GetPingFromServer());

		/***FPS*****************************/
		timePassed = 0.0f;
		/***FPS*****************************/
	}
	
	// Update is called once per frame
	void Update () {
		
		
		/*******fps*****************************************************/
		m_FrameCount = m_FrameCount + 1;
		timePassed = timePassed + Time.deltaTime;
		if (timePassed > fpsMeasuringDelta)
		{
			m_FPS = m_FrameCount / timePassed;

			timePassed = 0.0f;
			m_FrameCount = 0;
		}
		Fps.text = m_FPS.ToString();
		/*******fps******************************************************/
	}

	//隐藏未使用的技能槽位
	public void RejectorBlank()
	{
		foreach (GameObject obj in Skill)
		{
			if (!obj.GetComponent<CoolDownImageController>().SkillActived)
			{
				obj.SetActive(false);
			}
		}
	}
	
	//定时获取PING
	IEnumerator GetPingFromServer()
	{
		while (true)
		{
			Pingshow.text = "Ping:" + PhotonNetwork.GetPing();
			yield return new WaitForSeconds(2f);
		}
	}

	
	public void Refresh()
	{
		Killednum.text = "得分" + PlayerProperties.Score;
	}

	public void LeaveGame()
	{
		PhotonNetwork.LeaveRoom();
		PhotonNetwork.LoadLevel(0);
	}

	public void DisableSkill()
	{
		foreach (GameObject skill in Skill)
		{
			skill.GetComponent<CoolDownImageController>().PlayerActived = false;
		}
	}

	public void AbleSkill()
	{
		foreach (GameObject skill in Skill)
		{
			skill.GetComponent<CoolDownImageController>().PlayerActived = true;
		}
	}
}
	