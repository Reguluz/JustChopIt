using System;
using GamePlayer;
using UnityEngine;

namespace Items.Buff
{
    [Serializable]
    public class PlayerBuff
    {
        public Bufftype Bufftype;
        protected CharacterData Coefficient = new CharacterData(0,0,0);
        public float Maxtime;
        public float Interval;
        protected ParticleSystem Effect;

        public virtual void GetBuff(GamePlayerController player)
        {
            
        }

        public virtual void RemoveBuff(GamePlayerController player)
        {
            
        }

        public bool CheckOvertime()
        {
            return (!(Interval < Maxtime));
        }

    }
}