using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity1Week.rhythm_battle.View.Loading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VContainer.Unity;

#pragma warning disable CS4014
namespace Unity1Week.rhythm_battle.Presenter.Title
{
    public sealed class TitlePresenter : IInitializable
    {
        private readonly Image _mask;
        private readonly Transform _player;
        private readonly Button _button;
        private readonly LoadingView _loadingView;

        public TitlePresenter(Image mask,Transform player, Button button,
            LoadingView loadingView)
        {
            _mask = mask;
            _player = player;
            _button = button;
            _loadingView = loadingView;
        }

        public async void Initialize()
        {
            _mask.gameObject.SetActive(true);
            await _mask.DOFade(0, 0.5f);
            _mask.gameObject.SetActive(false);

            _button.onClick.AddListener(() => StartAsync().Forget());
        }

        private async UniTaskVoid StartAsync()
        {
            _mask.gameObject.SetActive(true);
            _player.DOLocalMoveX(10, 2);
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            await _loadingView.FadeInAsync();
            await SceneManager.UnloadSceneAsync("Title");
            // todo tutorial
            SceneManager.LoadScene("Menu", LoadSceneMode.Additive);
        }
    }
}