using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using Unity1Week.rhythm_battle.Entity.Game;
using Unity1Week.rhythm_battle.Entity.Menu;
using Unity1Week.rhythm_battle.View.Loading;
using Unity1Week.rhythm_battle.View.Menu;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VContainer.Unity;

#pragma warning disable CS4014
namespace Unity1Week.rhythm_battle.Presenter.Menu
{
    public sealed class MenuPresenter : IInitializable, IDisposable
    {
        private readonly MenuEntity _menuEntity;
        private readonly Camera _camera;
        private readonly CanvasGroup _menuCanvas;
        private readonly PlayRecodeEntity _playRecodeEntity;
        private readonly SelectButton[] _selectButtons;
        private readonly Button _optionButton;
        private readonly LoadingView _loadingView;

        private readonly CompositeDisposable _disposable = new();

        public MenuPresenter(MenuEntity menuEntity, Camera camera, CanvasGroup menuCanvas,
            PlayRecodeEntity playRecodeEntity, SelectButton[] selectButtons, Button optionButton,
            LoadingView loadingView)
        {
            _menuEntity = menuEntity;
            _camera = camera;
            _menuCanvas = menuCanvas;
            _playRecodeEntity = playRecodeEntity;
            _selectButtons = selectButtons;
            _optionButton = optionButton;
            _loadingView = loadingView;
        }

        public async void Initialize()
        {
            foreach (var selectButton in _selectButtons)
            {
                selectButton.Initialize(_playRecodeEntity[selectButton.SceneName]);
            }

            _selectButtons.Select(button => button.OnSelectAsObservable())
                .Merge()
                .Subscribe(sceneName => { SelectAsync(sceneName).Forget(); }).AddTo(_disposable);

            _optionButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _menuCanvas.DOFade(0, 1);
                    _camera.transform.DOMove(new Vector3(-10, 0, -10), 1);
                    _menuCanvas.interactable = false;
                    _menuCanvas.blocksRaycasts = false;
                    _menuEntity.OnNext(MenuItem.Options);
                }).AddTo(_disposable);

            _menuEntity.OnChangeAsObservable()
                .Where(item => item == MenuItem.MusicSelect)
                .Subscribe(_ =>
                {
                    _menuCanvas.DOFade(1, 1);
                    _menuCanvas.interactable = true;
                    _menuCanvas.blocksRaycasts = true;
                })
                .AddTo(_disposable);

            await UniTask.Delay(TimeSpan.FromSeconds(1));
            await _loadingView.FadeOutAsync();
        }

        private async UniTaskVoid SelectAsync(string sceneName)
        {
            _menuCanvas.DOFade(0, 0.5f);
            _menuCanvas.interactable = false;
            _menuCanvas.blocksRaycasts = false;
            SceneEntity.SceneName = sceneName;
            foreach (var selectButton in _selectButtons)
            {
                selectButton.OnSelect(sceneName);
                _camera.transform.DOMove(new Vector3(-3.5f, -0.5f, -10), 1);
                _camera.DOOrthoSize(2.5f, 1);
            }
            await UniTask.Delay(TimeSpan.FromSeconds(2));
            await _loadingView.FadeInAsync();
            await SceneManager.UnloadSceneAsync("Menu");
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}