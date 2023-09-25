using Unity1Week.rhythm_battle.Entity.Game;

namespace Unity1Week.rhythm_battle.UseCase
{
    public sealed class JudgeUseCase
    {
        private readonly TimeEntity _timeEntity;
        private readonly JudgeEntity _judgeEntity;
        private readonly LifeEntity _lifeEntity;

        public JudgeUseCase(TimeEntity timeEntity, JudgeEntity judgeEntity, LifeEntity lifeEntity)
        {
            _timeEntity = timeEntity;
            _judgeEntity = judgeEntity;
            _lifeEntity = lifeEntity;
        }

        public void Tap()
        {
            if (!_judgeEntity.TryTap(_timeEntity.Time, out var note)) return;
            _lifeEntity.Judgement(note);
        }
    }
}