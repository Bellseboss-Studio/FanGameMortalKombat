using System;
using Bellseboss.Pery.Scripts.Input;
using Cinemachine;

public interface ICameraTrigger
{
    void Config(CharacterV2 character, int i);
    CinemachineVirtualCameraBase Camera { get; }
    Action<int> ChangeCamera { get; set; }
}