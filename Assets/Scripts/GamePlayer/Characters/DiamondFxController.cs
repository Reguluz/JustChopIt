﻿using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GamePlayer.Characters
{
	public class DiamondFxController : CharacterFxController {

		public ParticleSystem RushfxParticle;

		void OnEnable()
		{
			RushfxParticle.Stop();
		}
		// Update is called once per frame
		void Update () {
			
		}
		
		
		//[PunRPC]
		public void RushFx(bool isenable)
		{
			Debug.Log("Rush is "+isenable);
			if(isenable){
				RushfxParticle.Play();
			}else{
				RushfxParticle.Stop();
			}
			
		}
	}
}