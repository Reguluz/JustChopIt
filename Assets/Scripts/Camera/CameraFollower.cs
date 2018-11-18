using System.Collections;
using System.Collections.Generic;
using GamePlayer;
using UnityEngine;

namespace Cameras
{
	public class CameraFollower : MonoBehaviour
	{

		public GameObject GamerObject;
		private PlayerProperties _gamerproperty;
		private Vector3 _offset = new Vector3(0,150,-25f);


		private float _smoothTime = 0.5f;
		private float _xVelocity = 0.0f;
		private float _yVelocity = 0.0f;

		private Vector3 _velocity;
		// Use this for initialization
		void Start () {
			//手机不息屏
			Screen.sleepTimeout = SleepTimeout.NeverSleep;
			_gamerproperty = GamerObject.GetComponent<PlayerProperties>();
		}
	
		// Update is called once per frame
		void LateUpdate () {
			
			if (_gamerproperty.StateType != PlayerStateType.Dead && _gamerproperty.StateType != PlayerStateType.Relieve)
			{
				transform.position = _offset + GamerObject.transform.position;
			}else if (_gamerproperty.StateType == PlayerStateType.Dead)
			{
				//死亡时摄像机不移动
			}else if (_gamerproperty.StateType == PlayerStateType.Relieve)
			{
				//复活后摄像机缓动至复活位置
				transform.position = Vector3.SmoothDamp(transform.position,GamerObject.transform.position + _offset,ref _velocity,_smoothTime);
			}
		}
	}


}
