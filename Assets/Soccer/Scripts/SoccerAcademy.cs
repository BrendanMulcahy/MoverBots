using System.Collections.Generic;
using MLAgents;
using UnityEngine;

namespace Assets.Soccer.Scripts
{
    public class SoccerAcademy : Academy
    {
        [SerializeField] private SoccerBall _ball;
        [SerializeField] private List<SoccerAgent> _blueAgents;
        [SerializeField] private List<SoccerAgent> _redAgents;

        public override void AcademyReset()
        {
            _ball.Reset();
        }

        public void Score(bool blueScore)
        {
            if (blueScore)
            {
                _blueAgents.ForEach(a => a.RewardGoal());
                _redAgents.ForEach(a => a.PenaltyGoal());
            }
            else
            {
                _redAgents.ForEach(a => a.RewardGoal());
                _blueAgents.ForEach(a => a.PenaltyGoal());
            }

            Done();
        }
    }
}