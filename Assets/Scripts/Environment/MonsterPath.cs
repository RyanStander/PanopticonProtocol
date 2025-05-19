using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Environment
{
    public class MonsterPath : MonoBehaviour
    {
        [SerializeField] private List<Transform> pathTargets;
        
        private void OnValidate()
        {
            // Ensure the PathTargets list is populated with all child transforms
            pathTargets.Clear();
            foreach (Transform child in transform)
            {
                pathTargets.Add(child);
            }
        }
        
        public List<Transform> GetPathTargets()
        {
            return pathTargets.ToList();
        }
    }
}
