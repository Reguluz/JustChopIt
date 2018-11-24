using System;
using System.Collections;
using System.Collections.Generic;
using GamePlayer;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class GameBoard : MonoBehaviour
{
	public GameObject ButtonS;
	public GameObject ButtonEnd;
	public GameObject InfoEntryPrefab;
	public GameObject InfoListTransform;
	public List<GameObject> PlayerObjList;
	public List<PlayerEntryController> Entries;
	public GameObject Untouchblank;
	
	private GameSettings _localSettings;
	private Text _btnSText;

	public List<GameObject> Players;
	private PlayerProperties[] _playerPropertieses;
	private RectTransform _transform;
	public int _target;

	private bool _isOnShow = false;
	// Use this for initialization
	private void Awake () {
		
		//_localSettings = GameObject.Find("GameSettings").GetComponent<GameSettings>();
		_target = ((int)PhotonNetwork.CurrentRoom.CustomProperties["TargetKilling"]+1)*10;
		_btnSText = ButtonS.GetComponentInChildren<Text>();
		ButtonEnd.SetActive(false);
		_transform = GetComponent<RectTransform>();
	}

	private void Start()
	{
		//Invoke(nameof(Init),2f);
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void DataRefresh(PlayerProperties source)
	{
		foreach (GameObject controller in PlayerObjList)
		{
			if (controller.GetComponent<PlayerEntryController>().Entry.User.Equals(source.User))
			{
				controller.GetComponent<PlayerEntryController>()?.SetData();
				Debug.Log("玩家"+source.User.NickName+"分数"+source.Score);
				if (source.Score == _target)
				{
					Debug.Log(source.User+"wins");
					GameOver(source.User);
				}
			}
		}
		Entries.Sort();
		/*for (int i = 0; i < PlayerObjList.Count; i++)
		{
			PlayerObjList[i].GetComponent<PlayerEntryController>().Entry = Entries[i].Entry;
		}*/
		
	}

	public void Move()
	{
		if (!_isOnShow)
		{
			_transform.anchoredPosition = _transform.anchoredPosition + new Vector2(600, 0);//Vector3.Lerp(transform.position,transform.position+new Vector3(600,0,0),1f);
			_btnSText.text = "记" + '\n' + "分" + '\n' + "板" + '\n' + "<";
			_isOnShow = true;
		}
		else
		{
			_transform.anchoredPosition = _transform.anchoredPosition + new Vector2(-600, 0);//Vector3.Lerp(transform.position,transform.position+new Vector3(-600,0,0),1f);
			_btnSText.text = "记" + '\n' + "分" + '\n' + "板" + '\n' + ">";
			_isOnShow = false;
		}
	}

	

	public void AddEntry(GameObject playerObj)
	{
		Players.Add(playerObj);
		GameObject info = Instantiate(InfoEntryPrefab,transform.position,transform.rotation);
		//PlayerEntryController controller = info.GetComponent<PlayerEntryController>();
		info.GetComponent<PlayerEntryController>().Layout = InfoListTransform;
		info.GetComponent<PlayerEntryController>().Repositioning();
		info.GetComponent<PlayerEntryController>().Entry = playerObj.GetComponent<PlayerProperties>();
		//info.GetComponent<PhotonView>().TransferOwnership(playerObj.GetComponent<PhotonView>().Owner);
		info.GetComponent<PlayerEntryController>().SetData();
		
		
		PlayerObjList.Add(info);
		Entries.Add(info.GetComponent<PlayerEntryController>());
	}

	private void GameOver(Player winner)
	{
		Untouchblank.SetActive(true);
		ButtonS.SetActive(false);
		ButtonEnd.SetActive(true);
		_transform.pivot = new Vector2(0.5f,0.5f);
		_transform.anchoredPosition = Vector2.zero;
		//transform.position = Vector3.zero;
		ButtonEnd.GetComponentInChildren<Text>().text = winner.NickName + "   Wins!";
	}

	public void ExitGame()
	{
		PhotonNetwork.LeaveRoom();
		PhotonNetwork.LoadLevel("MatchingScene");
	}
}
