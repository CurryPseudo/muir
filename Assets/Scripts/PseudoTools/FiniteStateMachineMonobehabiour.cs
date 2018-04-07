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
    public abstract class StateWithEvent : IState
    {
		public abstract IEnterEvents<T> GetEnterEvents();
        public void OnEnter(T fsm) {
			GetEnterEvents().Invoke(fsm);
			OnEnterWithEvent(fsm);
		}
		public virtual void OnEnterWithEvent(T fsm){} 
		public abstract IEnterEvents<T> GetExcuteEvents();
        public void OnExcute(T fsm) {
			GetExcuteEvents().Invoke(fsm);
			OnExcuteWithEvent(fsm);
		}
		public virtual void OnExcuteWithEvent(T fsm){} 
		public abstract IEnterEvents<T> GetExitEvents();
        public void OnExit(T fsm) {
			GetExitEvents().Invoke(fsm);
			OnExitWithEvent(fsm);
		}
		public virtual void OnExitWithEvent(T fsm){} 
    }
	public abstract class StateNormal<K> : StateWithEvent where K : StateSingleton<K>, new() {
		public static K Instance {
			get {
				return new K();
			}
		}
		
	}
    public abstract class StateSingleton<K> : StateWithEvent where K : StateSingleton<K> , new(){
		
		private static K instance;
		public static K Instance{
			get{
				if(instance == null) {
					instance = new K();
				}
				return instance;
			}
		}
		public ActionEventMap<T> enterEventMap = new ActionEventMap<T>();
		public override IEnterEvents<T> GetEnterEvents() {
			return enterEventMap;
		}
		public ActionEventMap<T> excuteEventMap = new ActionEventMap<T>();
		public override IEnterEvents<T> GetExcuteEvents() {
			return excuteEventMap;
		}
		public ActionEventMap<T> exitEventMap = new ActionEventMap<T>();
		public override IEnterEvents<T> GetExitEvents() {
			return exitEventMap;
		}
		public override string ToString() {
			return GetStateName();
		}
		public abstract string GetStateName();
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
	public void ChangeState<C>() where C : StateSingleton<C>, new(){
		ChangeState(StateSingleton<C>.Instance);
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
	private C GetStateInstance<C>() where C : StateSingleton<C>, new() {
		return StateSingleton<C>.Instance;
	}
	public void AddEnterEvent<C>(Action a, Func<C, ActionEventMap<T>> f) where C : StateSingleton<C>, new() {
		processEnterEvent<C>(a, f(GetStateInstance<C>()), ee => ee.AddEnterEvent);
	}
	public void AddEnterEvent<C>(Action a, Func<C, ActionEventMap<T>> f, MonoBehaviourHasDestroyEvent mb) where C : StateSingleton<C>, new() {
		processEnterEvent<C>(a, f(GetStateInstance<C>()), ee => ee.AddEnterEvent, mb);
	}
	public void RemoveEnterEvent<C>(Action a, Func<C, ActionEventMap<T>> f) where C : StateSingleton<C>, new() {
		processEnterEvent<C>(a, f(GetStateInstance<C>()), ee => ee.RemoveEnterEvent);
	}
}
