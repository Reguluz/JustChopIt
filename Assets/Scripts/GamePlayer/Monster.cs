using Photon.Pun;
using UnityEngine;

namespace GamePlayer
{
    public class Monster:MonoBehaviour,IPunInstantiateMagicCallback
    {
        public int Level;
        

        private PhotonView _photonView;
        private MapController _mapController;

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
           
        }

        private void OnEnable()
        {
            
        }

        private void FixedUpdate()
        {
            Collider[] onEffect=Physics.OverlapSphere(transform.position, 5);
            foreach (Collider obj in onEffect)
            {
                if (obj.CompareTag("Player"))
                {
                    Vector3 deltaPos = transform.position - obj.gameObject.transform.position;
                    Vector3 force = deltaPos.normalized * 1 * 10;
                    obj.transform.parent.GetComponent<Rigidbody>().AddForce(force);
                }
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponent<PhotonView>().RPC("Hurt",RpcTarget.All,DamageType.Hard,_photonView.ViewID);
            }
        }

        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            _mapController = GameObject.Find("Map").GetComponent<MapController>();
            _mapController.SetEffectCenter(this.gameObject);
            _mapController.EffectSwitch(true);
            Invoke(nameof(DestoryMonster),15f);
        }
        
        private void DestoryMonster()
        {
            _mapController.EffectSwitch(false);
            if (PhotonNetwork.IsMasterClient)
            {
               
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
    }
}