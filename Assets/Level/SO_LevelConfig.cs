using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    [CreateAssetMenu(fileName = "SO_LevelConfig", menuName = "Scriptable Objects/SO_LevelConfig")]
    public class SO_LevelConfig : ScriptableObject
    {
        public List<int> spawnTime;
        public List<GameObject> customerList;
        public int tax;
    }
}