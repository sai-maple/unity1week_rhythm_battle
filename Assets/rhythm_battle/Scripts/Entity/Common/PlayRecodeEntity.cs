using System.Collections.Generic;

namespace Unity1Week.rhythm_battle.Entity.Game
{
    public sealed class PlayRecodeEntity
    {
        private readonly Dictionary<string, int> _recode = new();

        public int this[string sceneName] => _recode.ContainsKey(sceneName) ? _recode[sceneName] : 0; 

        public void Add(string sceneName, int rank)
        {
            if (!_recode.ContainsKey(sceneName))
            {
                _recode.Add(sceneName, rank);
                return;
            }

            if (_recode[sceneName] > rank) return;
            _recode[sceneName] = rank;
        }
    }
}