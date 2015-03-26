using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Assets.Scripts;
using UnityEngine;
using System.Collections;

public class SelectCardMainView : MonoBehaviour
{
    public GameObject SelectCharacter;
    public GameObject SelectFenShen;
    public GameObject SelectMainCard;
    public ShowCard ShowCard;
     

	// Use this for initialization
	void Start ()
	{
	    setcard();
	}
    //读取xml的卡牌并且赋值
    public void setcard()
    {       
//        var root = CreateCardByXml.LoadFromXml();
//        List<XElement> L_xElements = root.Elements("Lrig").ToList();
//        Debug.Log("-------------" + L_xElements.LongCount());
//        for (int i = 0; i < L_xElements.LongCount(); i++)
//        {
//            var xElement = L_xElements[i].Element("CardId");
//            if (xElement != null)
//                DataSource.LrigCards.Add(xElement.Value);
//                Debug.Log("=======" + DataSource.LrigCards[i]);
//        }
//        List<XElement> A_xElements = root.Elements("Art").ToList();
//        for (int i = 0; i < A_xElements.LongCount(); i++)
//        {
//            var xElement = A_xElements[i].Element("CardId");
//            if (xElement != null)
//                DataSource.ArtCards.Add(xElement.Value); 
//        }
//        List<XElement> Si_xElements = root.Elements("Signi").ToList();
//        for (int i = 0; i < Si_xElements.LongCount(); i++)
//        {
//            var xElement = Si_xElements[i].Element("CardId");
//            if (xElement != null)
//                DataSource.SigniCards.Add(xElement.Value);
//        }
//        List<XElement> Sp_xElements = root.Elements("Spell").ToList();
//        for (int i = 0; i < Sp_xElements.LongCount(); i++)
//        {
//            var xElement = Sp_xElements[i].Element("CardId");
//            if (xElement != null)
//                DataSource.SpellCards.Add(xElement.Value);
//        }
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
    //出现卡牌详情界面
    public void OpenShowCard(string ID,string CardName,string Detial)
    {
        ShowCard.gameObject.SetActive(true);
        ShowCard.CardId = ID;
        ShowCard.CardName.text = CardName;
        ShowCard.CardDetial.text = Detial;
        ShowCard.CardTexture.mainTexture = Resources.Load(ID) as Texture2D;
    }
	// Update is called once per frame
	void Update () {
	
	}
}
