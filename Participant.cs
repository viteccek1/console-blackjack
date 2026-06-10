using System;
using System.Collections.Generic;

public abstract class Participant
{
    public string Name { get; private set; }
    // Seznam všech rukou, které hráč momentálně hraje (při splitu jich bude víc)
    public List<List<Card>> Hands { get; private set; }
    // Sázka pro každou jednotlivou ruku
    public List<int> HandBets { get; private set; }

    public Participant(string name)
    {
        Name = name;
        Hands = new List<List<Card>> { new List<Card>() };
        HandBets = new List<int> { 0 };
    }
    
    public void AddCard(Card card, int handIndex = 0)
    {
        Hands[handIndex].Add(card);
    }

    // Výpočet skóre pro konkrétní ruku podle indexu
    public int CalculateScore(int handIndex = 0)
    {
        int score = 0;
        int aceCount = 0;
        
        foreach (Card card in Hands[handIndex])
        {
            score += card.Value;
            if (card.Rank == "A") aceCount++;
        }
        
        while (score > 21 && aceCount > 0)
        {
            score -= 10; 
            aceCount--; 
        }

        return score;
    }
    
    private string ReturnSymbol(Card card)
    {
        if (card.Suit == CardType.Hearts) return "♥";
        if (card.Suit == CardType.Diamonds) return "♦";
        if (card.Suit == CardType.Clubs) return "♣";
        return "♠";
    }

    // Vykreslení konkrétní ruky podle indexu - JEDNODUŠE BEZ CUSTOMLABEL
    public void ShowHand(int handIndex = 0)
    {
        List<Card> currentHand = Hands[handIndex];

        // 1. Žlutě vypíšeme jméno (Player / Dealer)
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(Name);
        Console.ResetColor();

        // 2. JEDNODUCHÁ PODMÍNKA: Pokud má hráč více rukou (po splitu), připíšeme číslo ruky
        if (Hands.Count > 1)
        {
            Console.Write(" (Ruka " + (handIndex + 1) + ")");
        }

        // 3. Vypíšeme zbytek textu a body
        Console.Write(" má v ruce. ");
        Console.Write("Celkem bodů: ");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(CalculateScore(handIndex));
        Console.ResetColor();

        if (currentHand.Count == 0) return;

        // RÁDEK 1
        for (int i = 0; i < currentHand.Count; i++) 
        {
            if (currentHand[i].Suit == CardType.Hearts || currentHand[i].Suit == CardType.Diamonds) Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("┌───────┐  ");
            Console.ResetColor();
        }
        Console.WriteLine();

        // RÁDEK 2
        for (int i = 0; i < currentHand.Count; i++)
        {
            if (currentHand[i].Suit == CardType.Hearts || currentHand[i].Suit == CardType.Diamonds) Console.ForegroundColor = ConsoleColor.Red;
            string rank = currentHand[i].Rank;
            if (rank == "10") Console.Write("│ 10    │  ");
            else Console.Write($"│ {rank}     │  ");
            Console.ResetColor();
        }
        Console.WriteLine();

        // RÁDEK 3
        for (int i = 0; i < currentHand.Count; i++)
        {
            if (currentHand[i].Suit == CardType.Hearts || currentHand[i].Suit == CardType.Diamonds) 
                Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"│   {ReturnSymbol(currentHand[i])}   │  ");
            Console.ResetColor();
        }
        Console.WriteLine();

        // RÁDEK 4
        for (int i = 0; i < currentHand.Count; i++)
        {
            if (currentHand[i].Suit == CardType.Hearts || currentHand[i].Suit == CardType.Diamonds) Console.ForegroundColor = ConsoleColor.Red;
            string rank = currentHand[i].Rank;
            if (rank == "10") Console.Write("│    10 │  ");
            else Console.Write($"│     {rank} │  ");
            Console.ResetColor();
        }
        Console.WriteLine();

        // RÁDEK 5
        for (int i = 0; i < currentHand.Count; i++) 
        {
            if (currentHand[i].Suit == CardType.Hearts || currentHand[i].Suit == CardType.Diamonds) Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("└───────┘  ");
            Console.ResetColor();
        }
        Console.WriteLine();
        Console.WriteLine();
    }

    public void ShowHandHidden()
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write(Name);
        Console.ResetColor();
        Console.Write(" má v ruce. ");
        
        Console.Write("Celkem bodů: ");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(Hands[0][0].Value);
        Console.ResetColor();

        if (Hands[0].Count == 0) return;

        bool isFirstRed = (Hands[0][0].Suit == CardType.Hearts || Hands[0][0].Suit == CardType.Diamonds);

        // RÁDEK 1
        if (isFirstRed) Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("┌───────┐  ");
        Console.ResetColor();
        Console.Write("┌───────┐  ");
        Console.WriteLine();
        
        // RÁDEK 2
        if (isFirstRed) Console.ForegroundColor = ConsoleColor.Red;
        string rank = Hands[0][0].Rank;
        if (rank == "10") Console.Write("│ 10    │  ");
        else Console.Write($"│ {rank}     │  ");
        Console.ResetColor();
        Console.WriteLine("│ ?     │  ");

        // RÁDEK 3
        if (isFirstRed) Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"│   {ReturnSymbol(Hands[0][0])}   │  ");
        Console.ResetColor();
        Console.WriteLine("│   ?   │  ");

        // RÁDEK 4
        if (isFirstRed) Console.ForegroundColor = ConsoleColor.Red;
        if (rank == "10") Console.Write("│    10 │  ");
        else Console.Write($"│     {rank} │  ");
        Console.ResetColor();
        Console.WriteLine("│     ? │  ");

        // RÁDEK 5
        if (isFirstRed) Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("└───────┘  ");
        Console.ResetColor();
        Console.WriteLine("└───────┘  ");
        
        Console.WriteLine();
    }
    
    public void ClearHand()
    {
        Hands.Clear();
        Hands.Add(new List<Card>());
        HandBets.Clear();
        HandBets.Add(0);
    }
}