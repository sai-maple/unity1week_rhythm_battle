using System;
using UniRx;
using Unity1Week.rhythm_battle.Entity.Game;
using Unity1Week.rhythm_battle.View.Game;
using VContainer.Unity;

namespace Unity1Week.rhythm_battle.Presenter.Game
{
    public sealed class EnemyLifePresenter : IInitializable, IDisposable
    {
        private readonly LifeEntity _lifeEntity;
        private readonly EnemyLifeView _lifeView;

        private readonly CompositeDisposable _disposable = new();

        public EnemyLifePresenter(LifeEntity lifeEntity, EnemyLifeView lifeView)
        {
            _lifeEntity = lifeEntity;
            _lifeView = lifeView;
        }

        public void Initialize()
        {
            _lifeEntity.OnEnemyLifeChangedAsObservable()
                .Subscribe(life => _lifeView.OnChange(life, _lifeEntity.EnemyLifeTotal))
                .AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}