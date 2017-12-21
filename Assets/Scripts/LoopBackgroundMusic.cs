using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopBackgroundMusic : MonoBehaviour {
	#region Properties
	#endregion
	#region Private Methods And Fields
    private int nowPlayIndex = 0;
	#endregion	
	#region Inspector
    public List<AudioClip> musics = new List<AudioClip>();
    public AudioSource source;
	#endregion
	#region Monobehaviour Methods
    void Awake() {
        Debug.Assert(musics.Count != 0);
    }
    void Update() {
        if(!source.isPlaying) {
            source.clip = musics[nowPlayIndex];
            source.Play();
            nowPlayIndex++;
            if(nowPlayIndex == musics.Count) {
                nowPlayIndex = 0;
            } 
        }
    }
	#endregion
	#region Public Method
	#endregion
}
