using System;
using System.Collections.Generic;
using UnityEngine;
namespace Core
{
	public class State<T> : MonoBehaviour
	{
		// Object to manage
		private T targetObject;

		// Tracking last state
		protected string lastState;

		// Used after transition
		protected string transitionData;

		// Map of states that we can transition from
		private Dictionary<string, bool> allowedTransitions = new Dictionary<string, bool>();

		/// <summary>
		/// Init the specified targetObject, typeOfLastState and transitionData.
		/// </summary>
		/// <param name="targetObject">Target object.</param>
		/// <param name="typeOfLastState">Type of last state.</param>
		/// <param name="transitionData">Transition data.</param>
		public void Init() 
		{
			AddTransitions();
		}

		/// <summary>
		/// Handles the enter event.
		/// </summary>
		public virtual void OnEnter(T targetObject, string lastState, string transitionData)
		{
			this.targetObject = targetObject;
			this.lastState = lastState;
			this.transitionData = transitionData;
		}

		/// <summary>
		/// Handles the exit event.
		/// </summary>
		public virtual void OnExit(){}

		/// <summary>
		/// Returns true if this state can transition to one of the same type
		/// </summary>
		/// <value><c>true</c> if allow transition to same state; otherwise, <c>false</c>.</value>
		public virtual bool AllowTransitionToSameState 
		{
			get { return true; }
		}

		/// <summary>
		/// Adds the transitions. Override if needed
		/// </summary>
		protected virtual void AddTransitions(){}

		/// <summary>
		/// Sets transtition from state
		/// </summary>
		/// <param name="fromState">From state.</param>
		/// <param name="canTransitionFrom">If set to <c>true</c> can transition from.</param>
		protected void AddTransitionFrom(string fromState, bool canTransitionFrom)
		{
			if(!allowedTransitions.ContainsKey(fromState))
			{
				allowedTransitions.Add(fromState, canTransitionFrom);
			}
			else
			{
				allowedTransitions[fromState] = canTransitionFrom;
			}
		}

		/// <summary>
		/// Returns true if we can transition from the given state, to this state
		/// </summary>
		/// <returns><c>true</c> if this instance can transition from the specified fromState; otherwise, <c>false</c>.</returns>
		/// <param name="fromState">From state.</param>
		public bool CanTransitionFrom(string fromState)
		{
			return allowedTransitions.ContainsKey(fromState) && allowedTransitions[fromState];
		}

		/// <summary>
		/// Gets the target object.
		/// </summary>
		/// <value>The target object.</value>
		protected T TargetObject 
		{
			get { return targetObject; }
		}

		/// <summary>
		/// Gets the last state.
		/// </summary>
		/// <value>The last state.</value>
		public string LastState 
		{
			get { return lastState; }
		}
	}
}

