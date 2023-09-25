using System;

namespace Unity1Week.rhythm_battle.Applications.Enum
{
    public enum Judge
    {
        Non,
        Perfect,
        Good,
        Normal,
        Miss,
    }

    public static class JudgeExtension
    {
        public static float Score(this Judge self)
        {
            return self switch
            {
                Judge.Perfect => 1,
                Judge.Good => 0.6f,
                Judge.Normal => 0.2f,
                Judge.Miss => 0,
                _ => throw new ArgumentOutOfRangeException(nameof(self), self, null)
            };
        }
    }
}