using UnityEngine;

namespace Monsters
{
    public class MonsterObjective : MonoBehaviour
    {
        [SerializeField] private MonsterManager monsterManager;
        [SerializeField] private bool idleDuringObjective = true;
        [SerializeField] private float randomIdleTime = 5f;
        
    }
}
