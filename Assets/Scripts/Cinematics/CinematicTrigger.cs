using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        bool alreadtTriggered = false;

        private void OnTriggerEnter(Collider other) {
            if ( !alreadtTriggered && other.gameObject.tag == "Player" )
            {
                alreadtTriggered = true;
                GetComponent<PlayableDirector>().Play();
            }            
        }
    }
}