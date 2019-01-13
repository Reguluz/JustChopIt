using UnityEngine;

namespace GamePlayer.Characters
{
    [RequireComponent(typeof(AudioSource))]
    public class SorcererFxController:CharacterFxController
    {
        public ParticleSystem ChantfxParticle;
        private AudioSource fxSource;
        public ParticleSystem ReleasefxParticle;
        
        public AudioClip ChantClip;
        public AudioClip ReleaseClip;
        
        void OnEnable()
        {
            ChantfxParticle.Stop();
            ReleasefxParticle.Stop();
            fxSource = GetComponent<AudioSource>();
        }
        public override void PlayFx(string fxname)
        {
            switch (fxname)
            {
                case "Dead": DeadFx();
                    break;
                case "Relieve":RelieveFx();
                    break;
                case "Rebuild":RebuildFx();
                    break;
                case "SkillRelease":SkillRelease();
                    break;
				
                case "Chant": ChantFx();
                    break;
                case "MatrixRelease": MatrixReleaseFx();
                    break;
                
            }
        }

        private void ChantFx()
        { 
            ChantfxParticle.Play();
            fxSource.clip = ChantClip;
            fxSource.loop = true;
            fxSource.Play();
            
        }

        private void MatrixReleaseFx()
        {
            ChantfxParticle.Stop();
            ReleasefxParticle.Play();
            fxSource.loop = false;
            fxSource.clip = ReleaseClip;
            fxSource.Play();
        }
    }
}