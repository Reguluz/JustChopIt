using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class DiamondFxController : MonoBehaviour {

	public _2dxFX_BurningFX Rushfx;
	// Use this for initialization
	void Awake () {
		Rushfx.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	[PunRPC]
	public void RushFx(bool isenable)
	{
		if (isenable)
		{
			Rushfx.enabled = true;
		}
		else
		{
			Rushfx.enabled = false;
		}
	}
}
