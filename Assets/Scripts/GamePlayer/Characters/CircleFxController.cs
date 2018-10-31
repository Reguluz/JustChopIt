using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CircleFxController : MonoBehaviour {

	public _2dxFX_Hologram3 Dodgefx;
	// Use this for initialization
	void Awake () {
		Dodgefx.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	[PunRPC]
	public void DodgeFx(bool isenable)
	{
		if (isenable)
		{
			Dodgefx.enabled = true;
		}
		else
		{
			Dodgefx.enabled = false;
		}
	}
}
