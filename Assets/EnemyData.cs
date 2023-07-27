using DefaultNamespace.Player;
using RedBjorn.ProtoTiles;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(menuName = "Data/Enemy")]
    public class EnemyData:ScriptableObject
    {
        public Character Character;
        public TileTag TagOnField;
    }
}