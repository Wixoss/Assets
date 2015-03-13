using UnityEngine;
using System.Collections;

public class ShowCard : MonoBehaviour
{
    public UITexture CardTexture;
    public SelectCardMainView MainView;
    public GameObject MGameObject;
    public UILabel CardName;
    public UILabel CardDetial;
    public string CardId;

    public UIButton EnterBtn;
    public UIButton CancleBtn;

    void Awake()
    {
        MGameObject = this.gameObject;
        MainView = GameObject.Find("MainView").GetComponent<SelectCardMainView>();

        EventDelegate.Add(EnterBtn.onClick, OnClickEnterBtn);
        EventDelegate.Add(CancleBtn.onClick, OnClickCancleBtn);
    }
    //点击确定事件
    public void OnClickEnterBtn()
    {
        
    }
    //点击取消事件
    public void OnClickCancleBtn()
    {
        MGameObject.SetActive(false);
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
