using UnityEngine;

namespace Core
{
    public class DestroyAfterEffect : MonoBehaviour
    {
        private ParticleSystem _particleSystem;
        
        private void Start()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }
        
        void Update()
        {
            if (!_particleSystem.IsAlive())
            {
                Destroy(gameObject);
            }
        
        }
    }
}
