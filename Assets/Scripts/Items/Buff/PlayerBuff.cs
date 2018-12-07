using System;
using GamePlayer;
using GamePlayer.Characters;
using Photon.Pun;
using UnityEngine;

namespace Items.Buff
{
    [Serializable]
    public class PlayerBuff
    {
        public GameObject Owner;
        public Bufftype Bufftype;
        protected CharacterData Coefficient = new CharacterData(0,0,0);
        public float Maxtime;
        public float Interval;
        protected GameObject Effect;

        public virtual void GetBuff(GamePlayerController player)
        {
            
        }

        public  void RemoveBuff(GamePlayerController player)
        {
            player.BuffCo?.Sub(Coefficient);
            if (Owner.GetPhotonView().IsMine)
            {
                PhotonNetwork.Destroy(Effect); 
            }
                
        }

        public bool CheckOvertime()
        {
            return (!(Interval < Maxtime));
        }

    }
}