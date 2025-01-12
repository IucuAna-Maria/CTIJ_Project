using Platformer.Core;
using Platformer.Model;
using UnityEngine;
using UnityEngine.UI; 
namespace Platformer.UI
{
    public class ScoreDisplay : MonoBehaviour
    {
        public Text scoreText;  

        private PlatformerModel model;

        void Start()
        {
            
            model = Simulation.GetModel<PlatformerModel>();
        }

        void Update()
        {
            // Update the text with the current number of collected tokens
            scoreText.text = " Diamonds : " + model.collectedTokens.ToString();
        }
    }
}
