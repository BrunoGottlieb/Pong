using UnityEngine;

namespace _Project.Scripts
{
    public class InputManager : MonoBehaviour
    {
        public static bool MoveUp()
        {
            return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        }
    
        public static bool MoveDown()
        {
            return Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        }
    }
}
