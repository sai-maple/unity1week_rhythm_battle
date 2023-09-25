using System;
using UniRx;
using Unity1Week.rhythm_battle.Applications.Enum;
using Unity1Week.rhythm_battle.Entity.Game;
using Unity1Week.rhythm_battle.View.Game;
using VContainer.Unity;

namespace Unity1Week.rhythm_battle.Presenter.Game
{
    public sealed class NoteDespawnPresenter : IInitializable, IDisposable
    {
        private readonly PhaseEntity _phaseEntity;
        private readonly JudgeEntity _judgeEntity;
        private readonly NoteView _noteView;
        private readonly CompositeDisposable _disposable = new();

        public NoteDespawnPresenter(PhaseEntity phaseEntity, JudgeEntity judgeEntity, NoteView noteView)
        {
            _phaseEntity = phaseEntity;
            _judgeEntity = judgeEntity;
            _noteView = noteView;
        }

        public void Initialize()
        {
            _phaseEntity.OnPhaseChangedAsObservable()
                .Subscribe(phase => { _noteView.Pause(phase == Phase.Pause); })
                .AddTo(_disposable);

            _judgeEntity.OnJudgeAsObservable()
                .Subscribe(note =>
                {
                    // if (!_noteView.gameObject.activeSelf) return;
                    _noteView.ReactionAsync(note).Forget();
                }).AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}