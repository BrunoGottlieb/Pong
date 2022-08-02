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

        public static bool Restart()
        {
            return Input.GetKeyDown(KeyCode.R);
        }
        
        public static bool Pause()
        {
            return Input.GetKeyDown(KeyCode.P);
        }
        
        public static bool Escape()
        {
            return Input.GetKeyDown(KeyCode.Escape);
        }
    }
}
