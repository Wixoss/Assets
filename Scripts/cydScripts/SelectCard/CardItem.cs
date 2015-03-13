using System;
using System.Xml.Linq;
using UnityEngine;
using System.Collections;

public class CardItem : MonoBehaviour
{
    private SelectCardMainView mainView;
    public UIButton CardBtn;
    public string _CardId;
    public string _Cardname;
    public string _Detial;

    void Awake()
    {
        mainView = GameObject.Find("MainView").GetComponent<SelectCardMainView>();
        CardBtn = this.GetComponent<UIButton>();
        EventDelegate.Add(CardBtn.onClick, OnClickCard);
    }

    void OnClickCard()
    {
        mainView.OpenShowCard(_CardId,_Cardname,_Detial);
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
