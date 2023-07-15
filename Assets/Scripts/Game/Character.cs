using System.Collections.Generic;
using Game.Battle.Skills;
using UnityEngine;

namespace DefaultNamespace.Player
{
    public class Character
    {
        public Stat Health;
        public Stat MoveRange;
        public Stat ActionPoints;
        public Stat Damage;
        public Stat AttackRange;
        public Stat Initiative;
        public Stat Armor;

        public Attribute Hull;
        public Attribute Strength;
        public Attribute Luck;
        public Attribute Speed;
        public Attribute Balance;

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