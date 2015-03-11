using UnityEngine;

namespace Assets.Scripts
{
    public class ShowCardDetail : MonoBehaviour
    {
        public UITexture UiTexture;
        public UILabel UiLabel;
        public GameObject CardBack;
        public Camera Camera;

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
                            UiLabel.text = hand.MyCard.CardDetail;
                        }
                    }
                }
            }
        }
    }
}
