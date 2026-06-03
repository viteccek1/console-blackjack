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
        public CardType Suit { get; private set; } // Uloží typ karty
        public string Rank { get; private set; }   // Uloží text např. "J", "Q"
        public int Value { get; private set; }     // Uloží body karty např. 2, 10, 11

        // Konstruktor, který kartu vyrobí.
        // Zavolá se, když v balíčku napíšeme "new Card()" a dáme jí nějakou hodnotu.
        public Card(CardType suit, string rank, int value)
        {
            Suit = suit;   
            Rank = rank;   
            Value = value; 
        }
        
        // V konzoli se vypíše karta jako text.
        public override string ToString()
        {
            // Vrátí text např. "Hearts A" nebo "Spades 10"
            return $"{Suit} {Rank}";
        }
    }

    
    
    