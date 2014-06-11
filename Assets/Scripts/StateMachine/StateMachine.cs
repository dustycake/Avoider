using System;
using System.Collections.Generic;
using UnityEngine;
namespace Core
{
	public class StateMachine<T> : MonoBehaviour
	{
		public string StringInAvailableStateNames = "State";

		// Target to manage
		private T targetObject;

		// Current state
		private string currentStateName = "none";

		// List of available state names
		private Dictionary<string, State<T>> availableStates = new Dictionary<string, State<T>>();

		// Transition limits
		private bool preventUndefinedTransitions = true;

		/// <summary>
		/// Initializes a new instance of the <see cref="Core.StateMachine`1"/> class.
		/// </summary>
		/// <param name="targetObject">Target object.</param>
		public void Init(T targetObject)
		{
			this.targetObject = targetObject;

			// Grab available state names
			MonoBehaviour[] candidateStates = gameObject.GetComponents<MonoBehaviour>();
			foreach(MonoBehaviour candidateState in candidateStates)
			{
				string stateName = candidateState.GetType().Name;
				if(stateName.Contains(StringInAvailableStateNames))
				{
					if(candidateState is State<T>)
					{
						State<T> state = candidateState as State<T>;
						state.Init();
						candidateState.enabled = false;
						availableStates.Add(stateName, state);
					}
					else
					{
						Debug.Log("StateMachine - gameObject has a non-state monobehavior with " + StringInAvailableStateNames + " in the name. Please evaluate this naming");
					}
				}
			}
		}

		/// <summary>
		/// Changes the state.
		/// </summary>
		/// <param name="newState">New state.</param>
		/// <param name="transitionData">Transition data.</param>
		public void ChangeState(string newStateName, string transitionData)
		{
			if(!availableStates.ContainsKey(newStateName))
			{
				Debug.Log("StateMachine - " + newStateName + " is not a valid state");
				return;
			}

			// Try to retrieve last and new state
			State<T> currentState = GetState(currentStateName);
			State<T> newState = GetState(newStateName);

			// New and old are valid
			if(newStateName != null && currentState != null)
			{
				// New and old are the same, check availability
				if(newState.GetType() == currentState.GetType())
				{
					// Good to go
					if(newState.AllowTransitionToSameState)
					{
						ChangeStateHelper(newStateName, transitionData);
					}

					// Invalide
					else
					{
						Debug.Log("StateMachine - cannot transition to self from: " + currentState.GetType());
						return;
					}
				}

				// Transition is valid
				else if(IsTransitionAvailable(currentStateName, newStateName))
				{
					ChangeStateHelper(newStateName, transitionData);
				}

				// Transition not valid
				else
				{
					Debug.Log("StateMachine - cannot transition from " + currentState.GetType() + " to :" + newState.GetType());
					return;
				}
			}
			else
			{
				ChangeStateHelper(newStateName, transitionData);
			}
		}

		/// <summary>
		/// Changes the last state if one is available
		/// </summary>
		public void ChangeToLastState(string transitionData)
		{
			State<T> currentState = GetState(currentStateName);
			if(currentState == null)
			{
				Debug.Log("StateMachine - trying to change to last state, when current state is not valid");
				return;
			}

			State<T> lastState = GetState(currentState.LastState);
			if(lastState == null)
			{
				Debug.Log("StateMachine - trying to change to last state, when last state is null");
				return;
			}

			ChangeState(currentState.LastState, transitionData);
		}

		/// <summary>
		/// Performs state change after all validity checked.
		/// </summary>
		/// <param name="newState">New state.</param>
		/// <param name="transitionData">Transition data.</param>
		private void ChangeStateHelper(string newStateName, string transitionData)
		{
			// Get states in question
			// Try to retrieve starting state
			State<T> newState = GetState(newStateName);
			State<T> currentState = GetState(currentStateName);

			// Exit old
			if(currentState != null)
			{
				currentState.enabled = false;
				currentState.OnExit();
			}

			// Enter new
			if(newState != null)
			{
				currentState = newState;
				currentState.OnEnter(targetObject, currentStateName, transitionData);
				currentState.enabled = true;
			}

			currentStateName = newStateName;
		}

		/// <summary>
		/// Determines whether this instance is transition available the specified startingState destinationState.
		/// </summary>
		/// <returns><c>true</c> if this instance is transition available the specified startingState destinationState; otherwise, <c>false</c>.</returns>
		/// <param name="startingState">Starting state.</param>
		/// <param name="destinationState">Destination state.</param>
		private bool IsTransitionAvailable(string startingState, string destinationState)
		{
			// We have no limits on transitions
			if(!preventUndefinedTransitions)
			{
				return true;
			}

			State<T> newState = GetState(destinationState);
			State<T> currentState = GetState(startingState);

			if(currentState == null || newState == null)
			{
				return true;
			}
			else if(currentState != null && newState != null)
			{
				return newState.CanTransitionFrom(startingState);
			}

			return true;
		}

		/// <summary>
		/// Gets the state.
		/// </summary>
		/// <returns>The state.</returns>
		/// <param name="stateName">State name.</param>
		private State<T> GetState(string stateName)
		{
			if(availableStates.ContainsKey(stateName))
			{
				return availableStates[stateName];
			}
			return null;
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Core.StateMachine`1"/> prevent undefined transitions.
		/// </summary>
		/// <value><c>true</c> if prevent undefined transitions; otherwise, <c>false</c>.</value>
		public bool PreventUndefinedTransitions 
		{
			get { return preventUndefinedTransitions; }
			set { preventUndefinedTransitions = value; }
		}

		/// <summary>
		/// Gets the name of the current state.
		/// </summary>
		/// <value>The name of the current state.</value>
		public string CurrentStateName 
		{
			get { return currentStateName; }
		}

		/// <summary>
		/// Gets the current state
		/// </summary>
		/// <value>The name of the current state.</value>
		public State<T> CurrentState
		{
			get { return availableStates[currentStateName]; }
		}
	}
}

