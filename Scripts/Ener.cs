using UnityEngine;

namespace Assets.Scripts
{
    public class Ener : MonoBehaviour
    {
        public Card MyCard;

        public void ShowCard()
        {
            gameObject.GetComponent<UITexture>().mainTexture = MyCard.CardTexture;
        }
    }
}
