using UnityEngine;
using UnityEngine.UI;

public class VidaUI : MonoBehaviour
{
    [Header("Configuración de vidas")]
    public RawImage[] vidas;       // Las imágenes de las vidas
    public Texture vidaLlena;      // Textura de vida llena
    public Texture vidaVacia;      // Textura de vida vacía

    /// <summary>
    /// Actualiza la UI de vidas según la cantidad actual
    /// </summary>
    public void ActualizarVidas(int cantidad)
    {
        // Limitar cantidad para que no sobrepase el arreglo
        cantidad = Mathf.Clamp(cantidad, 0, vidas.Length);

        for (int i = 0; i < vidas.Length; i++)
        {
            if (vidas[i] != null)
            {
                vidas[i].texture = (i < cantidad) ? vidaLlena : vidaVacia;
            }
        }
    }
}
