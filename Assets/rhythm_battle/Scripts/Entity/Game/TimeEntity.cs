
namespace Unity1Week.rhythm_battle.Entity.Game
{
    public sealed class TimeEntity
    {
        private float _length;
        public float Time;

        public bool IsFinish => Time >= _length;

        public void Initialize(float length)
        {
            _length = length;
        }

        public void FixUpdate(float deltaTime)
        {
            Time += deltaTime;
        }
    }
}