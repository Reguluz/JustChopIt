using System;
using System.Collections;
using System.Collections.Generic;
using Cameras;
using GamePlayer;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Networks
{
	public class GameNetworkManager : MonoBehaviour
	{

		public GameObject[] PlayerPrefab;

		public GameObject MainCamera;

		public UIController GameCanvas;

		public GameBoard GameBoard;
		//public Camera MainCamera;
		public MapController MapController;

		private GameObject _localPlayer;

		public GameObject ItemFactory;

		public ObjectPool ObjectpoolController;
		// Use this for initialization
		private void Awake()
		{
		}

		private void OnEnable()
		{
			SceneManager.sceneLoaded += OnSceneLoaded;
		}
	
		private void OnDisable()
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
		}

		void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			Debug.Log("选择序号"+PlayerPrefs.GetInt("Charactertype"));


			
			//生成玩家角色
			_localPlayer = PhotonNetwork.Instantiate(PlayerPrefab[(int) PhotonNetwork.LocalPlayer.CustomProperties["Character"]].name, new Vector3(0,1,0), Quaternion.identity, 0);
			//给玩家角色	绑定	Map
			_localPlayer.GetComponent<PlayerProperties>().Map = MapController;
			//给玩家角色	绑定	UI
			_localPlayer.GetComponent<MoveController>().Touch = GameCanvas.EasyTouchMove;
			//_localPlayer.GetComponent<PlayerProperties>()._board = GameBoard;
			//给摄像机	绑定	角色
			MainCamera.GetComponent<CameraFollower>().GamerObject = _localPlayer;
			//给UI		绑定	角色信息
			GameCanvas.PlayerProperties = _localPlayer.GetComponent<PlayerProperties>();
			//角色信息组件初始化
			_localPlayer.GetComponent<PhotonView>().RPC("ComponentInit",RpcTarget.All);
			
			
			Invoke(nameof(OpenBlank),1f);
			//_localPlayer.GetComponent<PlayerSetup>().PlayerCamera = MainCamera.GetComponent<Camera>();
		}

		// Use this for initialization
		void Start ()
		{
			//_show.transform.parent = MainCamera.transform;
		}
	
		// Update is called once per frame
		void Update () {
		
		}

		private void OpenBlank()
		{
			GameBoard.Untouchblank.SetActive(false);
		}


		private void OnApplicationQuit()
		{
			PhotonNetwork.Disconnect();
		}
	}


}

