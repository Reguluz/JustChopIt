using Photon.Pun;
using UnityEngine;

namespace GamePlayer.Characters
{
    public class CharacterFxController:MonoBehaviour
    {
        public Material Default;
        public Material Dodgefx;
        public Material Vanish;
        public Material Relieve;
        
        public MeshRenderer Mesh;

        [PunRPC]
        public void DeadFx()
        {		
            Mesh.material = Vanish;
        }

        [PunRPC]
        public void RelieveFx()
        {
            Mesh.material = Relieve;
        }
        
        [PunRPC]
        public void DodgeFx()
        {		
            Mesh.material = Dodgefx;
        }

        [PunRPC]
        public void FxRebuild()
        {
            Mesh.material = Default;
        }
    }
}