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
	public Text Killednum;


	//public static UIController UiCanvas;

	// Use this for initialization
	void Start ()
	{
		StartCoroutine(GetPingFromServer());

	}
	
	// Update is called once per frame
	void Update () {

	}

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
		PhotonNetwork.LoadLevel(0);
		PhotonNetwork.LeaveRoom();
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
	