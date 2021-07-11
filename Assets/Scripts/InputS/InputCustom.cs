using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystemCustom
{
    public abstract class InputCustom
    {
        public PlayerInput playerInput;
        public abstract Vector2 GetDirection();
        public abstract bool IsFireActionPressed();
        public abstract Vector2 GetLasPosition();
    }
}