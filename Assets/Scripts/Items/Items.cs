using GamePlayer;
using Photon.Pun;
using UnityEngine;

namespace Items
{
    public class Items:MonoBehaviour
    {
        private ItemCreator Owner;
        
        public Bufftype Bufftype;
        public void SetOwner(ItemCreator owner)
        {
            Owner = owner;
        }

        public void SetEmpty()
        {
            Owner.IsEmpty = true;
        }
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("PlayerModel"))
            {
                PhotonView pv = other.transform.parent.GetComponent<PhotonView>();
                //给目标玩家buff
                pv.RPC("AddBuff",RpcTarget.All,Bufftype);
                //原句
                //other.GetComponent<MoveController>().GetBuff(0.2f,10);
                SetEmpty();
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
        
    }
}