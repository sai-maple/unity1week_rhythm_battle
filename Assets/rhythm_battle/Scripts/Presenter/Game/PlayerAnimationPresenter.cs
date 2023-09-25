using System;
using UniRx;
using Unity1Week.rhythm_battle.Applications.Data;
using Unity1Week.rhythm_battle.Applications.Enum;
using Unity1Week.rhythm_battle.Applications.Static;
using Unity1Week.rhythm_battle.Entity.Game;
using UnityEngine;
using VContainer.Unity;
using Random = UnityEngine.Random;

namespace Unity1Week.rhythm_battle.Presenter.Game
{
    public sealed class PlayerAnimationPresenter : IInitializable, IDisposable
    {
        private readonly JudgeEntity _judgeEntity;
        private readonly MusicalScoreEntity _musicalScoreEntity;
        private readonly PhaseEntity _phaseEntity;
        private readonly Animator _animator;

        private readonly CompositeDisposable _disposable = new();

        public PlayerAnimationPresenter(JudgeEntity judgeEntity, MusicalScoreEntity musicalScoreEntity,
            PhaseEntity phaseEntity, Animator animator)
        {
            _judgeEntity = judgeEntity;
            _musicalScoreEntity = musicalScoreEntity;
            _phaseEntity = phaseEntity;
            _animator = animator;
        }

        public void Initialize()
        {
            _judgeEntity.OnJudgeAsObservable()
                .Subscribe(OnAnimation).AddTo(_disposable);
            _phaseEntity.OnPhaseChangedAsObservable()
                .Where(phase => phase == Phase.Game)
                .Subscribe(_ => _animator.speed = StaticData.SpeedLate(_musicalScoreEntity.Bpm))
                .AddTo(_disposable);
        }

        private void OnAnimation(NoteDto note)
        {
            var trigger = note.Judge switch
            {
                Judge.Perfect => AnimatorParameter.Attack,
                Judge.Good => AnimatorParameter.Attack,
                Judge.Normal => AnimatorParameter.Attack,
                Judge.Miss => AnimatorParameter.Miss,
                Judge.Non => throw new ArgumentOutOfRangeException(),
                _ => throw new ArgumentOutOfRangeException()
            };

            // 反撃可能な攻撃の時は固有アニメーションに差し替える
            trigger = StaticData.IsCounter(note.Type) && note.Judge != Judge.Miss
                ? AnimatorParameter.Counter
                : trigger;
            _animator.SetInteger(AnimatorParameter.Random, Random.Range(0, 4));
            _animator.SetTrigger(trigger);
        }

        private static class AnimatorParameter
        {
            public static readonly int Attack = Animator.StringToHash("Attack");
            public static readonly int Miss = Animator.StringToHash("Miss");
            public static readonly int Counter = Animator.StringToHash("Counter");
            public static readonly int Lose = Animator.StringToHash("Lose");
            public static readonly int Random = Animator.StringToHash("Random");
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}