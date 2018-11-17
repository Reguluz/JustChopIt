using System;
using System.Collections;
using System.Collections.Generic;
using GamePlayer;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEntryController : MonoBehaviour,IComparable<PlayerEntryController>
{
	public GameObject Layout;
	public PlayerProperties Entry;
	public Text Name;
	public Text Character;
	public Text Score;
	public Text State;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Entry)
		{
			SetData();
		}
	}

	
	public void SetData()
	{
		//Debug.Log("BoardRefresh");
		Name.text = Entry.User.NickName;
		Character.text = Enum.GetName(typeof(CharacterType),(int)Entry.User.CustomProperties["Character"]);
		Score.text = Entry.Score.ToString();
		State.text = Entry.StateType.ToString();
	}

	public int CompareTo(PlayerEntryController other)
	{
		return Entry.Score.CompareTo(other.Entry.Score);
	}

	public void Repositioning()
	{
		transform.parent = Layout.transform;
	}

	
}
