using UnityEngine;

/// <summary>
/// Coloque na arma que está no CHÃO do cenário.
/// Precisa de um Collider2D com "Is Trigger" marcado.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class WeaponPickup : MonoBehaviour
{
    [Header("Arma")]
    [Tooltip("Nome único da arma (evita coletar duas vezes a mesma).")]
    [SerializeField] private string weaponName = "Canhão";
    [Tooltip("Prefab da arma que será colocado no jogador ao coletar.")]
    [SerializeField] private GameObject weaponPrefab;

    [Header("Opcional")]
    [Tooltip("Objeto de UI/sprite com o texto 'Pressione E' (começa desligado).")]
    [SerializeField] private GameObject promptObject;

    public string WeaponName => weaponName;
    public GameObject WeaponPrefab => weaponPrefab;

    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void Start()
    {
        ShowPrompt(false);
    }

    public void ShowPrompt(bool show)
    {
        if (promptObject != null) promptObject.SetActive(show);
    }
}