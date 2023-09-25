using System;
using UniRx;
using Unity1Week.rhythm_battle.Applications.Data;
using Unity1Week.rhythm_battle.Applications.Enum;
using Unity1Week.rhythm_battle.Applications.Static;
using Unity1Week.rhythm_battle.Entity.Game;
using UnityEngine;
using VContainer.Unity;

namespace Unity1Week.rhythm_battle.Presenter.Game
{
    public sealed class EnemyPresenter : IInitializable, IDisposable
    {
        private readonly JudgeEntity _judgeEntity;
        private readonly MusicalScoreEntity _musicalScoreEntity;
        private readonly LifeEntity _lifeEntity;
        private readonly Animator _animator;

        private readonly CompositeDisposable _disposable = new();

        public EnemyPresenter(JudgeEntity judgeEntity, MusicalScoreEntity musicalScoreEntity, LifeEntity lifeEntity,
            Animator animator)
        {
            _judgeEntity = judgeEntity;
            _musicalScoreEntity = musicalScoreEntity;
            _lifeEntity = lifeEntity;
            _animator = animator;
        }

        public void Initialize()
        {
            _musicalScoreEntity.OnSpawnAsObservable().Subscribe(OnSpawnAnimation).AddTo(_disposable);
            _judgeEntity.OnJudgeAsObservable().Subscribe(OnCounter).AddTo(_disposable);
            _animator.speed = StaticData.SpeedLate(_musicalScoreEntity.Bpm);

            _lifeEntity.OnEnemyLifeChangedAsObservable()
                .Where(life => life == 0)
                .Subscribe(_ => _animator.SetTrigger(AnimatorParameter.Down))
                .AddTo(_disposable);
        }

        private void OnSpawnAnimation(NoteDto note)
        {
            _animator.SetTrigger(AnimatorParameter.Attack);
        }

        private void OnCounter(NoteDto note)
        {
            if (!StaticData.IsCounter(note.Type) || note.Judge == Judge.Miss) return;
            _animator.SetTrigger(AnimatorParameter.Damage);
        }

        private static class AnimatorParameter
        {
            public static readonly int Attack = Animator.StringToHash("Attack");
            public static readonly int Damage = Animator.StringToHash("Damage");
            public static readonly int Down = Animator.StringToHash("Down");
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}