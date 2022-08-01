using Unity.Mathematics;
using UnityEngine;

namespace _Project.Scripts
{
    public class Replicator : MonoBehaviour
    {
        [SerializeField] private bool _isEnabled;
        [SerializeField] private GameObject _pongPrefab;
        [SerializeField] private int _numOfEntities;
        [SerializeField] private float _spacing;
        private Vector2 _spawnPos;

        private void Awake()
        {
            if (!_isEnabled) return;
            
            for (int i = 0; i < _numOfEntities; i++)
            {
                _spawnPos.x += _spacing;
                Instantiate(_pongPrefab, _spawnPos, quaternion.identity);
            }
        }
    }
}
