using System.Collections.Generic;
using GamePlayer;

namespace UI
{
    public class EntryCompare:IComparer<PlayerProperties>
    {
        public int Compare(PlayerProperties x, PlayerProperties y)
        {
            return x.CompareTo(y);
        }
    }
}