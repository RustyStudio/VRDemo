using InteractionDemo.Core;
using InteractionDemo.Interaction;
using InteractionDemo.WorldObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

namespace InteractionDemo.ArcheryZone
{
    class ArcheryHallManager : MonoBehaviour
    {
        private static ArcheryHallManager _instance;

        public static ArcheryHallManager Instance
        {
            get
            {
                return _instance;
            }
        }

        public TextMeshPro ScoreBoard;

        public TextMeshPro HintsBoard;

        public TextMeshPro LastHitScore;

        public float CurrentScore = 0f;
               
        void Awake()
        {
            _instance = this;
        }


        public void RegisterProjectile(Projectile proj)
        {
            proj.OnProjectileHit.AddListener(ProcessArrowHit);
        }

        public void ProcessArrowHit(Collision c)
        {
            var target = c.gameObject.GetComponentInParent<ArrowTarget>();
            Debug.Log("HIT!");
            if (target)
            {
                var score = target.GetScore(c.contacts[0].point);
                CurrentScore += score;
                var scoreText = score.ToString("F0");
                LastHitScore.text = scoreText;
                var curscoreText = CurrentScore.ToString("F0");
                ScoreBoard.text = curscoreText;
            }         
        }
    }
}
