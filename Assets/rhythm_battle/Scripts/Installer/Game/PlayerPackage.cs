using Unity1Week.rhythm_battle.Presenter.Game;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Unity1Week.rhythm_battle.Installer.Game
{
    public sealed class PlayerPackage : LifetimeScope
    {
        [SerializeField] private Animator _playerAnimator;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<PlayerAnimationPresenter>();
            builder.RegisterComponent(_playerAnimator);
        }
    }
}