using System;
using System.Collections.Generic;
using Game.Battle.Skills;
using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace.Player
{
    [Serializable]
    public class Character
    {
        public string Name;
        public Unit BattleView;
        public List<SkillCommandHandler> Skills;

        [HideInInspector] public int UpPoints;
        [HideInInspector] public int Exp;
        [HideInInspector] public int NextLevelExp => GetNextLevelExp();


        [HideInInspector] public int Level;

        public Attribute Hull = new() { Name = "Hull" };
        public Attribute Strength = new() { Name = "Strength" };
        public Attribute Agile = new() { Name = "Agile" };
        public Attribute Speed = new() { Name = "Speed" };
        public Attribute Balance = new() { Name = "Balance" };

        [HideInInspector] public Stat Health;
        [HideInInspector] public Stat MoveRange;
        [HideInInspector] public Stat ActionPoints;
        [HideInInspector] public Stat Damage;
        [HideInInspector] public Stat AttackRange;
        [HideInInspector] public Stat Initiative;

        private List<Stat> _stats = new List<Stat>();
        private List<Attribute> _attributes = new();

        public Character()
        {
            Health = new(Hull, 10) { Name = "Health" };
            MoveRange = new(Speed, 0.2f) { Name = "Move Range" };
            ActionPoints = new(Balance, 1.25f) { Name = "Action Points" };
            Damage = new(Strength, 0.5f) { Name = "Damage" };
            AttackRange = new(Agile, 0.15f) { Name = "Attack Range" };
            Initiative = new(Balance,Speed, 0.35f,0.25f) { Name = "Initiative" };
        }
        
        private int GetNextLevelExp()
        {
            return Level * 100;
        }
        
        public List<Stat> GetAllStats()
        {
            _stats = new();
            _stats.Add(Health);
            _stats.Add(MoveRange);
            _stats.Add(Damage);
            _stats.Add(AttackRange);
            _stats.Add(Initiative);

            return _stats;
        }

        public List<Attribute> GetAllAttributes()
        {
            //TODO: fix
            _attributes = new();
            _attributes.Add(Hull);
            _attributes.Add(Strength);
            _attributes.Add(Agile);
            _attributes.Add(Speed);
            _attributes.Add(Balance);

            return _attributes;
        }
    }

    [Serializable]
    public class Stat
    {
        public int Value => Calculate();

        [HideInInspector] public string Name;
        [HideInInspector] public string Description;

        private Attribute _refAttribute;
        private Attribute _refAttribute2;
        private float _multiplier;
        private float _multiplier2;

        public Stat(Attribute attribute, float multiplier)
        {
            _refAttribute = attribute;
            _multiplier = multiplier;
        }

        public Stat(Attribute attribute1, Attribute attribute2, float multiplier1, float multiplier2)
        {
            _refAttribute = attribute1;
            _multiplier = multiplier1;
            _refAttribute2 = attribute2;
            _multiplier2 = multiplier2;
        }

        private int Calculate()
        {
            if (_refAttribute == null)
                return 1;
            
            if (_refAttribute2 == null)
                return (int)(_refAttribute.Value * _multiplier);

            return (int)(_refAttribute.Value * _multiplier + _refAttribute2.Value * _multiplier2);
        }
    }

    [Serializable]
    public class Attribute
    {
        public int Value;
        public string Name;
        public string Description;
    }
}