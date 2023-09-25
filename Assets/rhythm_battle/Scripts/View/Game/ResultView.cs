using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

#pragma warning disable CS4014
namespace Unity1Week.rhythm_battle.View.Game
{
    public sealed class ResultView : MonoBehaviour
    {
        [SerializeField] private PlayableDirector _common;
        [SerializeField] private PlayableDirector[] _directors;
        [SerializeField] private Camera _camera;
        [SerializeField] private CanvasGroup _uiCanvas;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TextMeshProUGUI _defenceScore;
        [SerializeField] private TextMeshProUGUI _attackScore;
        [SerializeField] private TextMeshProUGUI _totalScore;
        [SerializeField] private TextMeshProUGUI _comment;

        [SerializeField] private CanvasGroup _buttonCanvas;
        [SerializeField] private Button _retryButton;
        [SerializeField] private Button _returnButton;

        public IObservable<Unit> OnRetryAsObservable()
        {
            return _retryButton.OnClickAsObservable().ThrottleFirst(TimeSpan.FromSeconds(1));
        }

        public IObservable<Unit> OnReturnAsObservable()
        {
            return _returnButton.OnClickAsObservable().ThrottleFirst(TimeSpan.FromSeconds(1));
        }

        private void Awake()
        {
            _canvasGroup.alpha = 0;
            gameObject.SetActive(false);
        }

        public void Test()
        {
            ResultAsync(50,50);
        }

        public async void ResultAsync(float musicScore, float battleScore)
        {
            _defenceScore.text = "";
            _attackScore.text = "";
            _totalScore.text = "";
            _buttonCanvas.alpha = 0;
            _buttonCanvas.interactable = false;
            _uiCanvas.interactable = false;
            _uiCanvas.blocksRaycasts = false;
            gameObject.SetActive(true);
            var grade = (musicScore + battleScore) switch
            {
                < 40f => 0,
                < 60f => 1,
                < 80f => 2,
                < 100f => 3,
                _ => 4,
            };
            _comment.text = Comment(grade, musicScore, battleScore);

            _common.Play();
            _uiCanvas.DOFade(0, 1);
            _camera.transform.DOMove(new Vector3(-4.5f, -1f, -10), 1f);
            _camera.DOOrthoSize(2.5f, 1);
            await _canvasGroup.DOFade(1, 1);

            // タイトル表示　ラベル表示
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

            await _defenceScore.DOCounter(0, (int)musicScore, 0.5f);
            _defenceScore.text = $"{musicScore:F1} 点";

            await _attackScore.DOCounter(0, (int)battleScore, 0.5f);
            _attackScore.text = $"{battleScore:F1} 点";

            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            _totalScore.text = $"{musicScore + battleScore:F1} 点";


            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            _directors[grade].Play();

            _buttonCanvas.DOFade(1, 0.5f);
            _buttonCanvas.interactable = true;
        }

        public void Disable()
        {
            _buttonCanvas.interactable = false;
            _buttonCanvas.blocksRaycasts = false;
        }

        private string Comment(int grade, float musicScore, float battleScore)
        {
            return grade switch
            {
                0 => musicScore > battleScore
                    ? "戦いに怯えていおるようでは話にならんの。\nしっかり剣を振るのじゃ。"
                    : "手元がおろそかになっておらんかの。\n猪突猛進。おぬしにぴったりの言葉じゃ",
                1 => musicScore > battleScore ? "すこしは様になってきたかの。\n戦いは攻撃も重要じゃぞ。" : "攻撃は一人前。しかし防御がのう。",
                2 => musicScore > battleScore ? "安定して戦えるようになってきたの。" : "攻撃に鋭さが増してきたの。\n一人前までもう少しじゃ。",
                3 => musicScore > battleScore ? "あれだけの戦いでほとんどキズを追わないとは流石じゃ！" : "見事！\nおぬしならどんな敵でも打ち倒せるじゃろう。",
                _ => "これぞ武の境地。\nおぬしが伝説の勇者じゃ！",
            };
        }
    }
}