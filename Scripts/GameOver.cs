using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {

	public UILabel UILabel;
	public GameObject MoreBtn;
	public GameObject OverBtn;

	public void Awake()
	{
		//gameObject.SetActive (false);
		UIEventListener.Get (MoreBtn).MyOnClick = () => Application.LoadLevel (1);
		UIEventListener.Get (OverBtn).MyOnClick = DisConnection;
	}

	public void DisConnection()
	{
		Network.Disconnect ();
		Application.LoadLevel (0);
	}

	public void ShowGameResoult(string word)
	{
		UILabel.text = word;
		gameObject.SetActive (true);
	}
}
