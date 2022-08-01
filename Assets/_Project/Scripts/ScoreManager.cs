using TMPro;
using UnityEngine;

namespace _Project.Scripts
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private Ball _ball;
        [SerializeField] private TextMeshProUGUI _leftScoreTMP;
        [SerializeField] private TextMeshProUGUI _rightScoreTMP;
    
        private int LeftScore { get; set; }
        private int RightScore { get; set; }

        private void Awake()
        {
            _ball.OnLeftScore += OnLeftScore;
            _ball.OnRightScore += OnRightScore;
        }

        private void OnRightScore()
        {
            RightScore++;
            _rightScoreTMP.text = RightScore.ToString();
        }

        private void OnLeftScore()
        {
            LeftScore++;
            _leftScoreTMP.text = LeftScore.ToString();
        }
    }
}
