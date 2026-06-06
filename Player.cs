public class Player
{
    
    public string Name { get; private set; }
    public List<Card> Hand { get; private set; }
    
    public Player(string name)
    {
        Name = name;
        Hand = new List<Card>();
    }
    
    public void AddCard(Card card)
    {
        Hand.Add(card);
    }

    
    public int CalculateScore()
    {
        int score = 0;
        int aceCount = 0;
        
        foreach (Card card in Hand)
        {
            score += card.Value;
            if (card.Rank == "A")
            {
                aceCount++;
            }
        }
        
        // Pokud hráč kvůli Esu překročil 21, snížíme hodnotu Esa z 11 na 1 bod
        while (score > 21 && aceCount > 0)
        {
            score -= 10; 
            aceCount--; 
        }

        return score;
    }
    
    public void ShowHand()
    {
        Console.Write($"{Name} má v ruce: ");
        foreach (Card card in Hand)
        {
            Console.Write($"{card} ");  
        }
        Console.WriteLine($"Celkem bodů: {CalculateScore()}");
    }
    
    public void ClearHand()
    {
        Hand.Clear();
    }
}