using System;
using System.Collections;
using System.Collections.Generic;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Networks
{

	public class MatchingNetConnect : MonoBehaviourPunCallbacks
	{

		
		public Text State;
		
		private int _reconnectime;
		/*public Button Match;
		public Button RoomBtn;
		

		public Text State;
		public Text Serverping;
		public Text CurrentInfo;
		public Text InfoLists;

		public Text PlayerNum;
		private List<RoomInfo> _roomInfos;
		private Player[] _playersInRoom;
		private String _longInfo;
		private int _reconnectime;*/
		private LobbyUIController _lobbyUiController;


		// Use this for initialization
		void Start()
		{
			PhotonNetwork.ConnectUsingSettings();
			//PhotonNetwork.ConnectToRegion("cn");
			_lobbyUiController = GetComponent<LobbyUIController>();
			_lobbyUiController.LobbyInit();
		}

		// Update is called once per frame
		void Update()
		{
			
		}
		



		public override void OnRoomListUpdate(List<RoomInfo> roomList)
		{
			_lobbyUiController.RefreshLobby();
			_lobbyUiController.RefreshRoomList(roomList);
		}

		public void StartMatching()
		{
			State.text = "随机加入房间中";
			PhotonNetwork.JoinRandomRoom(); //随机加入房间
		}

		public void RoomBtnPressed()
		{
			if (!PhotonNetwork.InRoom)
			{
				PhotonNetwork.CreateRoom(PhotonNetwork.LocalPlayer.NickName); //创建名为1的房间
				
			}
			else
			{
				if (PhotonNetwork.IsMasterClient)
				{
					if (PhotonNetwork.PlayerList.Length > 1)
					{
						StartGame();
					}
					else
					{
						State.text = "人数不足无法开始游戏";
					}
				}
				else
				{
					PhotonNetwork.LeaveRoom();
					_lobbyUiController.JoinLobby();

				}
				
			}
			
		}



		public override void OnConnected()
		{
			State.text = "正在连接服务器";
		}

		public override void OnDisconnected(DisconnectCause cause)
		{
			_reconnectime++;
			State.text = "连接服务器失败,正在重新连接第"+_reconnectime+"次";
			PhotonNetwork.Reconnect();
		}
		
		public override void OnConnectedToMaster()
		{
			State.text = "连入服务器成功";
			_lobbyUiController.ConnectSucceed();
			PhotonNetwork.JoinLobby();
		}


		public override void OnJoinedLobby()
		{
			
		}

		public override void OnJoinRandomFailed(short returnCode, string message)
		{
			State.text = "找不到可进入房间";
			
		}

		public override void OnCreatedRoom() //创建房间时调用（只要创建一定会加入
		{
			State.text = "创建新房间";
			Debug.Log("Create Room");
		}

		public override void OnJoinedRoom() //加入房间时调用
		{
			State.text = "加入房间";
			_lobbyUiController.JoinRoom();
			PhotonNetwork.AutomaticallySyncScene = true; //开启场景同步
		}

		
		public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer) //每次有玩家进入房间时调用
		{
			State.text = "玩家" + newPlayer.NickName + "进入房间";
			_lobbyUiController.RefreshRoom();
		}

		
		private void StartGame()
		{
			PhotonNetwork.CurrentRoom.IsVisible = false;
			StartCoroutine(LoadGame());
		}


		IEnumerator LoadGame()
		{
			PlayerPrefs.SetInt("Charactertype",_lobbyUiController.Chosentype);
			State.text = "倒计时3";
			yield return new WaitForSeconds(1f);
			State.text = "倒计时2";
			yield return new WaitForSeconds(1f);
			State.text = "倒计时1";
			yield return new WaitForSeconds(1f);
			if (PhotonNetwork.IsMasterClient) //检测是否为主机（Photon会自主选择房间内最优用户作为主机
			{
				PhotonNetwork.LoadLevel("GameScene");
			}
			else
			{
				Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
			}
		}



		public void OnApplicationQuit()
		{
			PhotonNetwork.Disconnect();
		}
		
		
	}
}