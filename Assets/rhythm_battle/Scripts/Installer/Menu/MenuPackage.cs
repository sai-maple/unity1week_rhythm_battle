using Unity1Week.rhythm_battle.Presenter.Menu;
using Unity1Week.rhythm_battle.View.Menu;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace Unity1Week.rhythm_battle.Installer.Menu
{
    public sealed class MenuPackage : LifetimeScope
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private CanvasGroup _menuCanvas;
        [SerializeField] private SelectButton[] _selectButtons;
        [SerializeField] private Button _optionButton;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<MenuPresenter>();
            builder.RegisterComponent(_camera);
            builder.RegisterComponent(_menuCanvas);
            builder.RegisterComponent(_selectButtons);
            builder.RegisterComponent(_optionButton);
        }
    }
}