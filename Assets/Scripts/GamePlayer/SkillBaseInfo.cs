using System;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlayer
{
    [Serializable]
    public class SkillBaseInfo
    {
        public int Pos;
        public float CoolDown;
        public Sprite SkillImage;
        public String SkillName;
        public String SkillInfo;
        public SkillType SkillType;

    }
    
    
}