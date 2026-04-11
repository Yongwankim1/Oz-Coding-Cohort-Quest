using UnityEngine;
using UnityEngine.UI;

public class XBtn : MonoBehaviour
{
    private Button myBtn;
    [SerializeField] GameObject[] gameObjects;
    private void OnEnable()
    {
        if(myBtn == null)
            myBtn = GetComponent<Button>();
        myBtn.onClick.AddListener(() =>
        {
            if (gameObjects.Length <= 0) return;

            for(int i = 0; i < gameObjects.Length; i++)
            {
                gameObjects[i].SetActive(false);
            }

        });
    }
    private void OnDisable()
    {
        myBtn.onClick.RemoveAllListeners();
    }
}
