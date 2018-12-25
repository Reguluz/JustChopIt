using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GamePlayer.Characters
{
	public class DiamondFxController : CharacterFxController {

		public ParticleSystem RushfxParticle;
		public TrailRenderer RushTrail;

		void OnEnable()
		{
			RushfxParticle.Stop();
			RushTrail.enabled = false;
		}
		public override void PlayFx(string fxname)
		{
			switch (fxname)
			{
				case "Dead": DeadFx();
					break;
				case "Relieve":RelieveFx();
					break;
				case "Dodge": DodgeFx();
					break;
				case "Rebuild":RebuildFx();
					break;
				case "SkillRelease":SkillRelease();
					break;
				
				case "Rush": RushFx(true);
					break;
			}
		}

		public override void StopFx(string fxname)
		{
			switch (fxname)
			{
				case "Rush": RushFx(false);
					break;
			}
		}
		
		
		//[PunRPC]
		public void RushFx(bool isenable)
		{
			Debug.Log("Rush is "+isenable);
			if(isenable){
				RushfxParticle.Play();
				RushTrail.enabled = true;
			}else{
				RushfxParticle.Stop();
				RushTrail.enabled = false;
			}
			
		}
	}
}