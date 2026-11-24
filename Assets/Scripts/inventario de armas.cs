using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Coloque no JOGADOR. Guarda as armas coletadas, troca entre elas
/// e coleta armas próximas ao apertar E.
/// O jogador precisa de um Collider2D e de um Rigidbody2D.
/// </summary>
public class WeaponInventory : MonoBehaviour
{
    [Header("Configuração")]
    [Tooltip("Objeto filho do jogador onde as armas ficam (posição da mão/canhão).")]
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private int maxWeapons = 4;
    [SerializeField] private KeyCode pickupKey = KeyCode.E;
    [SerializeField] private KeyCode nextWeaponKey = KeyCode.Q;

    [Header("Debug (somente leitura)")]
    [SerializeField] private int currentIndex = -1;

    // Armas guardadas (instâncias desativadas dentro do holder)
    private readonly List<WeaponSlot> weapons = new List<WeaponSlot>();

    // Pickup que está ao alcance do jogador agora
    private WeaponPickup nearbyPickup;

    private class WeaponSlot
    {
        public string weaponName;
        public GameObject instance;
    }

    public GameObject CurrentWeapon =>
        currentIndex >= 0 && currentIndex < weapons.Count ? weapons[currentIndex].instance : null;

    private void Awake()
    {
        if (weaponHolder == null) weaponHolder = transform;
    }

    private void Update()
    {
        HandlePickupInput();
        HandleSwitchInput();
    }

    // ---------- COLETAR ----------

    private void HandlePickupInput()
    {
        if (nearbyPickup != null && Input.GetKeyDown(pickupKey))
        {
            if (TryAddWeapon(nearbyPickup.WeaponPrefab, nearbyPickup.WeaponName))
            {
                Destroy(nearbyPickup.gameObject);
                nearbyPickup = null;
            }
        }
    }

    public bool TryAddWeapon(GameObject weaponPrefab, string weaponName)
    {
        if (weaponPrefab == null) return false;

        // Já tem essa arma?
        foreach (var slot in weapons)
        {
            if (slot.weaponName == weaponName)
            {
                Debug.Log("Você já possui: " + weaponName);
                return false;
            }
        }

        // Inventário cheio?
        if (weapons.Count >= maxWeapons)
        {
            Debug.Log("Inventário de armas cheio!");
            return false;
        }

        GameObject instance = Instantiate(weaponPrefab, weaponHolder);
        instance.transform.localPosition = Vector3.zero;
        instance.transform.localRotation = Quaternion.identity;
        instance.SetActive(false);

        weapons.Add(new WeaponSlot { weaponName = weaponName, instance = instance });

        // Equipa automaticamente se for a primeira, senão equipa a nova arma coletada
        EquipWeapon(weapons.Count - 1);
        Debug.Log("Arma coletada: " + weaponName);
        return true;
    }

    // ---------- TROCAR ----------

    private void HandleSwitchInput()
    {
        if (weapons.Count == 0) return;

        // Teclas 1 a 9
        for (int i = 0; i < weapons.Count && i < 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                EquipWeapon(i);
                return;
            }
        }

        // Roda do mouse
        float scroll = Input.mouseScrollDelta.y;
        if (scroll > 0f) EquipWeapon((currentIndex + 1) % weapons.Count);
        else if (scroll < 0f) EquipWeapon((currentIndex - 1 + weapons.Count) % weapons.Count);

        // Tecla Q = próxima arma
        if (Input.GetKeyDown(nextWeaponKey))
            EquipWeapon((currentIndex + 1) % weapons.Count);
    }

    public void EquipWeapon(int index)
    {
        if (index < 0 || index >= weapons.Count) return;
        if (index == currentIndex && CurrentWeapon != null && CurrentWeapon.activeSelf) return;

        if (CurrentWeapon != null) CurrentWeapon.SetActive(false);

        currentIndex = index;
        weapons[currentIndex].instance.SetActive(true);
        Debug.Log("Arma equipada: " + weapons[currentIndex].weaponName);
    }

    // ---------- DETECTAR PICKUP PRÓXIMO ----------

    private void OnTriggerEnter2D(Collider2D other)
    {
        WeaponPickup pickup = other.GetComponent<WeaponPickup>();
        if (pickup != null)
        {
            nearbyPickup = pickup;
            pickup.ShowPrompt(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        WeaponPickup pickup = other.GetComponent<WeaponPickup>();
        if (pickup != null && pickup == nearbyPickup)
        {
            pickup.ShowPrompt(false);
            nearbyPickup = null;
        }
    }
}