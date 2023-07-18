using System.Collections.Generic;
using Game.Battle.Skills;
using UnityEngine;

namespace DefaultNamespace.Player
{
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
        public Stat Armor= new()
        {
            Name = "Armor"
        };


        public Attribute Hull = new()
        {
            Name = "Hull"
        };
        public Attribute Strength = new()
        {
            Name = "Strength"
        };
        public Attribute Luck = new()
        {
            Name = "Luck"
        };
        public Attribute Speed = new()
        {
            Name = "Speed"
        };
        public Attribute Balance = new()
        {
            Name = "Balance"
        };

        public string Name;

        public int UpPoints;

        public int Level;

        public List<Equipment> Equipments;
        public List<SkillCommandHandler> Skills;
        public GameObject Figure;

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
            _stats.Add(Armor);

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

    public class Stat
    {
        public int Value;
        public string Name;
        public string Description;
    }

    public class Attribute
    {
        public int Value;
        public string Name;
        public string Description;
    }
}