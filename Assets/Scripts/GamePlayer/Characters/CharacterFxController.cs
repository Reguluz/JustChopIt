using System;
using Photon.Pun;
using UnityEngine;

namespace GamePlayer.Characters
{
    [RequireComponent(typeof(AudioListener))]
    public class CharacterFxController:MonoBehaviour
    {
        public Material Default;
        public Material Dodge;
        public Material Vanish;
        public Material Relieve;
        
        public MeshRenderer Mesh;
        public ParticleSystem SkillReleaseParticle;
        public ParticleSystem DeadParticle;
        private AudioSource _skillReleaseClip;
        private AudioSource _deadfxClip;


        private void Awake()
        {
            _skillReleaseClip = SkillReleaseParticle.gameObject.GetComponent<AudioSource>();
            _deadfxClip = DeadParticle.gameObject.GetComponent<AudioSource>();
        }

        public virtual void PlayFx(string fxname)
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
            }
        }

        public virtual void StopFx(string fxname)
        {
            
        }
        
        //[PunRPC]
        public void DeadFx()
        {		
            Mesh.material = Vanish;
            DeadParticle.Play();
            _deadfxClip.Play();
        }

        //[PunRPC]
        public void RelieveFx()
        {
            Mesh.material = Relieve;
        }
        
        //[PunRPC]
        public void DodgeFx()
        {		
            Mesh.material = Dodge;
        }

        //[PunRPC]
        public void RebuildFx()
        {
            Mesh.material = Default;
        }
        void OnEnable()
        {
            SkillReleaseParticle.Stop();
           
        }
        public void SkillRelease()
        {
            SkillReleaseParticle.Play();
            _skillReleaseClip.Play();
        }
    }
}