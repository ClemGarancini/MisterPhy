using UnityEngine;
using System.Collections;

public class SwitchAction : MonoBehaviour
{
    public GameObject obj;
    public float duration = 2.0f;
    // Start is called before the first frame update
    public void On()
    {
        StartCoroutine(LowerDoor());
        gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
    }

    private IEnumerator LowerDoor()
    {

        float elapsedTime = 0f;
        Vector3 originalScale = obj.transform.localScale;

        while (elapsedTime < duration)
        {
            float newScaleY = Mathf.Lerp(originalScale.y, 0f, elapsedTime / duration);
            obj.transform.localScale = new Vector3(originalScale.x, newScaleY, originalScale.z);

            elapsedTime += Time.deltaTime;
            yield return null; // attendre le prochain frame
        }

        // Assurez-vous que la hauteur est exactement 0 à la fin
        obj.transform.localScale = new Vector3(originalScale.x, 0f, originalScale.z);
        obj.SetActive(false); // désactiver l'objet une fois l'animation terminée
    }
}
