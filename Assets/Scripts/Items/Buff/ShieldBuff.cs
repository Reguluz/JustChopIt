using GamePlayer;

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
            player.BuffCo.Add(Coefficient);
        }

        public override void RemoveBuff(GamePlayerController player)
        {
            player.BuffCo?.Sub(Coefficient);
        }
    }
}