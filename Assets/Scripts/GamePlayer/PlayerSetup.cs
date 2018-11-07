using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GamePlayer
{
	public class PlayerSetup : MonoBehaviour
	{

		//public Camera PlayerCamera;

		public Behaviour[] StuffNeedDisable;//所有在本物体中   在其他玩家视角里   不能使用的  对自身单位的  控制脚本，比如移动脚本
		public GameObject[] ObjectNeedDisable;

		private PhotonView _photonView;
		// Use this for initialization
		void Start ()
		{
			_photonView = GetComponent<PhotonView>();
			CheckPhotonView();
		}
	
		// Update is called once per frame
		void Update () {
		
		}

		void CheckPhotonView()
		{
			if (!_photonView.IsMine)
			{
				//PlayerCamera.enabled = false;
				foreach (Behaviour behaviour in StuffNeedDisable)
				{
					behaviour.enabled = false;
				}

				foreach (GameObject gameObjects in ObjectNeedDisable)
				{
					gameObjects.SetActive(false);
				}
			}
		}
	}


}
