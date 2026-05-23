using Game.Scripts.InventoryScripts;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Nokobot/Modern Guns/Simple Shoot")]
public class SimpleShoot : MonoBehaviour
{
    [Header("Prefab Refrences")]
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;

    [Header("Location Refrences")]
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;

    [Header("Settings")]
    [Tooltip("Specify time to destory the casing object")] [SerializeField] private float destroyTimer = 2f;
    [Tooltip("Bullet Speed")] [SerializeField] private float shotPower = 500f;
    [Tooltip("Casing Ejection Speed")] [SerializeField] private float ejectPower = 150f;

    public AudioSource Source;
    public AudioSource reload;

    private InventoryManager inventoryManager;
    private QuickSlotInventory quickSlotInventory;

    public Transform firePoint;
    public Text bulletText;

    public int maxAmmo = 20;
    private int currentAmmo;
    private float nextTimeToFire = 0f;

    void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        quickSlotInventory = FindObjectOfType<QuickSlotInventory>();

        currentAmmo = maxAmmo;  
        bulletText.text = currentAmmo.ToString();

        if (barrelLocation == null)
            barrelLocation = transform;

        if (gunAnimator == null)
            gunAnimator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        bulletText.text = currentAmmo.ToString();

        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire && currentAmmo > 0)
        {
            if (quickSlotInventory.activeSlot != null && quickSlotInventory.activeSlot.item != null && quickSlotInventory.activeSlot.item.itemType == ItemType.Weapon && !inventoryManager.isOpened)
            {
                gunAnimator.SetTrigger("Fire");
                

            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
            reload.Play();

        }
    }

    void Reload()
    {
        InventorySlot magazineSlot = inventoryManager.FindItemSlot(ItemType.PistolMagazine);

        if (magazineSlot != null && magazineSlot.amount > 0)
        {
            currentAmmo = maxAmmo;

            magazineSlot.amount--;
            if (magazineSlot.amount == 0)
            {
                magazineSlot.ClearSlot();
            }
            else
            {
                magazineSlot.itemAmountText.text = magazineSlot.amount.ToString();
            }
        }

        bulletText.text = currentAmmo.ToString();
    }

    void Shoot()
    {
        if (currentAmmo > 0)
        {
            if (muzzleFlashPrefab)
            {
                GameObject tempFlash;
                tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);
                Destroy(tempFlash, destroyTimer);
            }

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            BulletMovement bulletMovement = bullet.AddComponent<BulletMovement>();
            bulletMovement.SetSpeed(30f);
            currentAmmo -= 1;
            Source.Play();
            StartCoroutine(DestroyBulletAfterTime(bullet, 4f));
        }
    }

    void CasingRelease()
    {
        if (!casingExitLocation || !casingPrefab)
            return;

        GameObject tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation);
        tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower), (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
        tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);
        Destroy(tempCasing, destroyTimer);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        if (bullet == null) yield break;
        yield return new WaitForSeconds(delay);
        if (bullet != null)
        {
            Destroy(bullet);
        }
    }

    public void ChangeBulletCount(int bulletChange)
    {
        if (currentAmmo == 0)
        {
            currentAmmo += bulletChange;
        }
    }
}
public class BulletMovement : MonoBehaviour
{
    private float speed;

    public void SetSpeed(float bulletSpeed)
    {
        speed = bulletSpeed;
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}