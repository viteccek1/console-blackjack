// Seznam barev, kdybych použil string tak se uživatel může přepsat ale enum povolí jenom ty níže vypsané.
    public enum CardType
    {
        Hearts,   
        Diamonds, 
        Clubs,   
        Spades   
    }

    // Takto bude vypadat každá karta ve hře.
    public class Card
    {
        public CardType Suit { get; private set; } 
        public string Rank { get; private set; }   
        public int Value { get; private set; }     

        
        
        public Card(CardType suit, string rank, int value)
        {
            Suit = suit;   
            Rank = rank;   
            Value = value; 
        }
        
        
        public override string ToString()
        {
            
            return $"{Suit} {Rank}";
        }
    }

    
    
    