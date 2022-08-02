using UnityEngine;

namespace _Project.Scripts
{
    public class InputManager : MonoBehaviour
    {
        public static bool MoveUp()
        {
            return Input.GetAxis("Vertical") > 0.1f;
        }
    
        public static bool MoveDown()
        {
            return Input.GetAxis("Vertical") < -0.1f;
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
