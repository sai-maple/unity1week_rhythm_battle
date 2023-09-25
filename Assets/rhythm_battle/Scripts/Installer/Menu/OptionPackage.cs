using Unity1Week.rhythm_battle.Presenter.Menu;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace Unity1Week.rhythm_battle.Installer.Menu
{
    public sealed class OptionPackage : LifetimeScope
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private CanvasGroup _optionCanvas;
        [SerializeField] private Button _backButton;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<OptionPresenter>();
            builder.RegisterComponent(_camera);
            builder.RegisterComponent(_optionCanvas);
            builder.RegisterComponent(_backButton);
        }
    }
}