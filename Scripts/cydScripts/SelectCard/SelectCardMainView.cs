using UnityEngine;
using System.Collections;

public class SelectCardMainView : MonoBehaviour
{
    public GameObject SelectCharacter;
    public GameObject SelectFenShen;
    public GameObject SelectMainCard;
	// Use this for initialization
	void Start () {
	
	}
	//打开界面
    public void OpenView(GameObject go)
    {
        Debug.Log("000000"+go.name);
        SelectCharacter.SetActive(false);
        SelectFenShen.SetActive(false);
        SelectMainCard.SetActive(false);
        go.SetActive(true);
    }
	// Update is called once per frame
	void Update () {
	
	}
}
