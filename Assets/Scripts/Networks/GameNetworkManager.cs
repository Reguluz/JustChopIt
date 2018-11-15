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
			_localPlayer = PhotonNetwork.Instantiate(PlayerPrefab[(int) PhotonNetwork.LocalPlayer.CustomProperties["Character"]].name, new Vector3(0,1,0), Quaternion.identity, 0);
			_localPlayer.GetComponent<PlayerProperties>().Map = MapController;
			//_localPlayer.GetComponent<PlayerProperties>()._board = GameBoard;
			MainCamera.GetComponent<CameraFollower>().GamerObject = _localPlayer;
			_localPlayer.GetComponent<MoveController>().Touch = GameCanvas.EasyTouchMove;
			GameCanvas.PlayerProperties = _localPlayer.GetComponent<PlayerProperties>();
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

