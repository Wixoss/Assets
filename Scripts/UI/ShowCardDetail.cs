using UnityEngine;

namespace Assets.Scripts
{
    public class ShowCardDetail : MonoBehaviour
    {
        public UITexture UiTexture;
        public UILabel UiLabel;
        public UILabel AtkLabel;
        public GameObject CardBack;
        public Camera Camera;

        public static UITexture UiTexture2;
        public static UILabel UiLabel2;
        public static UILabel AtkLabel2;
        public static GameObject CardBack2;

        private void Awake()
        {
            UiTexture2 = UiTexture;
            UiLabel2 = UiLabel;
            AtkLabel2 = AtkLabel;
            CardBack2 = CardBack;
        }


        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo))
                {
                    if (hitInfo.collider)
                    {
                        var hand = hitInfo.collider.GetComponent<Hands>();
                        if (hand != null)
                        {
                            CardBack.SetActive(false);
                            UiTexture.mainTexture = hand.MyCard.CardTexture;
                            UiLabel.text = hand.MyCard.Buff + hand.MyCard.CardDetail;
                            AtkLabel.text = hand.MyCard.Atk.ToString();
                        }
                    }
                }
            }
        }

        public static void ShowCardDetailByCard(Card card)
        {
            UiTexture2.mainTexture = card.CardTexture;
            UiLabel2.text = card.Buff + card.CardDetail;
            AtkLabel2.text = card.Atk.ToString();
        }

    }
}
