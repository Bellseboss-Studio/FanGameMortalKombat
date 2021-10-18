using MenuUI;
using UnityEngine;
using UnityEngine.UI;

namespace View.UI
{
    public class MoveTowardsAdapter : IAmuletPositioner
    {
        public bool MoveAmulet(Image amulet, float velocity, GameObject button, bool isOver, bool isMoving)
        {
            if (isOver != true && isMoving != true) return false;
            var amuletPosition = amulet.transform.position;
            var buttonPosition = button.transform.position;
            var step = velocity * Time.deltaTime * Vector3.Distance(amuletPosition, new Vector3(amuletPosition.x, buttonPosition.y, 0));
            amuletPosition = Vector3.MoveTowards(amuletPosition, new Vector3(amuletPosition.x, buttonPosition.y, 0), step);
            amulet.transform.position = amuletPosition;
            return !(Vector3.Distance(amuletPosition, new Vector3(amuletPosition.x, buttonPosition.y, 0)) < 1);
        }
    }
}