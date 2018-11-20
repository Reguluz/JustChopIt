using Photon.Pun;
using UnityEngine;

namespace GamePlayer.Characters
{
	public class CircleFxController : CharacterFxController
	{
		public ParticleSystem ShootFxParticle;
		

		/// <summary>
		/// Awake is called when the script instance is being loaded.
		/// </summary>
		void Awake()
		{
			ShootFxParticle.Stop();
		}
		
		//[PunRPC]
		public void ShootFx()
		{
			ShootFxParticle.Play();
		}
		
	}
}
