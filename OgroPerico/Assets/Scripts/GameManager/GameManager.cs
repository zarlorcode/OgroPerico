using UnityEngine;

public class SceneMaster : MonoBehaviour
{
    [Header("UI Dialogue Popup")]
    public DialoguePopup dialoguePopup;

    [Header("Intro Dialogue Texts")]
    public DialogueLine[] introSentences;      // Escribe aquí las frases del prólogo

    void Start()
    {
        DialogueLine[] dialogueLines = new DialogueLine[]
                {
                    new DialogueLine { characterName = "Ogro Perico", sentence = "¡¡GRUAAAAARGH!! ¿Quién osa perturbar mis dominios? Mmm… ya veo quién eres, ratilla de oficina." },
                    new DialogueLine { characterName = "Ogro Perico", sentence = "¿Vienes por el Plan Dorado? ¡Qué osadía! No lo conseguirás… y tampoco saldrás de estas mazmorras con vida." },
                    new DialogueLine { characterName = "Ogro Perico", sentence = "Bienvenido a mi mazmorra. Aquí solo sobreviven los que saben obedecer… y tú has llegado demasiado lejos." }
                };

        introSentences = dialogueLines;
        if (dialoguePopup == null)
        {
            Debug.LogError("SceneMaster: dialoguePopup no está asignado en el inspector.");
            return;
        }
        if (introSentences != null && introSentences.Length > 0)
        {
                
                dialoguePopup.ShowDialogue(introSentences);
        }
        else
        {
            Debug.LogWarning("SceneMaster: introSentences está vacío. No se mostrará diálogo de inicio.");
        }
    }
}
