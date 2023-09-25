using Unity1Week.rhythm_battle.Presenter.Game;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace Unity1Week.rhythm_battle.Installer.Game
{
    public sealed class GameUiPackage : LifetimeScope
    {
        [SerializeField] private CanvasGroup _uiCanvas;
        [SerializeField] private Button _pauseButton;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GameUiPresenter>();
            builder.RegisterComponent(_uiCanvas);
            builder.RegisterComponent(_pauseButton);
        }
    }
}