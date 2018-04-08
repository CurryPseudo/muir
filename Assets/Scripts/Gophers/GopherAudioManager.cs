using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GopherAudioManager : MonoBehaviourHasDestroyEvent {
	#region Properties
	#endregion
	#region Private Methods And Fields

    private int playIndex = 0;
    private void StopPace() {
        if(pacesSource.isPlaying) {
            if(paceClips.Find((s)=>s == pacesSource.clip) != null) {
                pacesSource.Stop();
            }
        }
    }
    private void PlayPaces() {
        if(!pacesSource.isPlaying) {
            pacesSource.clip = paceClips[playIndex];
            pacesSource.PlayDelayed(0.2f);
            playIndex++;
            playIndex = playIndex % paceClips.Count;
        }
    }
    private void PlayLand() {
        if(movementFsm.lastStateName == "InAir") {
            pacesSource.clip = landSound;
            pacesSource.Play();
        }
    }
    private void PlayDead() {
        pacesSource.clip = DieSound;
        pacesSource.Play();
        List<int> voiceRandomValueSum = new List<int>();
        int sum = 0;
        for(int i = 0; i < voiceRandomValue.Count; i++) {
            sum += voiceRandomValue[i];
            voiceRandomValueSum.Add(sum);
        }
        int max = voiceRandomValueSum[voiceRandomValueSum.Count - 1];
        
        float randomValue = Random.Range(0, max);
        int index = voiceRandomValueSum.BinarySearch((int)randomValue);
        index = index < 0 ? ~index : index;
        if(index != 0) {
            voiceSource.clip = dieVoices[index - 1];
            voiceSource.Play();
        }
        
    }
    private void PlayJump() {
        float randomValue = Random.value;
        if(randomValue < jumpProbability) {
            voiceSource.clip = jumpSound;
            voiceSource.Play();
        }
    }
	#endregion	
	#region Inspector
    public MovementFsm movementFsm;
    public AudioSource pacesSource;
    public List<AudioClip> paceClips = new List<AudioClip>();
    public AudioClip landSound;
    public AudioSource voiceSource;
    public AudioClip DieSound;
    public List<AudioClip> dieVoices = new List<AudioClip>();
    public List<int> voiceRandomValue = new List<int>();
    public AudioClip jumpSound;
    public float jumpProbability = 0.4f;
	#endregion
	#region Monobehaviour Methods
    private void Awake() {
        movementFsm.AddEnterEventBeforeExcute<MovementFsm.OnGroundState>(PlayPaces, this);
        movementFsm.AddEnterEventBeforeEnter<MovementFsm.OnGroundState>(PlayLand, this);
        movementFsm.AddEnterEventBeforeExit<MovementFsm.OnGroundState>(StopPace, this);
        movementFsm.AddEnterEventBeforeEnter<MovementFsm.DeadState>(PlayDead, this);
        movementFsm.AddEnterEventBeforeEnter<MovementFsm.InAirState>(PlayJump, this);
        Debug.Assert(dieVoices.Count == voiceRandomValue.Count - 1);

    }
	#endregion
	#region Public Method
	#endregion
}
