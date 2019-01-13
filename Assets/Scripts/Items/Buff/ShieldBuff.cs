using GamePlayer;
using GamePlayer.Characters;
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
            player.DamageCalculator += ShieldChecker;
            if (Owner.GetPhotonView().IsMine)
            {
                Effect = PhotonNetwork.Instantiate("ShieldEffect", Vector3.zero, Quaternion.identity);
            }
            
            //tempeffect.GetComponentInChildren<ParticleSystem>().Play();
        }

        public override void RemoveBuff(GamePlayerController player)
        {
            player.DamageCalculator -= ShieldChecker;
            player.BuffCo?.Sub(Coefficient);
            if (Owner.GetPhotonView().IsMine)
            {
                Effect.GetPhotonView().RPC("DestoryEffect",RpcTarget.All);
            }
                
        }
        public bool ShieldChecker(GamePlayerController player, bool iseffected)
        {
            if (iseffected)
            {
                return iseffected;
            }
            else
            {
                player.Buffs.Remove(this);
                RemoveBuff(player);
                return true;
            }
        }

        
    }
}