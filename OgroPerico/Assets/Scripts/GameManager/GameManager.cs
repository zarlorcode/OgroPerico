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
                    new DialogueLine { characterName = "Ogro Perico", sentence = "¡GRAAAARRR! Así que finalmente alguien ha entrado… veo quién eres."},
                    new DialogueLine { characterName = "Ogro Perico", sentence = "¡No conseguirás el Plan Dorado… ni saldrás de mis mazmorras con vida!" },
                    new DialogueLine { characterName = "Ogro Perico", sentence = "Bienvenido a la mazmorra" }
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
