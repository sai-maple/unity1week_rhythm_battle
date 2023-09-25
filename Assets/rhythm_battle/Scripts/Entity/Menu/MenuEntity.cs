using System;
using UniRx;

namespace Unity1Week.rhythm_battle.Entity.Menu
{
    public sealed class MenuEntity : IDisposable
    {
        private readonly Subject<MenuItem> _subject = new();

        public IObservable<MenuItem> OnChangeAsObservable()
        {
            return _subject.Share();
        }

        public void OnNext(MenuItem item)
        {
            _subject.OnNext(item);
        }
        
        public void Dispose()
        {
            _subject?.OnCompleted();
            _subject?.Dispose();
        }
    }

    public enum MenuItem
    {
        MusicSelect,
        Options,
    }
}