using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private Transform barrel;

    [Header("Ammo")]
    [SerializeField] private int currentAmmo;
    [SerializeField] private int maxAmmo;
    [SerializeField] private bool infiniteAmmo;

    [Header("Performance")]
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float shootRate;
    [SerializeField] private int damage;

    private ObjectPool objectPool;
    private float lastShootTime;

    private bool isPlayer;

    public float ShootRate { get => shootRate; set => shootRate = value; }

    private void Awake()
    {
        //Check if I am a Player
        isPlayer = GetComponent<PlayerMovement>() != null;

        //get objectPool component
        objectPool = GetComponent<ObjectPool>();
    }

    /// <summary>
    /// check if is possible to shoot
    /// </summary>
    /// <returns>bool</returns>
    public bool CanShoot()
    {
        //Check shootRate
        if (Time.time - lastShootTime >= ShootRate)
        {
            //Check Ammo
            if (currentAmmo >0 || infiniteAmmo)
            {
                return true;
            }
        }

        return false;

    }

    /// <summary>
    /// Handle Weapon Shoot
    /// </summary>
    public void Shoot()
    {
        //update last Shoot Time
        lastShootTime = Time.time;

        //reduce the Ammo
        if (!infiniteAmmo) currentAmmo--;

        //Get a new bullet
        GameObject bullet = objectPool.GetGameObject();

        //Locate the ball at the barrel position
        bullet.transform.position = barrel.position;
        bullet.transform.rotation = barrel.rotation;
        //assign damage to Bullet
        bullet.GetComponent<BulletController>().Damage = damage;

        if (isPlayer)
        {
            //Create Ray from Camera to the middle of the screen
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            RaycastHit hit;
            Vector3 targetPoint;

            //Check if your are pointing to something and adjust the direction
            if (Physics.Raycast(ray, out hit))
                targetPoint = hit.point;
            else
                targetPoint = ray.GetPoint(5); //Get a point at 5m

            bullet.GetComponent<Rigidbody>().linearVelocity = (targetPoint - barrel.position).normalized * bulletSpeed;

        }
        //Enemy Shoot
        else
        {
            //Give velocity to Bullet 
            bullet.GetComponent<Rigidbody>().linearVelocity = barrel.forward * bulletSpeed;
        }

        

    }


}
