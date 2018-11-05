﻿using System;
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

		private GameObject _localPlayer;

		private GameSettings _localSettings;
		// Use this for initialization
		private void Awake()
		{
			_localSettings = GameObject.Find("GameSettings").GetComponent<GameSettings>();
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
			_localPlayer = PhotonNetwork.Instantiate(PlayerPrefab[(int)_localSettings.Chosentype].name, new Vector3(0,1,0), Quaternion.identity, 0);
			//_localPlayer.GetComponent<PlayerProperties>().Board = GameBoard;
			MainCamera.GetComponent<CameraFollower>().GamerObject = _localPlayer;
			_localPlayer.GetComponent<MoveController>().Touch = GameCanvas.EasyTouchMove;
			GameCanvas.PlayerProperties = _localPlayer.GetComponent<PlayerProperties>();
			_localPlayer.GetComponent<PhotonView>().RPC("ComponentInit",RpcTarget.All);
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

		private void OnApplicationQuit()
		{
			PhotonNetwork.Disconnect();
		}
	}


}

