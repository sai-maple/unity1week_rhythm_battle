#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity1Week.rhythm_battle.Applications.Data;
using UnityEditor;
using UnityEngine;

namespace Unity1Week.rhythm_battle.Editor
{
    public sealed class ScoreConvertEditor : EditorWindow
    {
        [SerializeField] private TextAsset _score;

        [MenuItem("Tools/ScoreConvert")]
        private static void ShowWindow()
        {
            GetWindow<ScoreConvertEditor>("ViewTestWindow");
        }

        private void OnGUI()
        {
            GUILayout.Label("");
            GUILayout.Label("------------------------------");
            GUILayout.Label("NoteEditorのjsonをこのゲームのフォーマットに変換します");

            var so = new SerializedObject(this);
            so.Update();
            EditorGUILayout.PropertyField(so.FindProperty("_score"), true);
            so.ApplyModifiedProperties();

            if (!GUILayout.Button("変換")) return;
            var notes = JsonUtility.FromJson<Notes>(_score.text);
            var scoreDto = Convert(notes);

            var filePath = $"Assets/StoreAssets/Score/{_score.name}.json";
            var outJson = JsonUtility.ToJson(scoreDto);
            File.WriteAllText(filePath, outJson);
        }

        private static ScoreDto Convert(Notes notes)
        {
            var bpm = notes.BPM;
            var beetTime = 60f / notes.BPM;

            var score = notes.notes
                .Where(n => !(n.type == 2 && n.notes.Count == 0))
                .Select((note, i) =>
                {
                    var numTime = beetTime / note.LPB;
                    var time = note.num * numTime;
                    var hold = note.notes.Count == 0 ? 0 : (note.notes[0].num - note.num) % 16 * numTime;
                    var type = note.block;
                    return new NoteDto(i, bpm, type, time, hold);
                })
                .ToList();

            var sounds = notes.notes
                .Where(n => n.type == 2)
                .Where(n => n.notes.Count == 0)
                .Select(note =>
                {
                    var numTime = beetTime / note.LPB;
                    var time = note.num * numTime;
                    var type = note.block;
                    return new SoundDto(type, time);
                })
                .ToList();
            return new ScoreDto(score, sounds, notes.BPM);
        }

        [Serializable]
        private sealed class Notes
        {
            public string name;
            public int maxBlock;
            public int BPM;
            public int offset;
            public List<Note> notes = new List<Note>();
        }

        [Serializable]
        private sealed class Note
        {
            public int LPB;
            public int num;
            public int block;
            public int type;
            public List<Note> notes = new List<Note>();
        }
    }
}
#endif