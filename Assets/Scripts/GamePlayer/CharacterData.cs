using System;

namespace GamePlayer
{
    [Serializable]
    public class CharacterData
    {
        public float MoveSpeed;
        public float RotateSpeed;
        public float Scale;

        public CharacterData(float moveSpeed, float rotateSpeed, float scale)
        {
            MoveSpeed = moveSpeed;
            RotateSpeed = rotateSpeed;
            Scale = scale;
        }

        public void Add(CharacterData data)
        {
            MoveSpeed += data.MoveSpeed;
            RotateSpeed += data.RotateSpeed;
            Scale += data.Scale;
        }
        
        public void Sub(CharacterData data)
        {
            MoveSpeed -= data.MoveSpeed;
            RotateSpeed -= data.RotateSpeed;
            Scale -= data.Scale;
        }
    }
}