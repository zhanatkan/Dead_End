using UnityEngine;
using UnityEngine.UI;

namespace BigRookGames.Weapons
{
    public class GunfireController : MonoBehaviour
    {
        public AudioClip GunShotClip;
        public AudioSource source;
        public Vector2 audioPitch = new Vector2(0.9f, 1.1f);

        public GameObject muzzlePrefab;
        public GameObject muzzlePosition;

        public bool autoFire;
        public float shotDelay = 0.1f;
        public bool rotate = true;
        public float rotationSpeed = 0.25f;

        public GameObject scope;
        public bool scopeActive = true;
        private bool lastScopeState;

        [Tooltip("The projectile gameObject to instantiate each time the weapon is fired.")]
        public GameObject projectilePrefab;
        [Tooltip("Sometimes a mesh will want to be disabled on fire. For example: when a rocket is fired, we instantiate a new rocket, and disable" +
            " the visible rocket attached to the rocket launcher")]
        public GameObject projectileToDisableOnFire;

        [SerializeField] private float timeLastFired;

        private InventoryManager inventoryManager;
        private QuickSlotInventory quickSlotInventory;

        public Transform firePoint;
        public GameObject bulletPrefab;
        public Text bulletText;

        public int maxAmmo = 30;
        private int currentAmmo;
        private float nextTimeToFire = 0f;


        private void Start()
        {
            inventoryManager = FindObjectOfType<InventoryManager>();
            quickSlotInventory = FindObjectOfType<QuickSlotInventory>();

            currentAmmo = maxAmmo;
            bulletText.text = currentAmmo.ToString();
            if (source != null) source.clip = GunShotClip;
            timeLastFired = 0;
            lastScopeState = scopeActive;
        }

        private void Update()
        {
            if (PauseGame.isPaused) return;
            bulletText.text = currentAmmo.ToString();
            if (Input.GetKeyDown(KeyCode.R) && currentAmmo == 0)
            {
                Reload();
            }
            if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
            {
                if (quickSlotInventory.activeSlot != null)
                {
                    if (quickSlotInventory.activeSlot.item != null)
                    {
                        if (quickSlotInventory.activeSlot.item.itemType == ItemType.Gun)
                        {
                            if (inventoryManager.isOpened == false)
                            {
                                if (rotate)
                                {
                                    transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + rotationSpeed, transform.localEulerAngles.z);
                                }
                                if (autoFire && ((timeLastFired + shotDelay) <= Time.time))
                                {
                                    FireWeapon();
                                }
                                if (scope && lastScopeState != scopeActive)
                                {
                                    lastScopeState = scopeActive;
                                    scope.SetActive(scopeActive);
                                }
                                return;
                            }
                            else return;
                        }
                        else return;
                    }
                    else return;
                }
                else return;
            }
            else return;
        }

        public void FireWeapon()
        {
            timeLastFired = Time.time;

            var flash = Instantiate(muzzlePrefab, muzzlePosition.transform);

            if (projectilePrefab != null)
            {
                GameObject newProjectile = Instantiate(projectilePrefab, muzzlePosition.transform.position, muzzlePosition.transform.rotation, transform);
            }

            if (projectileToDisableOnFire != null)
            {
                projectileToDisableOnFire.SetActive(false);
                Invoke("ReEnableDisabledProjectile", 1);
            }
            if (source != null)
            {
                if (source.transform.IsChildOf(transform))
                {
                    source.Play();
                }
                else
                {
                    AudioSource newAS = Instantiate(source);
                    if ((newAS = Instantiate(source)) != null && newAS.outputAudioMixerGroup != null && newAS.outputAudioMixerGroup.audioMixer != null)
                    {
                        newAS.outputAudioMixerGroup.audioMixer.SetFloat("Pitch", Random.Range(audioPitch.x, audioPitch.y));
                        newAS.pitch = Random.Range(audioPitch.x, audioPitch.y);

                        newAS.PlayOneShot(GunShotClip);

                        Destroy(newAS.gameObject, 2);
                    }
                }
            }
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            BulletMovement bulletMovement = bullet.AddComponent<BulletMovement>();
            bulletMovement.SetSpeed(30f);
            currentAmmo -= 1;
            StartCoroutine(DestroyBulletAfterTime(bullet, 4f));
        }
        private System.Collections.IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
        {
            if (bullet == null) yield break;

            yield return new WaitForSeconds(delay);

            if (bullet != null)
            {
                Destroy(bullet);
            }
        }
        private void ReEnableDisabledProjectile()
        {
            projectileToDisableOnFire.SetActive(true);
        }
        void Reload()
        {
            // Проверяем наличие магазина в инвентаре
            InventorySlot magazineSlot = inventoryManager.FindItemSlot(ItemType.RiffleMagazine);

            if (magazineSlot != null && magazineSlot.amount > 0)
            {
                // Устанавливаем количество патронов после перезарядки
                currentAmmo = maxAmmo;

                // Уменьшаем количество магазинов в инвентаре
                magazineSlot.amount--;
                if (magazineSlot.amount == 0)
                {
                    magazineSlot.ClearSlot(); // Очищаем слот, если магазин израсходован
                }
                else
                {
                    // Обновляем текст количества, если магазин еще не исчерпан
                    magazineSlot.itemAmountText.text = magazineSlot.amount.ToString();
                }

                Debug.Log("Оружие перезаряжено!");
            }
            else
            {
                Debug.Log("Нет магазина для перезарядки!");
            }

            bulletText.text = currentAmmo.ToString();
        }

    }
}