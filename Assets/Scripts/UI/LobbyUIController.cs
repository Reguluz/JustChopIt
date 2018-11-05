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
		private String _roomInfoText;
		public GameObject RoomModeMenu;
		
		
		[Header("Room")]
		public GameObject CharacterList;	//选择角色菜单
		public ToggleGroup CharacterlistToggle;
		public Text PlayerNum;
		private String _playerInfoText;
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
			LocalSettings = GameObject.Find("GameSettings").GetComponent<GameSettings>();
			NickNameInput.transform.GetChild(0).GetComponent<Text>().text = AccountInfo.Nickname ?? "输入昵称";
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
						Match.GetComponentInChildren<Text>().text = "未准备";
						CharacterlistToggle.allowSwitchOff = true;
					}
					else
					{
						LocalSettings.SetReady(true); //准备
						Match.GetComponentInChildren<Text>().text = "准备";
						CharacterlistToggle.allowSwitchOff = false;
					}
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
			Connection.MaxPlayer = LocalSettings.GetRoomSetting().MaxPlayer;
			Connection.CreateRoom();
		}

		public void JoinRoom()
		{
			
			CharacterList.SetActive(true);
			NickNameInput.SetActive(false);
			//CurrentInfo.text = "当前房间内人数" + PhotonNetwork.CurrentRoom.PlayerCount + '\n';
			PlayerPrefs.SetInt("Charactertype",0);
			if (PhotonNetwork.IsMasterClient)
			{
				Match.GetComponentInChildren<Text>().text = "开始游戏";
				RoomBtn.GetComponentInChildren<Text>().text = "返回大厅";
			}
			else
			{
				Match.GetComponentInChildren<Text>().text = "未准备";
				RoomBtn.GetComponentInChildren<Text>().text = "返回大厅";
			}
			RefreshRoom();
			
		}

		public void JoinLobby()
		{
			Match.gameObject.SetActive(true);
			CharacterList.SetActive(false);
			NickNameInput.SetActive(true);
			Match.GetComponentInChildren<Text>().text = "随机加入房间";
			RoomBtn.GetComponentInChildren<Text>().text = "创建房间";
			InfoLists.text = _roomInfoText;
		}

		public void RefreshRoom()
		{
			_playerInfoText = LocalSettings.MapSelector.captionText.text + "  " + LocalSettings.ModeSelector.captionText.text + "  最大人数" + LocalSettings.PlayerSelector.captionText.text + "  目标分数" + LocalSettings.TargetSelector.captionText.text+'\n';
			_playerInfoText += "玩家姓名"+ "   " + "选择角色" + "   " + "准备状态"+'\n';
			foreach (Player playerinroom in PhotonNetwork.PlayerList)
			{
				if (playerinroom.IsMasterClient)
				{
					_playerInfoText += playerinroom.NickName+ "   " + Enum.GetName(typeof(CharacterType), playerinroom.CustomProperties["Character"]) + "   " + "房主"+'\n';
				}
				else
				{
					_playerInfoText += playerinroom.NickName+ "   " + Enum.GetName(typeof(CharacterType), playerinroom.CustomProperties["Character"]) + "   " + playerinroom.CustomProperties["IsReady"]+'\n';
				}
			}	
			PlayerNum.text = PhotonNetwork.PlayerList.Length + "/" + Connection.MaxPlayer + "位玩家";
			if (PhotonNetwork.PlayerList.Length == Connection.MaxPlayer)
			{
				Connection.MessageShow("房间人数已满");//StartGame();
			}
			InfoLists.text = _playerInfoText;
		}

		public void RefreshLobby()
		{
			CurrentInfo.text = "在线人数" + PhotonNetwork.CountOfPlayers + '\n'
			                   + "正在匹配人数" + PhotonNetwork.CountOfPlayersOnMaster + '\n'
			                   + "房间数量" + PhotonNetwork.CountOfRooms + '\n';
			_roomInfoText = "";
			if (_roomInfos != null)
			{
				foreach (RoomInfo roomInfo in _roomInfos)
				{
					_roomInfoText +=roomInfo.Name + "的房间" +"    " + roomInfo.PlayerCount + "/" + roomInfo.MaxPlayers+"人" + '\n';
				}
			}
			InfoLists.text = _roomInfoText;
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
						case "CircleToggle":
							
							//PlayerPrefs.SetInt("Charactertype", 0);
							LocalSettings.SetCharacter(CharacterType.Circle);
							break;
						case "DiamondToggle":
							
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
				yield return new WaitForSeconds(2f);
			}
		}
	}


}
