using UnityEngine;

namespace GamePlayer.Characters
{
    public class SorcererFxController:CharacterFxController
    {
        public ParticleSystem ChantfxParticle;
        public ParticleSystem ReleasefxParticle;
        
        void OnEnable()
        {
            ChantfxParticle.Stop();
            ReleasefxParticle.Stop();
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
        }

        private void MatrixReleaseFx()
        {
            ReleasefxParticle.Play();
        }
    }
}