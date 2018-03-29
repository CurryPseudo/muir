using UnityEngine;
[ExecuteInEditMode]
public abstract class CouldDisableEditorUpdateMonoBehaviour : MonoBehaviour {
    public bool editorUpdate = false;
    public void Update() {
        if(editorUpdate || Application.isPlaying) {
            _Update();
        }
    }
    public abstract void _Update();
}