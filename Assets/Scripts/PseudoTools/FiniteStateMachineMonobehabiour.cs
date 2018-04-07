using System.Collections;
using System.Collections.Generic;
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
	public abstract class State<K> : IState where K : State<K> ,new(){
		private static K instance;
		public static K Instance{
			get{
				if(instance == null) {
					instance = new K();
				}
				return instance;
			}
		}
		protected State(){
		}
		public virtual void OnEnter(T fsm){}
		public virtual void OnExcute(T fsm){}
		public virtual void OnExit(T fsm){}
		public override string ToString() {
			return GetStateName();
		}
		public abstract string GetStateName();
	}
	public abstract class StateWithEvent<K> : State<K> where K : State<K> , new(){
		
		public ActionEventMap<T> enterEventMap = new ActionEventMap<T>();
		public sealed override void OnEnter(T fsm){
			enterEventMap.Invoke(fsm);
			OnEnterWithEvent(fsm);
		}
		public virtual void OnEnterWithEvent(T fsm){} 

		public ActionEventMap<T> excuteEventMap = new ActionEventMap<T>();
		public sealed override void OnExcute(T fsm){
			excuteEventMap.Invoke(fsm);
			OnExcuteWithEvent(fsm);
		}
		public virtual void OnExcuteWithEvent(T fsm){}
		public ActionEventMap<T> exitEventMap = new ActionEventMap<T>();
		public sealed override void OnExit(T fsm){
			exitEventMap.Invoke(fsm);
			OnExitWithEvent(fsm);
		}
		public virtual void OnExitWithEvent(T fsm){}
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
	public void ChangeState(IState newState){
		if(current != null){
			lastStateName = current.ToString();
			current.OnExit(this as T);
		}
		current = newState;
		current.OnEnter(this as T);
		currentStateName = current.ToString();
	}
	public void AddEnterEvent<C>(System.Action a, System.Func<C, ActionEventMap<T>> f) where C : StateWithEvent<C>, new() {
		f(StateWithEvent<C>.Instance).AddEnterEvent(this as T, a);
	}
	public void AddEnterEvent<C>(System.Action a, System.Func<C, ActionEventMap<T>> f, MonoBehaviourHasDestroyEvent mb) where C : StateWithEvent<C>, new() {
		f(StateWithEvent<C>.Instance).AddEnterEvent(this as T, a, mb);
	}
	public void RemoveEnterEvent<C>(System.Action a, System.Func<C, ActionEventMap<T>> f) where C : StateWithEvent<C>, new() {
		f(StateWithEvent<C>.Instance).RemoveEnterEvent(this as T, a);
	}
}
