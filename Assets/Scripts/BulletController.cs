using System.Collections;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("Bullet Info")]
    [SerializeField] private float activeTime;

    [Header("Particles")]
    [SerializeField] private GameObject damageParticle;    
    [SerializeField] private GameObject impactParticle;

    private int damage;

    public int Damage { get => damage; set => damage = value; }

    //When the gameobject SetActive = true
    private void OnEnable()
    {
        StartCoroutine(DeactiveAfterTime());
    }

    private IEnumerator DeactiveAfterTime()
    {
        yield return new WaitForSeconds(activeTime);
        gameObject.SetActive(false);
    }

    //when the bullet collide with something 
    private void OnTriggerEnter(Collider other)
    {
        //Deactive the bullet
        gameObject.SetActive(false);

        //Collision with enemy or player or floor or wall or object
        if (other.CompareTag("Enemy"))
        {
            //Instatiate damageParticle "Blood"
            GameObject particles = Instantiate(damageParticle,transform.position, Quaternion.identity);
            //Create Damage on Enemy
            other.GetComponent<EnemyController>().DamageEnemy(damage);

        }else if (other.CompareTag("Player"))
        {
            //Instatiate damageParticle "Blood"
            GameObject particles = Instantiate(damageParticle, transform.position, Quaternion.identity);
            //reduce life to Player
            other.GetComponent<PlayerController>().DamagePlayer(damage);

        }
        else
        {
            //Instatiate "Impact"Particle 
            GameObject particles = Instantiate(impactParticle, transform.position, Quaternion.identity);
        }

    }



}
