using System;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Unity1Week.rhythm_battle.View.Menu
{
    public sealed class SelectButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        [SerializeField] private BoxCollider2D _collider;
        [SerializeField] private SpriteRenderer _rank;
        [SerializeField] private SpriteRenderer _selectIcon;
        [SerializeField] private Sprite[] _rankIcons;
        [SerializeField] private Animator _buttonAnimator;
        [SerializeField] private Animator _monsterAnimator;
        [SerializeField] private Vector3 _center;
        [SerializeField] private string _sceneName;

        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _enterClip;
        [SerializeField] private AudioClip _pressedClip;

        public string SceneName => _sceneName;

        private readonly Subject<string> _subject = new();
        private static readonly int IsWalk = Animator.StringToHash("isWalk");
        private static readonly int Select = Animator.StringToHash("Select");

        public void Initialize(int rank)
        {
            _rank.DOFade(rank == 0 ? 0 : 1, 0);
            _rank.sprite = _rankIcons[rank];
            _selectIcon.enabled = false;
        }

        public IObservable<string> OnSelectAsObservable()
        {
            return _subject.Share();
        }

        public void OnSelect(string sceneName)
        {
            _collider.enabled = false;
            _rank.DOFade(0, 0.5f);
            if (sceneName == _sceneName) return;
            transform.DOMove(new Vector3(10, 0, -10), 0.5f);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _audioSource.PlayOneShot(_enterClip);
            _buttonAnimator.SetBool(Select, true);
            _monsterAnimator.SetBool(IsWalk, true);
            _selectIcon.enabled = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _buttonAnimator.SetBool(Select, false);
            _monsterAnimator.SetBool(IsWalk, false);
            _selectIcon.enabled = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _subject.OnNext(_sceneName);
            _audioSource.PlayOneShot(_pressedClip);
            transform.DOMove(_center, 1f);
            _selectIcon.enabled = false;
        }
    }
}