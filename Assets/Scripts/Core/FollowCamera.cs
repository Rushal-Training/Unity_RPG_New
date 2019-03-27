using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] Transform target;

        void Start()
        {

        }

        void LateUpdate()
        {
            transform.position = target.position;
        }
    }
}