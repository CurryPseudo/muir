using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FiniteStateMachineMonobehaviour<T> : MonoBehaviour where T : FiniteStateMachineMonobehaviour<T>{ 

	public string currentStateName;
	public string lastStateName;
	public IState current;
	public interface IState{
		void OnEnter(T fsm);
		void OnExcute(T fsm);
		void OnExit(T fsm);
		string ToString();
	}
    public abstract class StateNormal<K> : IState where K : new()
    {
		public static K Instance {
			get {
				return new K();
			}
		}
        public void OnEnter(T fsm) {
			GetEnterEvents().Invoke(fsm);
			OnEnterWithEvent(fsm);
		}
		public virtual void OnEnterWithEvent(T fsm){} 
        public void OnExcute(T fsm) {
			GetExcuteEvents().Invoke(fsm);
			OnExcuteWithEvent(fsm);
		}
		public virtual void OnExcuteWithEvent(T fsm){} 
        public void OnExit(T fsm) {
			GetExitEvents().Invoke(fsm);
			OnExitWithEvent(fsm);
		}
		public virtual void OnExitWithEvent(T fsm){} 
		private static ActionEventMap<T> enterEventMap = new ActionEventMap<T>();
		public static IEnterEvents<T> GetEnterEvents() {
			return enterEventMap;
		}
		private static ActionEventMap<T> excuteEventMap = new ActionEventMap<T>();
		public static IEnterEvents<T> GetExcuteEvents() {
			return excuteEventMap;
		}
		private static ActionEventMap<T> exitEventMap = new ActionEventMap<T>();
		public static IEnterEvents<T> GetExitEvents() {
			return exitEventMap;
		}
		public override string ToString() {
			return GetStateName();
		}
		public abstract string GetStateName();
    }
    public abstract class StateSingleton<K> : StateNormal<K> where K : StateSingleton<K> , new(){
		
		private static K instance;
		public new static K Instance{
			get{
				if(instance == null) {
					instance = new K();
				}
				return instance;
			}
		}
	}
	/// <summary>
	/// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
	/// </summary>
	private void FixedUpdate()
	{
		FixedUpdateBeforeFSMUpdate();
		if(current != null){
			current.OnExcute(this as T);
			
		}
		FixedUpdateAfterFSMUpdate();
	}
	protected virtual void FixedUpdateBeforeFSMUpdate() {}
	protected virtual void FixedUpdateAfterFSMUpdate() {}
	public void ChangeState<C>(StateNormal<C> stateInstance) where C : StateNormal<C>, new(){
		if(stateInstance == null) {
			stateInstance = new C();
		}
        ChangeState((IState)stateInstance);
	}
	public void ChangeState<C>() where C : StateSingleton<C>, new(){
        ChangeState((IState)StateSingleton<C>.Instance);
	}
	private void ChangeState(IState newState) {
		if(current != null){
			lastStateName = current.ToString();
			current.OnExit(this as T);
		}
		current = newState;
		current.OnEnter(this as T);
		currentStateName = current.ToString();
	}
	private void processEnterEvent<C>(Action a, IEnterEvents<T> ee, Func<IEnterEvents<T>, Action<T, Action>> processAction) {
		processAction(ee)(this as T, a);
	}
	private void processEnterEvent<C>(Action a, IEnterEvents<T> ee, Func<IEnterEvents<T>, Action<T, Action, MonoBehaviourHasDestroyEvent>> processAction, MonoBehaviourHasDestroyEvent mb) {
		processAction(ee)(this as T, a, mb);
	}
	public void AddEnterEventBeforeEnter<C>(Action a) where C : StateNormal<C>, new() {
		processEnterEvent<C>(a, StateNormal<C>.GetEnterEvents(), ee => ee.AddEnterEvent);
	}
	public void AddEnterEventBeforeExcute<C>(Action a) where C : StateNormal<C>, new() {
		processEnterEvent<C>(a, StateNormal<C>.GetExcuteEvents(), ee => ee.AddEnterEvent);
	}
	public void AddEnterEventBeforeExit<C>(Action a) where C : StateNormal<C>, new() {
		processEnterEvent<C>(a, StateNormal<C>.GetExitEvents(), ee => ee.AddEnterEvent);
	}
	public void AddEnterEventBeforeEnter<C>(Action a, MonoBehaviourHasDestroyEvent mb) where C : StateNormal<C>, new() {
		processEnterEvent<C>(a, StateNormal<C>.GetEnterEvents(), ee => ee.AddEnterEvent, mb);
	}
	public void AddEnterEventBeforeExcute<C>(Action a, MonoBehaviourHasDestroyEvent mb) where C : StateNormal<C>, new() {
		processEnterEvent<C>(a, StateNormal<C>.GetExcuteEvents(), ee => ee.AddEnterEvent, mb);
	}
	public void AddEnterEventBeforeExit<C>(Action a, MonoBehaviourHasDestroyEvent mb) where C : StateNormal<C>, new() {
		processEnterEvent<C>(a, StateNormal<C>.GetExitEvents(), ee => ee.AddEnterEvent, mb);
	}
	public void RemoveEnterEventBeforeEnter<C>(Action a) where C : StateNormal<C>, new() {
		processEnterEvent<C>(a, StateNormal<C>.GetEnterEvents(), ee => ee.RemoveEnterEvent);
	}
	public void RemoveEnterEventBeforeExcute<C>(Action a) where C : StateNormal<C>, new() {
		processEnterEvent<C>(a, StateNormal<C>.GetExcuteEvents(), ee => ee.RemoveEnterEvent);
	}
	public void RemoveEnterEventBeforeExit<C>(Action a) where C : StateNormal<C>, new() {
		processEnterEvent<C>(a, StateNormal<C>.GetExitEvents(), ee => ee.RemoveEnterEvent);
	}
}
