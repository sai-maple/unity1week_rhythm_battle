using System;
using DG.Tweening;
using UniRx;
using Unity1Week.rhythm_battle.Entity.Menu;
using UnityEngine;
using UnityEngine.UI;
using VContainer.Unity;

namespace Unity1Week.rhythm_battle.Presenter.Menu
{
    public sealed class OptionPresenter : IInitializable, IDisposable
    {
        private readonly MenuEntity _menuEntity;
        private readonly Camera _camera;
        private readonly CanvasGroup _canvasGroup;
        private readonly Button _backButton;

        public OptionPresenter(MenuEntity menuEntity, Camera camera, CanvasGroup canvasGroup, Button backButton)
        {
            _menuEntity = menuEntity;
            _camera = camera;
            _canvasGroup = canvasGroup;
            _backButton = backButton;
        }

        private readonly CompositeDisposable _disposable = new();

        public void Initialize()
        {
            _menuEntity.OnChangeAsObservable()
                .Where(item => item == MenuItem.Options)
                .Subscribe(_ =>
                {
                    _canvasGroup.DOFade(1, 1);
                    _canvasGroup.interactable = true;
                    _canvasGroup.blocksRaycasts = true;
                })
                .AddTo(_disposable);

            _backButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _canvasGroup.DOFade(0, 1);
                    _camera.transform.DOMove(new Vector3(0, 0, -10), 1);
                    _canvasGroup.interactable = false;
                    _canvasGroup.blocksRaycasts = false;
                    _menuEntity.OnNext(MenuItem.MusicSelect);
                }).AddTo(_disposable);

            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}