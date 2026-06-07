using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CharacterShooting : MonoBehaviour
{
    [Header("Mechanics")]
    public float bulletSpeed = 20f;
    public float fireRate = 0.5f;
    public int maxAmmo = 30;
    private int currentAmmo;
    private float nextTimeToFire = 0f;
    [Header("Links")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    //public InventoryManager inventoryManager;
    //public QuickSlotInventory quickSlotInventory;
    public Text bulletText;


    private void Start()
    {
        currentAmmo = maxAmmo; 
        bulletText.text = currentAmmo.ToString();
    }

    private void Update()
    {
        bulletText.text = currentAmmo.ToString();
        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time >= nextTimeToFire && currentAmmo > 0)
        {
            /*if (quickSlotInventory.activeSlot != null)
            {
                if (quickSlotInventory.activeSlot.item != null)
                {
                    if (quickSlotInventory.activeSlot.item.itemType == ItemType.Weapon)
                    {
                        if (inventoryManager.isOpened == false)
                        {
                            Shoot();
                            nextTimeToFire = Time.time + fireRate;
                        }
                    }
                }
            }*/
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        BulletMovement bulletMovement = bullet.AddComponent<BulletMovement>();
        bulletMovement.SetSpeed(bulletSpeed);

        currentAmmo--;
        StartCoroutine(DestroyBulletAfterTime(bullet, 2f));
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        if (bullet == null)
        {
            yield break;
        }

        yield return new WaitForSeconds(delay);

        if (bullet != null)
        {
            Destroy(bullet);
        }
    }
}
