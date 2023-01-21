using UnityEngine;
using UnityEngine.Playables;

namespace Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        private bool _hasTriggered;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!_hasTriggered && other.CompareTag("Player"))
            {
                GetComponent<PlayableDirector>().Play();
                _hasTriggered = true;
            }
        }
    }
}