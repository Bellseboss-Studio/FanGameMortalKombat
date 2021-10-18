using UnityEngine;
using UnityEngine.UI;

namespace View.UI
{
    public interface IAmuletPositioner
    {
        public bool MoveAmulet(Image amulet, float velocity, GameObject button, bool isOver, bool isMoving);
    }
}