using UnityEngine;

namespace _Project.Scripts
{
    public class Player : MonoBehaviour
    {
        private const float _speed = 10;
        private const int _targetDistance = 10;
        private const float _boardHeight = 4;
        
        void Update()
        {
            Move();
        }

        private void Move()
        {
            if (InputManager.MoveUp())
            {
                DoMove(_targetDistance);
            }
            else if (InputManager.MoveDown())
            {
                DoMove(_targetDistance * -1);
            }
        }

        private void DoMove(float towards)
        {
            Vector2 pos = transform.position;
            pos.y = Mathf.MoveTowards(pos.y, towards, _speed * Time.deltaTime);
            if(IsUnderLimit(pos.y))
                transform.position = pos;
        }
        
        private bool IsUnderLimit(float y)
        {
            return y is < _boardHeight and > _boardHeight * -1;
        }
    }
}
