using Unity1Week.rhythm_battle.Applications.Enum;
using Unity1Week.rhythm_battle.Entity.Game;

namespace Unity1Week.rhythm_battle.UseCase
{
    /// <summary>
    /// ゲームの時間更新、終了判定などライフサイクルに関するUseCase
    /// </summary>
    public sealed class GameLoopUseCase
    {
        private readonly PhaseEntity _phaseEntity;
        private readonly TimeEntity _timeEntity;
        private readonly JudgeEntity _judgeEntity;
        private readonly LifeEntity _lifeEntity;
        private readonly MusicalScoreEntity _musicalScoreEntity;

        public GameLoopUseCase(PhaseEntity phaseEntity, TimeEntity timeEntity, JudgeEntity judgeEntity,
            LifeEntity lifeEntity, MusicalScoreEntity musicalScoreEntity)
        {
            _phaseEntity = phaseEntity;
            _timeEntity = timeEntity;
            _judgeEntity = judgeEntity;
            _lifeEntity = lifeEntity;
            _musicalScoreEntity = musicalScoreEntity;
        }

        public void TryUpdate(float deltaTime)
        {
            if (_phaseEntity.Value != Phase.Game) return;
            _timeEntity.FixUpdate(deltaTime);
            Spawn();
            _musicalScoreEntity.CheckSound(_timeEntity.Time);
        }

        private void Spawn()
        {
            // 自然消滅のダメージ
            if (_judgeEntity.TryDespawn(_timeEntity.Time, out var missNote))
            {
                _lifeEntity.Judgement(missNote);
            }

            if (_timeEntity.IsFinish)
            {
                _phaseEntity.OnNext(Phase.Finish);
                return;
            }

            _musicalScoreEntity.TrySpawn(_timeEntity.Time);
        }
    }
}