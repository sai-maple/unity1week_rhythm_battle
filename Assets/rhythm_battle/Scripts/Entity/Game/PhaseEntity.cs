using System;
using UniRx;
using Unity1Week.rhythm_battle.Applications.Enum;

namespace Unity1Week.rhythm_battle.Entity.Game
{
    public sealed class PhaseEntity : IDisposable
    {
        private readonly Subject<Phase> _subject;
        public Phase Value { get; private set; }

        public IObservable<Phase> OnPhaseChangedAsObservable()
        {
            return _subject.Share();
        }

        public PhaseEntity()
        {
            _subject = new Subject<Phase>();
        }

        public void OnNext(Phase phase)
        {
            Value = phase;
            _subject.OnNext(phase);
        }

        public void Dispose()
        {
            _subject?.Dispose();
        }
    }
}