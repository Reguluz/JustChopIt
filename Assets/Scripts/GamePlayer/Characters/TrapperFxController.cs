using UnityEngine;

namespace GamePlayer.Characters
{
    public class TrapperFxController :CharacterFxController
    {
        public ParticleSystem TrapfxParticle;

        void OnEnable()
        {
            TrapfxParticle.Stop();
        }
        // Update is called once per frame
        void Update () {
			
        }
		
		
        //[PunRPC]
        public void TrapFx()
        {
            TrapfxParticle.Play();
        }
    }
}