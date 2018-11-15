using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GamePlayer.Characters
{
	public class DiamondFxController : CharacterFxController {

		public ParticleSystem RushfxParticle;

		void Awake()
		{
			RushfxParticle.Stop();
		}
		// Update is called once per frame
		void Update () {
			
		}
		
		
		[PunRPC]
		public void RushFx(bool isenable)
		{
			if(isenable){
				RushfxParticle.Play();
			}else{
				RushfxParticle.Stop();
			}
			
		}
	}
}