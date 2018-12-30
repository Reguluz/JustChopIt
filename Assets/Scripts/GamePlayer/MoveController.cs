using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GamePlayer
{

	public class MoveController : MonoBehaviour
	{

		
		public EasyTouchMove Touch;
		[HideInInspector]
		public float SpeedLevel;

		[HideInInspector]
		public float RotateLevel;

		private Rigidbody _body;
		//private Vector3 _forwardposition;

		//private Quaternion _rawRotation;
		//private Quaternion _lookatRotation;
		//public float PerSecondRotate = 20f;
		//float _lerpSpeed = 0.0f;
		//float _lerpTm = 0.0f;

		// Use this for initialization
		void Awake()
		{
			_body = GetComponent<Rigidbody>();
		}

		// Update is called once per frame
		void Update()
		{
			MoveByVector3Lerp();
		}

		private void MoveByVector3Lerp()
		{
			float hor = Touch.Horizontal;
			float ver = Touch.Vertical;
			if (SpeedLevel > 2)
			{
				SpeedLevel = 2;
			}
			Vector3 direction = new Vector3(hor, 0, ver);
			if (direction != Vector3.zero)
			{
				//控制转向
				transform.forward = Vector3.Lerp(transform.forward, direction, RotateLevel*0.002f);
				
				Debug.DrawLine(transform.position, transform.position + direction,    Color.cyan);
				Debug.DrawLine(transform.position, transform.position + transform.forward, Color.red);
				//向前移动
				//_body.velocity = transform.position+transform.forward * Time.deltaTime * 30 * SpeedLevel;
				_body.MovePosition(transform.position+transform.forward * Time.deltaTime * 30 * SpeedLevel);
				//transform.Translate(transform.forward * Time.deltaTime * 3 * SpeedLevel, Space.World);
			}
		}

		/*[PunRPC]
		public void GetBuff(float coefficient, float effecttime)
		{
			ISpeedLevel += coefficient;
			Invoke(nameof(RebuildBuff),effecttime);
		}

		public void RebuildBuff()
		{
			ISpeedLevel = 1;
		}*/


	}
}