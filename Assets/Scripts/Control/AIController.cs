using System;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
	public class AIController : MonoBehaviour
	{
		[SerializeField] float chaseDistance = 5f;
		[SerializeField] float suspicionTime = 5f;
		[SerializeField] float waypointTolerance = 1f;
		[SerializeField] float waypointDwellTime = 2f;
		[Range(0, 1)][SerializeField] float patrolSpeedFraction = 0.2f;
		[SerializeField] PatrolPath patrolPath;

		Fighter fighter;
		GameObject player;
		Health health;
		Mover mover;

		Vector3 guardPosition;
		float timeSinceLastSawPlayer = Mathf.Infinity;
		float timeSinceArrivedAtWaypoint = Mathf.Infinity;
		int currentWaypointIndex = 0;

		void Start()
		{
			fighter = GetComponent<Fighter>();
			health = GetComponent<Health>();
			mover = GetComponent<Mover>();
			player = GameObject.FindWithTag( "Player" );

			guardPosition = transform.position;
		}

		void Update()
		{
			if ( health.IsDead() ) return;

			if ( InAttackRangeOfPlayer() && fighter.CanAttack( player ) )
			{
				AttackBehavior();
			}
			else if ( timeSinceLastSawPlayer < suspicionTime )
			{
				SuspicionBehavior();
			}
			else
			{
				PatrolBehavior();
			}

			UpdateTimers();
		}

		private void UpdateTimers()
		{
			timeSinceLastSawPlayer += Time.deltaTime;
			timeSinceArrivedAtWaypoint += Time.deltaTime;
		}

		private void PatrolBehavior()
		{
			Vector3 nextPosition = guardPosition;

			if ( patrolPath != null )
			{
				if ( AtWaypoint() )
				{
					timeSinceArrivedAtWaypoint = 0;
					CycleWaypoint();
				}
				nextPosition = GetCurrentWaypoint();
			}

			if ( timeSinceArrivedAtWaypoint > waypointDwellTime )
			{
				mover.StartMoveAction( nextPosition, patrolSpeedFraction );
			}
		}

		private Vector3 GetCurrentWaypoint()
		{
			return patrolPath.GetWaypoint( currentWaypointIndex );
		}

		private void CycleWaypoint()
		{
			currentWaypointIndex = patrolPath.GetNextIndex( currentWaypointIndex );
		}

		private bool AtWaypoint()
		{
			float distanceToWaypoint = Vector3.Distance( transform.position, GetCurrentWaypoint() );
			return distanceToWaypoint < waypointTolerance;
		}

		private void SuspicionBehavior()
		{
			GetComponent<ActionScheduler>().CancelCurrentAction();
		}

		private void AttackBehavior()
		{
			timeSinceLastSawPlayer = 0;
			fighter.Attack( player );
		}

		private bool InAttackRangeOfPlayer()
		{
			float distanceToPlayer = Vector3.Distance( player.transform.position, transform.position );
			return distanceToPlayer < chaseDistance;
		}

		// Called by unity
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere( transform.position, chaseDistance );
		}
	}
}