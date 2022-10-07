using UnityEngine;
using TMPro;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] public int health;
    public int maxHealth;

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
            GameObject ex = Instantiate(deathFx, transform.position, transform.rotation);
            GameObject.Destroy(ex, 2);

            // Add Points
            GameObject.Destroy(gameObject);
        }
    }
}
