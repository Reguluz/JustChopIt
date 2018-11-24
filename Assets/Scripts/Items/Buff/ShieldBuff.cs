using GamePlayer;
using Photon.Pun;
using UnityEngine;

namespace Items.Buff
{
    public class ShieldBuff:PlayerBuff
    {
        
        public ShieldBuff()
        {
            Bufftype = Bufftype.Shield;
            Coefficient.MoveSpeed = 0;
            Coefficient.RotateSpeed = 0;
            Maxtime = 10;
            Interval = 0;
        }

        public override void GetBuff(GamePlayerController player)
        {
            Owner = player.gameObject;
            player.BuffCo.Add(Coefficient);
            if (Owner.GetPhotonView().IsMine)
            {
                Effect = PhotonNetwork.Instantiate("ShieldEffect", Vector3.zero, Quaternion.identity);
            }
            
            //tempeffect.GetComponentInChildren<ParticleSystem>().Play();
        }

        

        
    }
}