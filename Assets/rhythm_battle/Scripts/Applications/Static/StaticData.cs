namespace Unity1Week.rhythm_battle.Applications.Static
{
    public static class StaticData
    {
        public const float FrameTime = 0.01667f;
        public const float SpawnPosition = -4f;
        private const float DefaultBpm = 120f;
        public static float SpeedLate(int bpm) => bpm / DefaultBpm;

        private const int CounterThreshold = 3;
        public static bool IsCounter(int type) => type > CounterThreshold;

        public static float BGM = 0.5f;
        public static float SE = 0.5f;
    }
}