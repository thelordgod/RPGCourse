using Control;
using Core;
using UnityEngine;
using UnityEngine.Playables;

namespace Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        private GameObject _player;
        
        private void Start()
        {
            _player = GameObject.FindWithTag("Player");
            var director = GetComponent<PlayableDirector>();
            director.played += DisableControl;
            director.stopped += EnableControl;
        }

        private void DisableControl(PlayableDirector director)
        {
            _player.GetComponent<ActionScheduler>().CancelCurrentAction();
            _player.GetComponent<PlayerController>().enabled = false;
        }

        private void EnableControl(PlayableDirector director)
        {
            _player.GetComponent<PlayerController>().enabled = true;
        }
    }
}