using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamePlayer
{

	public class MoveController : MonoBehaviour
	{
		private float _local = 5f;


		public EasyTouchMove Touch;
		[HideInInspector]
		public float SpeedLevel;
		[HideInInspector]
		public float RotateLevel;

		//private Vector3 _forwardposition;

		//private Quaternion _rawRotation;
		//private Quaternion _lookatRotation;
		//public float PerSecondRotate = 20f;
		//float _lerpSpeed = 0.0f;
		//float _lerpTm = 0.0f;

		// Use this for initialization
		void Start()
		{
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

			Vector3 direction = new Vector3(hor, 0, ver);
			if (direction != Vector3.zero)
			{
				//控制转向
				transform.forward = Vector3.Lerp(transform.forward, direction, RotateLevel*0.002f);
				
				Debug.DrawLine(transform.position, transform.position + direction,    Color.cyan);
				Debug.DrawLine(transform.position, transform.position + transform.forward, Color.red);
				//向前移动
				transform.Translate(transform.forward * Time.deltaTime * 3 * SpeedLevel, Space.World);
			}
		}


	}
}