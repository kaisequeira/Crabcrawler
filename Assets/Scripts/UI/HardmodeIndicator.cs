using UnityEngine;

public class HardmodeIndicator : MonoBehaviour
{
    /// <summary>
    /// Start is called before the first frame update. Is used here to determine if the indicator should be displayed.
    /// </summary>
    void Start()
    {
        if (!SaveSystem.LoadHardMode())
           gameObject.SetActive(false);
    }
}
