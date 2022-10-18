using UnityEngine;
using TMPro;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] private int health;
    private int maxHealth;

    [SerializeField] GameObject floatingText;
    [SerializeField] GameObject deathFx;

    private void Start()
    {
        maxHealth = health;
    }

    public void TakeDMG(int dmg)
    {
        health = Mathf.Clamp(health -= dmg, 0, maxHealth);

        GameObject text = GameObject.Instantiate(floatingText, transform.position, transform.rotation);
        text.GetComponent<TextMeshPro>().text = dmg.ToString();
        GameObject.Destroy(text, 0.5f);

        // Add Points

        if (health <= 0)
        {
            // Add Points
            // Die
            GameObject.Destroy(gameObject); // just here to temp kill the enemy will be replaced with an actual death anim once made
        }
    }
}