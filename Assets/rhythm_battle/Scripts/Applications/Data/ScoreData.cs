using System;
using System.Collections.Generic;
using Unity1Week.rhythm_battle.Applications.Enum;
using UnityEngine;

namespace Unity1Week.rhythm_battle.Applications.Data
{
    [Serializable]
    public sealed class ScoreDto
    {
        [SerializeField] private List<NoteDto> scores;
        [SerializeField] private List<SoundDto> sounds;
        [SerializeField] private int bpm;

        public List<NoteDto> Scores => scores;
        public List<SoundDto> Sounds => sounds;
        public int Bpm => bpm;

        public ScoreDto(List<NoteDto> scores, List<SoundDto> sounds, int bpm)
        {
            this.scores = scores;
            this.sounds = sounds;
            this.bpm = bpm;
        }
    }

    [Serializable]
    public sealed class NoteDto
    {
        [SerializeField] private int id;
        [SerializeField] private int bpm;
        [SerializeField] private int type;
        [SerializeField] private float time;
        [SerializeField] private float hold;

        public int Id => id;
        public int Bpm => bpm;
        public int Type => type;
        public float Time => time;
        public float Hold => hold;

        public bool IsSingle => hold == 0;
        public Judge Judge { get; set; }

        public NoteDto(int id, int bpm, int type, float time, float hold)
        {
            this.id = id;
            this.bpm = bpm;
            this.type = type;
            this.time = time;
            this.hold = hold;
        }
    }

    [Serializable]
    public sealed class SoundDto
    {
        [SerializeField] private int type;
        [SerializeField] private float time;

        public int Type => type;
        public float Time => time;

        public SoundDto(int type, float time)
        {
            this.type = type;
            this.time = time;
        }
    }
}