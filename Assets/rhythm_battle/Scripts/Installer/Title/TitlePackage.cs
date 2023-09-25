using TMPro;
using Unity1Week.rhythm_battle.Presenter.Title;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace Unity1Week.rhythm_battle.Installer.Title
{
    public sealed class TitlePackage : LifetimeScope
    {
        [SerializeField] private Image _mask;
        [SerializeField] private Transform _player;
        [SerializeField] private Button _button;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<TitlePresenter>();
            builder.RegisterComponent(_mask);
            builder.RegisterComponent(_player);
            builder.RegisterComponent(_button);
        }
    }
}