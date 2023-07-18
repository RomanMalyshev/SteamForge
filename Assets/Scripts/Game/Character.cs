using System.Collections.Generic;
using Game.Battle.Skills;
using UnityEngine;

namespace DefaultNamespace.Player
{
    public class Character
    {
        public Stat Health = new();
        public Stat MoveRange= new();
        public Stat ActionPoints= new();
        public Stat Damage= new();
        public Stat AttackRange= new();
        public Stat Initiative= new();
        public Stat Armor= new();

        public Attribute Hull = new();
        public Attribute Strength = new();
        public Attribute Luck = new();
        public Attribute Speed = new();
        public Attribute Balance = new();

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

    public struct Stat
    {
        public int Value;
        public string Name;
        public string Description;
    }

    public struct Attribute
    {
        public int Value;
        public string Name;
        public string Description;
    }
}