﻿using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class NickNameInput : MonoBehaviour
{
	private InputField _inputField;
	// Use this for initialization
	void Start ()
	{
		_inputField = GetComponent<InputField>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetNickName()
	{
		PhotonNetwork.LocalPlayer.NickName = _inputField.text;
	}
}
