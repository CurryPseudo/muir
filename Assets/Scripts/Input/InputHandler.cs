using UnityEngine;
using System.Collections.Generic;
using PseudoTools;
public class InputHandler : Singleton<InputHandler>
{
    private Dictionary<RuntimePlatform, IMuirInput> platformMap = new Dictionary<RuntimePlatform, IMuirInput>();
    public InputHandler()
    {
        addMap(new PcInput());
    }
    private void addMap(IMuirInput i) {
        foreach(var platform in i.GetPlatforms()) {
            platformMap.Add(platform, i);
        }
    }
    public static IMuirInput Input{
        get{
            return InputHandler.Instance.platformMap[Application.platform];
        }
    }
}
public interface IMuirInput {
    RuntimePlatform[] GetPlatforms();
    bool GetJumpButton();
    bool GetDigButton();
}
public class PcInput : IMuirInput
{
    bool IMuirInput.GetDigButton()
    {
        return Input.GetButton("Dig");
    }

    bool IMuirInput.GetJumpButton()
    {
        return Input.GetButton("Jump");
    }

    RuntimePlatform[] IMuirInput.GetPlatforms()
    {
        return new RuntimePlatform[] { RuntimePlatform.WindowsPlayer, RuntimePlatform.WindowsEditor, RuntimePlatform.OSXEditor, RuntimePlatform.OSXPlayer, RuntimePlatform.LinuxEditor, RuntimePlatform.LinuxPlayer};
    }
}