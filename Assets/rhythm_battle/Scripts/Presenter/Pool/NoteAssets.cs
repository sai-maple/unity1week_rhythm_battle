using System.Collections.Generic;
using Unity1Week.rhythm_battle.View;
using Unity1Week.rhythm_battle.View.Game;
using UnityEngine;

namespace Unity1Week.rhythm_battle.Presenter.Pool
{
    [CreateAssetMenu(menuName = "Assets/NoteAssets", fileName = "NoteAssets")]
    public sealed class NoteAssets : ScriptableObject
    {
        public List<NoteView> NoteViews;
    }
}