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
			MessageShow("随机加入房间中");
			PhotonNetwork.JoinRandomRoom(); //随机加入房间
		}

		public void CreateRoom()
		{
			_lobbyUiController.LocalSettings.SetNickName();
			PhotonNetwork.CreateRoom(PhotonNetwork.LocalPlayer.NickName); //创建名为1的房间
			
		}

		public void LeaveRoom()
		{
			PhotonNetwork.LeaveRoom();
			_lobbyUiController.JoinLobby();
		}
		
		public override void OnConnected()
		{
			MessageShow("正在连接服务器");
		}

		public override void OnDisconnected(DisconnectCause cause)
		{
			_reconnectime++;
			MessageShow("连接服务器失败,正在重新连接第"+_reconnectime+"次");
			PhotonNetwork.Reconnect();
		}
		
		public override void OnConnectedToMaster()
		{
			MessageShow("连入服务器成功");
			_lobbyUiController.ConnectSucceed();
			PhotonNetwork.JoinLobby();
		}


		public override void OnJoinedLobby()
		{
			
		}

		public override void OnJoinRandomFailed(short returnCode, string message)
		{
			MessageShow("找不到可进入房间");
			
		}

		public override void OnCreatedRoom() //创建房间时调用（只要创建一定会加入
		{
			MessageShow("创建新房间");
			Debug.Log("Create Room");
		}

		public override void OnJoinedRoom() //加入房间时调用
		{
			MessageShow("加入房间");
			_lobbyUiController.LocalSettings.PlayerSettingInitial();
			_lobbyUiController.JoinRoom();
			PhotonNetwork.AutomaticallySyncScene = true; //开启场景同步
		}

		
		public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer) //每次有玩家进入房间时调用
		{
			MessageShow("玩家" + newPlayer.NickName + "进入房间");
			_lobbyUiController.RefreshRoom();
		}

		public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
		{
			MessageShow("玩家" + otherPlayer.NickName + "离开房间");
			_lobbyUiController.RefreshRoom();
		}

		
		public void StartGame()
		{
			PhotonNetwork.CurrentRoom.IsVisible = false;
			StartCoroutine(LoadGame());
		}


		IEnumerator LoadGame()
		{
			//PlayerPrefs.SetInt("Charactertype",(int) _lobbyUiController.Chosentype);
			//photonView.RPC("MessageShow",RpcTarget.All,"倒计时3");
			MessageShow("倒计时3");
			yield return new WaitForSeconds(1f);
			
			//photonView.RPC("MessageShow",RpcTarget.All,"倒计时2");
			MessageShow("倒计时2");
			yield return new WaitForSeconds(1f);
			
			//photonView.RPC("MessageShow",RpcTarget.All,"倒计时1");
			MessageShow("倒计时1");
			yield return new WaitForSeconds(1f);
			if (PhotonNetwork.IsMasterClient) //检测是否为主机（Photon会自主选择房间内最优用户作为主机
			{
				PhotonNetwork.LoadLevel("GameScene3D");
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

		[PunRPC]
		public void MessageShow(string sentence)
		{
			State.text = sentence;
		}
	}
}