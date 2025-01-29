using UnityEngine;

public class BaseUI : MonoBehaviour
{
    public virtual void Show() {
        Debug.Log("BaseUI - Open " + gameObject.name);
        gameObject.SetActive(true);
    }

    public virtual void Hide() {
        Debug.Log("BaseUI - Close " + gameObject.name);
        gameObject.SetActive(false);
    }
}