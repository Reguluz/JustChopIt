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

        
        //[PunRPC]
        public void DeadFx()
        {		
            Mesh.material = Vanish;
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
        public void FxRebuild()
        {
            Mesh.material = Default;
        }
    }
}