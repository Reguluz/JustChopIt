using System.Collections;
using System.Collections.Generic;
using Cameras;
using GamePlayer;
using UnityEngine;

public class InitTest : MonoBehaviour {



	public GameObject MainCamera;

	public UIController GameCanvas;
	//public Camera MainCamera;

	public GameObject _localPlayer;

	// Use this for initialization
	private void Awake()
	{
		
	}

	

	void Start()
	{
		Debug.Log("选择序号"+PlayerPrefs.GetInt("Charactertype"));
		MainCamera.GetComponent<CameraFollower>().GamerObject = _localPlayer;
		_localPlayer.GetComponent<MoveController>().Touch = GameCanvas.EasyTouchMove;
		GameCanvas.PlayerProperties = _localPlayer.GetComponent<PlayerProperties>();
		_localPlayer.GetComponent<GamePlayerController>().SetSkillButton();
		//_localPlayer.GetComponent<PlayerSetup>().PlayerCamera = MainCamera.GetComponent<Camera>();
	}

	// Use this for initialization

	// Update is called once per frame
	void Update () {
		
	}

	
}
