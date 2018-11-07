namespace System
{
    [Serializable]
    public struct GameParameters
    {
        public int MapSerial;   //0.示例
        public int ModeSerial;    //0.无额外系统 1.有道具 2.有地图boss 3.既有道具也有地图boss
        public int MaxPlayer;   //0.10   1.8   2.6   3.4
        public int TargetKilling;   //0.10  1.20    2.30    3.40
    }
}