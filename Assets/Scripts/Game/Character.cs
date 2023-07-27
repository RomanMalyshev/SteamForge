using System;
using System.Collections.Generic;
using Game.Battle.Skills;
using UnityEngine;

namespace DefaultNamespace.Player
{
    [Serializable]
    public class Character
    {
        public Stat Health = new()
        {
            Name = "Health"
        };
        public Stat MoveRange= new()
        {
            Name = "MoveRange"
        };

        public Stat ActionPoints= new()
        {
            Name = "ActionPoints"
        };
        public Stat Damage= new()
        {
            Name = "Damage"
        };
        public Stat AttackRange= new()
        {
            Name = "AttackRange"
        };
        public Stat Initiative= new()
        {
            Name = "Initiative"
        };
     

        [HideInInspector]
        public Attribute Hull = new()
        {
            Name = "Hull"
        };
        [HideInInspector]
        public Attribute Strength = new()
        {
            Name = "Strength"
        };
        [HideInInspector]
        public Attribute Luck = new()
        {
            Name = "Luck"
        };
        [HideInInspector]
        public Attribute Speed = new()
        {
            Name = "Speed"
        };
        [HideInInspector]
        public Attribute Balance = new()
        {
            Name = "Balance"
        };
        
        public string Name;
        [HideInInspector]
        public int UpPoints;
        [HideInInspector]
        public int Exp;
        [HideInInspector]
        public int Level;

        [HideInInspector]
        public List<Equipment> Equipments;
        public List<SkillCommandHandler> Skills;
        public Unit BattleView;

        private List<Stat> _stats = new List<Stat>();
        private List<Attribute> _attributes = new ();

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
            _attributes.Add(Luck);
            _attributes.Add(Speed);
            _attributes.Add(Balance);

            return _attributes;
        }
    }

    [Serializable]
    public class Stat
    {
        public int Value;
        
        [HideInInspector]
        public string Name;
        
        [HideInInspector]
        public string Description;
    }

    [Serializable]
    public class Attribute
    {
        public int Value;
        public string Name;
        public string Description;
    }
}