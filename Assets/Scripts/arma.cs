using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string weaponName = "Nova Arma";

    // Método chamado quando o jogador pressiona o botão de atacar/usar
    public virtual void Use()
    {
        Debug.Log($"Usando a arma: {weaponName}");
        // Coloque aqui a lógica específica (ex: atirar raio, tocar animação, instanciar projétil)
    }
}