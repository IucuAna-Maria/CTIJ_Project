using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    public float distance;
    public int damage;
    public LayerMask Enemy;

    private void Start()
    {
        Invoke("DestroyProjectile", lifeTime);
    }

    private void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.right, distance, Enemy);
        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                Debug.Log("enemy hit");
                hitInfo.collider.GetComponent<EnemyHealth>().TakeDamage(damage);
            }
            DestroyProjectile();
        }

        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
