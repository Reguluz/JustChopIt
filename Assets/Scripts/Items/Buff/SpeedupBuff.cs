using GamePlayer;
using GamePlayer.Characters;
using Photon.Pun;
using UnityEngine;

namespace Items.Buff
{
    public class SpeedupBuff:PlayerBuff
    {
        
        public SpeedupBuff()
        {
            Bufftype = Bufftype.Speedup;
            Coefficient.MoveSpeed = 0.3f;
            Coefficient.RotateSpeed = 0.3f;
            Maxtime = 10;
            Interval = 0;
        }

        public override void GetBuff(GamePlayerController player)
        {
            Owner = player.gameObject;
            player.BuffCo.Add(Coefficient);
            if (Owner.GetPhotonView().IsMine)
            {
                Effect = PhotonNetwork.Instantiate("SpeedUpEffect", Vector3.zero, Quaternion.identity);
            }
            
        }

        
    }
}