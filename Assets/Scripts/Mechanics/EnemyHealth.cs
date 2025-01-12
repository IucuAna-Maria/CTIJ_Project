using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyHealth : MonoBehaviour
{
    public int health;

    public GameObject gameCompletePanel;
    public GameObject pauseButton;
    public string victorySceneName;

    private void Update()
    {
        if (health <= 0)
        {
            GameComplete();
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    private void GameComplete()
    {
        
        if (SceneManager.GetActiveScene().name == victorySceneName)
        {
            if (gameCompletePanel != null && pauseButton != null)
            {
                Time.timeScale = 0f;
                gameCompletePanel.SetActive(true);
                pauseButton.SetActive(false);

            }
        }
        
        Destroy(gameObject);
    }
}
