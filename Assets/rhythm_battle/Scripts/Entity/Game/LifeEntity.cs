using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using Unity1Week.rhythm_battle.Applications.Data;
using Unity1Week.rhythm_battle.Applications.Enum;
using Unity1Week.rhythm_battle.Applications.Static;

namespace Unity1Week.rhythm_battle.Entity.Game
{
    /// <summary>
    /// スライダーで表示する残りHPの管理、とHPがそのままスコアになる
    /// 
    /// </summary>
    public sealed class LifeEntity : IDisposable
    {
        private readonly ReactiveProperty<float> _enemyLife = new(3);

        private float _perfect;

        private float _score;

        public float MusicBonus => _score / (_perfect * 2) * 100;
        public float BattleBonus => (EnemyLifeTotal - _enemyLife.Value) / (EnemyLifeTotal * 2) * 100;
        public float EnemyLifeTotal { get; private set; }

        public IObservable<float> OnEnemyLifeChangedAsObservable()
        {
            return _enemyLife;
        }

        public void Initialize(List<NoteDto> scores)
        {
            _perfect = scores.Count * Judge.Perfect.Score();
            _score = 0;
            EnemyLifeTotal = scores.Count(note => StaticData.IsCounter(note.Type));
            _enemyLife.Value = (int)EnemyLifeTotal;
        }

        public void Judgement(NoteDto note)
        {
            // カウンター判定のノートの時はダメージ
            if (StaticData.IsCounter(note.Type))
            {
                _enemyLife.Value -= note.Judge.Score();
            }

            _score += note.Judge.Score();
        }

        public void Dispose()
        {
            _enemyLife.Dispose();
        }
    }
}