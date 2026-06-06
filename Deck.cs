    public class Deck
    {
        private List<Card> cards;
        private Random random;
        
        public Deck()
        {
            cards = new List<Card>();
            random = new Random();
            GenerateDeck();   
            Shuffle();        
        }

        // Vytvořím balíček a naplní List 52 kartami
        private void GenerateDeck()
        {
            // Vezmeme enum CardType a uděláme z něj pole všech 4 barev
            CardType[] type = { CardType.Hearts, CardType.Diamonds, CardType.Clubs, CardType.Spades };
            
            string[] ranks = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
            
            // Spojím barvy a čísla = 4 x 13 a to je 52 kominací.
            foreach (CardType suit in type)
            {
                foreach (string rank in ranks)
                {
                    int value = DetermineCardValue(rank); 
                    cards.Add(new Card(suit, rank, value));
                }
            }
        }
        private int DetermineCardValue(string rank)
        {
            if (rank == "J" || rank == "Q" || rank == "K")
            {
                return 10;
            }
            if (rank == "A")
            {
                return 11;
            }
            
            return int.Parse(rank); 
        }

        // Míchání karek pomocí FIsher-Yates algoritmu 
        public void Shuffle()
        {
            for (int i = cards.Count - 1; i > 0; i--)
            {
                // Vybereme náhodný index od 0 do i
                int j = random.Next(i + 1);
                
                // Vezmu kartu z konec balíčku a dám ji do temp
                Card temporary = cards[i];
                // Teď je pozice i volná, takže tam můžu přesunout náhodnou kartu z pozice j.
                cards[i] = cards[j];
                // A na uvolněnou pozici j vrátím kartu, kterou jsem si na začátku dal do temp.    
                cards[j] = temporary;
            }
        }
        
        public Card DrawCard()
        {
            if (cards.Count == 0)
            {
                GenerateDeck();
                Shuffle();
            }
            
            Card drawnCard = cards[cards.Count - 1];
            cards.Remove(drawnCard);
            return drawnCard;
        }
    }
