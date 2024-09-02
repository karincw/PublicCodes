namespace CW
{
    public class CardManager : MonoSingleton<CardManager>
    {
        StandCard _standCard;
        CardInven _cardInven;

        private void Awake()
        {
            _standCard = FindObjectOfType<StandCard>();
            _cardInven = FindObjectOfType<CardInven>();
        }

        public void StandCard()
        {
            _standCard.Stand();
        }
        public void UpdateCard()
        {
            _standCard.UpdateCard();
        }
        public void AddCard(CardSO card, int count = 1)
        {
            _cardInven.AddCard(card, count);
        }

    }

}