using System;
using System.Collections;
using System.Collections.Generic;
using Account;
using GamePlayer;
using Networks;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace UI
{
	public class LobbyUIController : MonoBehaviour
	{
		

		[Header("Script")]
		public MatchingNetConnect Connection;
		public GameSettings LocalSettings;
		
		[Header("Total")]
		public Button Match;
		public Button RoomBtn;
		public Text Serverping;
		public Text CurrentInfo;
		public Text InfoLists;
		
		[Header("Lobby")]
		public GameObject NickNameInput;	
		private List<RoomInfo> _roomInfos;
		private String _roomListInfoText;
		public GameObject RoomModeMenu;
		
		
		[Header("Room")]
		public GameObject CharacterList;	//选择角色菜单
		public ToggleGroup CharacterlistToggle;
		public Text PlayerNum;
		private String _playerInfoText;
		private IEnumerable<Toggle> _characters;
		private String _roomSettingText;



		private void Awake()
		{
			CharacterList.SetActive(false);
		}

		// Use this for initialization
		void Start () {
			StartCoroutine(GetPingFromServer());
			StartCoroutine(DataRefresh());
			_characters = CharacterlistToggle.ActiveToggles();
			LocalSettings = GameObject.Find("GameSettings").GetComponent<GameSettings>();
			NickNameInput.transform.GetChild(0).GetComponent<Text>().text = AccountInfo.Nickname ?? "Nickname";
			RoomModeMenu.SetActive(false);
		}
	
		// Update is called once per frame
		void Update () {
			
		}
		
		/*public void ChooseCharacter(int i)
		{
			PlayerPrefs.SetInt("Charactertype",i);
		}*/

		public void MatchBtnPressed()
		{
			if (PhotonNetwork.InLobby)
			{
				Connection.StartMatching();
			}else if (PhotonNetwork.InRoom)
			{
				if (PhotonNetwork.IsMasterClient)
				{
					if (PhotonNetwork.PlayerList.Length > -1)
					{
						bool allready = true;
						foreach (Player otherinroom in PhotonNetwork.PlayerListOthers)
						{
							if (!(bool) otherinroom.CustomProperties["IsReady"])
							{
								allready = false;
								break;
							}
						}

						if (allready)
						{
							Connection.StartGame();
						}
						else
						{
							Connection.MessageShow("有玩家没有准备");
						}
					}
					else
					{
						Connection.MessageShow("人数不足无法开始游戏");
					}
				}
				else
				{
					if (LocalSettings.IsReady)
					{
						LocalSettings.SetReady(false); //准备
						Match.GetComponentInChildren<Text>().text = "Unready";
						CharacterlistToggle.allowSwitchOff = true;
					}
					else
					{
						LocalSettings.SetReady(true); //准备
						Match.GetComponentInChildren<Text>().text = "Ready";
						CharacterlistToggle.allowSwitchOff = false;
					}
					RefreshRoom();
				}
				
			}
		}

		public void RoomBtmPressed()
		{
			if (PhotonNetwork.InLobby)
			{
				RoomModeMenu.SetActive(true);
			}else if (PhotonNetwork.InRoom)
			{
				Connection.LeaveRoom();
			}
		}

		public void RoomCreate()
		{
			RoomModeMenu.SetActive(false);
			//获取参数
			Connection.CreateRoom();
			Connection.MaxPlayer = (int)PhotonNetwork.CurrentRoom.CustomProperties["MaxPlayer"];
		}

		public void JoinRoom()
		{
			
			CharacterList.SetActive(true);
			NickNameInput.SetActive(false);
			PlayerPrefs.SetInt("Charactertype",0);
			if (PhotonNetwork.IsMasterClient)
			{
				Match.GetComponentInChildren<Text>().text = "Start Game";
				RoomBtn.GetComponentInChildren<Text>().text = "Back";
			}
			else
			{
				Match.GetComponentInChildren<Text>().text = "Unready";
				RoomBtn.GetComponentInChildren<Text>().text = "Back";
			}
			RefreshRoom();
			
		}

		public void JoinLobby()
		{
			Debug.Log("LobbyUIRefresh");
			Match.gameObject.SetActive(true);
			CharacterList.SetActive(false);
			NickNameInput.SetActive(true);
			Match.GetComponentInChildren<Text>().text = "Start Matching";
			RoomBtn.GetComponentInChildren<Text>().text = "Create Room";
			InfoLists.text = _roomListInfoText;
		}

		public void RefreshRoom()
		{
			_roomSettingText =  "地图         "+LocalSettings.MapSelector.options[(int) PhotonNetwork.CurrentRoom.CustomProperties["MapSerial"]].text + '\n'
			                  + "游戏模式  " 
			                  + LocalSettings.ModeSelector.options[(int) PhotonNetwork.CurrentRoom.CustomProperties["ModeSerial"]].text + '\n'
			                  + "最大人数  " 
			                  + LocalSettings.PlayerSelector.options[(int) PhotonNetwork.CurrentRoom.CustomProperties["MaxPlayer"]].text + '\n'
			                  + "目标分数  " 
			                  + LocalSettings.TargetSelector.options[(int) PhotonNetwork.CurrentRoom.CustomProperties["TargetKilling"]].text;
			_playerInfoText = "玩家姓名"+ "   " + "选择角色" + "   " + "准备状态"+'\n';
			foreach (Player playerinroom in PhotonNetwork.PlayerList)
			{
				if (playerinroom.IsMasterClient)
				{
					_playerInfoText += playerinroom.NickName+ "   " + Enum.GetName(typeof(CharacterType), playerinroom.CustomProperties["Character"]) + "   " + "房主"+'\n';
				}
				else
				{
					_playerInfoText += playerinroom.NickName+ "   " + Enum.GetName(typeof(CharacterType), playerinroom.CustomProperties["Character"]) + "   " 
					                   + ((bool)playerinroom.CustomProperties["IsReady"]?"准备":"未准备")
					                                                                   +'\n';
				}
			}	
			PlayerNum.text = PhotonNetwork.PlayerList.Length + "/" + Connection.MaxPlayer + "位玩家";
			if (PhotonNetwork.PlayerList.Length == Connection.MaxPlayer)
			{
				Connection.MessageShow("房间人数已满");//StartGame();
			}
			InfoLists.text = _playerInfoText;
			CurrentInfo.text = _roomSettingText;
		}

		public void RefreshLobby()
		{
			CurrentInfo.text =   "在线人数      " + PhotonNetwork.CountOfPlayers+"人" + '\n'
			                   + "正在匹配      " + PhotonNetwork.CountOfPlayersOnMaster+"人" + '\n'
			                   + "房间数量      " + PhotonNetwork.CountOfRooms+"人" + '\n';
			_roomListInfoText = "";
			if (_roomInfos != null)
			{
				foreach (RoomInfo roomInfo in _roomInfos)
				{
					_roomListInfoText +=roomInfo.Name + "的房间" +"    " + roomInfo.PlayerCount + "/" + roomInfo.MaxPlayers+"人" + '\n';
				}
			}
			InfoLists.text = _roomListInfoText;
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

		public void ChooseCharacter()
		{
			PlayerPrefs.DeleteKey("Charactertype");
			foreach (Toggle t in _characters)
			{
				if (t.isOn)
				{
					switch (t.name)
					{
						case "ShooterToggle":
							
							//PlayerPrefs.SetInt("Charactertype", 0);
							LocalSettings.SetCharacter(CharacterType.Circle);
							break;
						case "RusherToggle":
							
							//PlayerPrefs.SetInt("Charactertype", 1);
							LocalSettings.SetCharacter(CharacterType.Diamond);
							break;
					}
					break;
				}
			}
			RefreshRoom();
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
				yield return new WaitForSeconds(1f);
			}
		}
	}


}
