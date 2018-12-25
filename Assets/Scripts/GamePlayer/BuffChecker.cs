using Items;
using Items.Buff;

namespace GamePlayer
{
    public class BuffChecker
    {
        public static PlayerBuff Check(Bufftype bufftype)
        {
            switch (bufftype)
            {
                case    Bufftype.Speedup:
                    return new SpeedupBuff();
                case    Bufftype.Shield:
                    return new ShieldBuff();
            }
            return new PlayerBuff();
        }
    }
}