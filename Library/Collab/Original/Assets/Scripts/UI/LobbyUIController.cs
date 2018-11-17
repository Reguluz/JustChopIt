using System;
using System.Collections;
using System.Collections.Generic;
using GamePlayer;
using Networks;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class LobbyUIController : MonoBehaviour
	{
		public static int MAXPLAYER = 5;
		
		public GameObject CharacterList;
		public Button Match;
		public Button RoomBtn;
		public ToggleGroup CharacterlistToggle;

		
		public Text Serverping;
		public Text CurrentInfo;
		public Text InfoLists;

		public Text PlayerNum;
		public int Chosentype;
		private List<RoomInfo> _roomInfos;
		private Player[] _playersInRoom;
		private String _longInfo;
		private IEnumerable<Toggle> _characters;


		private void Awake()
		{
			CharacterList.SetActive(false);
		}

		// Use this for initialization
		void Start () {
			StartCoroutine(GetPingFromServer());
			StartCoroutine(DataRefresh());
			_characters = CharacterlistToggle.ActiveToggles();
			Chosentype = 0;
		}
	
		// Update is called once per frame
		void Update () {
			
		}
		
		/*public void ChooseCharacter(int i)
		{
			PlayerPrefs.SetInt("Charactertype",i);
		}*/

		public void JoinRoom()
		{
			Match.gameObject.SetActive(false);
			CharacterList.SetActive(true);
			_playersInRoom = PhotonNetwork.PlayerList;
			CurrentInfo.text = "当前房间内人数" + PhotonNetwork.CurrentRoom.PlayerCount + '\n';
			if (PhotonNetwork.IsMasterClient)
			{
				RoomBtn.GetComponentInChildren<Text>().text = "开始游戏";
			}
			else
			{
				RoomBtn.GetComponentInChildren<Text>().text = "返回大厅";
			}
		}

		public void JoinLobby()
		{
			Match.gameObject.SetActive(true);
			Match.GetComponentInChildren<Text>().text = "随机加入房间";
			RoomBtn.GetComponentInChildren<Text>().text = "创建房间";
		}

		public void RefreshRoom()
		{
			
			PlayerNum.text = PhotonNetwork.PlayerList.Length + "/" + MAXPLAYER + "位玩家";
			if (PhotonNetwork.PlayerList.Length == MAXPLAYER)
			{
				//StartGame();
			}
		}

		public void RefreshLobby()
		{
			CurrentInfo.text = "在线人数" + PhotonNetwork.CountOfPlayers + '\n'
			                   + "正在匹配人数" + PhotonNetwork.CountOfPlayersOnMaster + '\n'
			                   + "房间数量" + PhotonNetwork.CountOfRooms + '\n';
			_longInfo = "";
			if (_roomInfos != null)
			{
				foreach (RoomInfo roomInfo in _roomInfos)
				{
					_longInfo += "房间名:"+roomInfo.Name+"   " + "房间信息:"+roomInfo.CustomProperties + '\n';
				}
			}
			

			InfoLists.text = _longInfo;
		}

		public void RefreshRoomList(List<RoomInfo> roomList)
		{
			
			_roomInfos = roomList;
		}

		public void LobbyInit()
		{
			Match.interactable = false;
			JoinLobby();
		}

		public void ConnectSucceed()
		{
			Match.interactable = true;
		}

		public void ChooseCharacter(int serial)
		{
			Chosentype = serial;
			/*
			foreach (Toggle t in _characters)
			{
				if (t.isOn)
				{
					switch (t.name)
					{
						case "CircleToggle":
							Chosentype = CharacterType.Circle;
							break;
						case "DiamondToggle":
							Chosentype = CharacterType.Diamond;
							break;
					}
					break;
				}
			}*/
		}
		
		IEnumerator GetPingFromServer()
		{
			while (true)
			{
				Serverping.text = "Ping:" + PhotonNetwork.GetPing();
				yield return new WaitForSeconds(2f);
			}
		}
		
		IEnumerator DataRefresh()
		{
			while (true)
			{
				if (PhotonNetwork.InRoom)
				{
					RefreshRoom();
				}
				else
				{
					RefreshLobby();
				}
				yield return new WaitForSeconds(2f);
			}
		}
	}


}
