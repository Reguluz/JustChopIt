using GamePlayer;
using Photon.Pun;
using UnityEngine;

namespace Items
{
    public class Items:MonoBehaviour
    {
        public ItemCreator Owner;
        
        public Bufftype Bufftype;
        public void SetOwner(ItemCreator owner)
        {
            Owner = owner;
            transform.parent = Owner.gameObject.transform;
            transform.localPosition = Vector3.zero;
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



                    //other.transform.parent.GetComponent<GamePlayerController>().AddBuff(Bufftype);
                    //原句
                    //other.GetComponent<MoveController>().GetBuff(0.2f,10);
                    if (PhotonNetwork.IsMasterClient)
                    {
                    Owner.SetEmpty();
                    PhotonNetwork.Destroy(this.gameObject);
                }
            }
        }
        
    }
}