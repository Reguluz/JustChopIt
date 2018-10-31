using System.Collections;
using System.Collections.Generic;
using Photon;
using Photon.Pun;
using UnityEngine;

public class NetworkTest : MonoBehaviourPunCallbacks {

	// Use this for initialization
	void Start ()
	{
		PhotonNetwork.GameVersion = "0.0.1";
		PhotonNetwork.ConnectUsingSettings();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
