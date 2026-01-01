using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;

    [Header("Referencias")]
    public Transform contenedorIconos; // El objeto vacío con HorizontalLayoutGroup
    public GameObject prefabIcono;     // El prefab de la imagen (el que creamos antes)

    [Header("Biblioteca de Iconos")]
    // Esto nos permite asignar sprites a tipos desde el Inspector
    public List<ItemData> biblioteca; 

    [System.Serializable]
    public struct ItemData
    {
        public OfficeCollectible.CollectibleType tipo;
        public Sprite icono;
    }

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        // AL INICIAR: Reconstruir la UI basada en lo que guardamos en DatosDeJuego
        foreach (var tipoGuardado in DatosDeJuego.ConsumiblesRecogidos)
        {
            Sprite spriteCorrespondiente = BuscarSprite(tipoGuardado);
            CrearIconoVisual(spriteCorrespondiente);
        }
    }

    // Llamamos a esto cuando recogemos algo NUEVO
    public void RegistrarNuevoConsumible(OfficeCollectible.CollectibleType tipo)
    {
        // 1. Lo guardamos en la memoria estática (para el siguiente nivel)
        DatosDeJuego.ConsumiblesRecogidos.Add(tipo);

        // 2. Lo mostramos visualmente ahora mismo
        Sprite sprite = BuscarSprite(tipo);
        CrearIconoVisual(sprite);
    }

    // Método auxiliar para crear el objeto UI
    private void CrearIconoVisual(Sprite icono)
    {
        if (icono == null) return;

        GameObject nuevoObj = Instantiate(prefabIcono, contenedorIconos);
        nuevoObj.transform.localScale = Vector3.one; 
        nuevoObj.GetComponent<Image>().sprite = icono;
    }

    // Busca en la lista que configuramos en el Inspector
    private Sprite BuscarSprite(OfficeCollectible.CollectibleType tipo)
    {
        foreach (var item in biblioteca)
        {
            if (item.tipo == tipo) return item.icono;
        }
        return null; // Si no encontramos imagen
    }
}
