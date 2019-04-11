using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
		[SerializeField] float weaponDamage = 5f;
		[SerializeField] float timeBetweenAttacks = 1f;

        Mover mover;
        Health target;

        float timeSinceLastAttack = Mathf.Infinity;

        private void Start()
        {
            mover = GetComponent<Mover>();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if ( target == null ) return;
			if ( target.IsDead() ) return;

            if (!GetIsInRange() )
            {
                mover.MoveTo( target.transform.position, 1f );
            }
            else
            {
                mover.Cancel();
                AttackBehavior();
            }
        }

        private void AttackBehavior()
        {
			transform.LookAt( target.transform );
			if ( timeSinceLastAttack >= timeBetweenAttacks )
			{
				// This will trigger the Hit() event
				TriggerAttack();
				timeSinceLastAttack = 0;
			}
		}

		private void TriggerAttack()
		{
			GetComponent<Animator>().ResetTrigger( "stopAttack" );
			GetComponent<Animator>().SetTrigger( "attack" );
		}

		// Animation Event
		private void Hit()
		{
			if ( target == null ) return;
			target.TakeDamage( weaponDamage );
		}

		private bool GetIsInRange()
        {
            return Vector3.Distance( target.transform.position, transform.position ) < weaponRange;
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

		// Start public methods
		public bool CanAttack( GameObject combatTarget )
		{
			if ( combatTarget == null ) return false;

			Health targetToTest = combatTarget.GetComponent<Health>();
			return targetToTest != null && !targetToTest.IsDead();
		}

        public void Attack( GameObject combatTarget )
        {
            GetComponent<ActionScheduler>().StartAction( this );
            target = combatTarget.GetComponent<Health>();
        }

		public void Cancel()
		{
			StopAttack();
			target = null;
			mover.Cancel();
		}
	}
}