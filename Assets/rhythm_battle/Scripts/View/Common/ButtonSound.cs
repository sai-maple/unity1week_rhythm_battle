using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace Unity1Week.rhythm_battle.View.Common
{
    [RequireComponent(typeof(Button), typeof(AudioSource))]
    public sealed class ButtonSound : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private Button _button;

        [SerializeField] private AudioClip _select;
        [SerializeField] private AudioClip _pressed;

        private void Awake()
        {
            _button.OnPointerEnterAsObservable().TakeUntilDestroy(this).Where(_ => _select != null)
                .Subscribe(_ => _audioSource.PlayOneShot(_select));

            _button.OnClickAsObservable().TakeUntilDestroy(this).Where(_ => _pressed != null)
                .Subscribe(_ => _audioSource.PlayOneShot(_pressed));
        }

        private void Reset()
        {
            _audioSource = GetComponent<AudioSource>();
            _button = GetComponent<Button>();
            _audioSource.playOnAwake = false;
        }
    }
}