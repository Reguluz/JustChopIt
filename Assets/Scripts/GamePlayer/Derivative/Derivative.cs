using Photon.Pun;
using UnityEngine;

namespace GamePlayer.Derivative
{
    [RequireComponent(typeof(PhotonView))]
    [RequireComponent(typeof(PhotonTransformView))]
    public class Derivative:MonoBehaviour
    {
        //绑定创建者
        public int CreatorId;
        public PhotonView photonView;

        // Use this for initialization

        
        [PunRPC]
        public void SetOwner(int serial)
        {
            Debug.Log("设置来源为"+serial);
            CreatorId = serial;
        }

        public virtual void DestroySelf()
        {
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
        
        private void OnDisable()
        {
            CancelInvoke(nameof(DestroySelf));
        }

        [PunRPC]
        public void SetDirection(Vector3 direction)
        {
            transform.forward = direction;
        }
    }
}