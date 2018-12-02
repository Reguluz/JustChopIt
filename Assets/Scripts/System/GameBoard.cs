using System;
using System.Collections;
using System.Collections.Generic;
using GamePlayer;
using Photon.Pun;
using Photon.Realtime;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class GameBoard : MonoBehaviour
{
	public GameObject ButtonS;
	public GameObject ButtonEnd;
	public GameObject InfoEntryPrefab;
	public GameObject InfoListTransform;
	public List<GameObject> PlayerObjList;	//单行板子内容Gameobject
	public List<PlayerEntryController> Entries;
	public GameObject Untouchblank;

	public MonsterSystem MonsterSystem;
	
	private GameSettings _localSettings;
	private Text _btnSText;

	public List<PlayerProperties> Players;
	private PlayerProperties[] _playerPropertieses;
	private RectTransform _transform;
	public int _target;

	private bool _isOnShow = false;

	private int _maxKill = 0;
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
		
		foreach (PlayerEntryController entry in Entries)
		{
			if (entry.Entry.User.Equals(source.User))
			{
				entry.SetData();
				Debug.Log("玩家"+source.User.NickName+"分数"+source.Score);
				if (source.Score >= _target)
				{
					source.Score = _target;
					Debug.Log(source.User+"wins");
					GameOver(source.User);
				}

				
				if (_maxKill < source.Score)
				{
					Debug.Log("最大击杀更新为"+_maxKill);
					_maxKill = source.Score;
					if (_maxKill % 5 == 0)
					{
						//最大击杀为5的倍数释放一个boss
						MonsterSystem.SetMonster();
						Debug.Log("释放boss");
					}
				} 
				break;
			}
		}
		Players.Sort(new EntryCompare());
		for (int i = 0; i < Entries.Count; i++)
		{
			Entries[i].Entry = Players[i];
		}
		
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
		Players.Add(playerObj.GetComponent<PlayerProperties>());
		GameObject info = Instantiate(InfoEntryPrefab,transform.position,transform.rotation);
		info.name = playerObj.GetComponent<PhotonView>().Owner.NickName;
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
