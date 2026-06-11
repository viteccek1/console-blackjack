public class BlackjackGame
{
    private Deck deck;
    private Player player;
    private Dealer dealer;

    public void Start()
    {
        Console.Clear();
        // Zajišťuje správné zobrazení UTF-8 znaků (symboly srdce, piky...) v konzoli Windows.
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=== VÍTEJTE VE HŘE BLACKJACK ===");
        Console.ResetColor();
        
        deck = new Deck();
        player = new Player();
        dealer = new Dealer();

        player.Chips = 1500;

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Vstupuješ do hry s částkou {player.Chips} tokenů.");
        Console.ResetColor();

        bool playAgain = true;

        while (playAgain)
        {
            if (player.Chips <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n Bankrot! Došly ti všechny tokeny.");
                Console.ResetColor();
                break;
            }

            player.ClearHand();
            dealer.ClearHand();
            
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"\n--- NOVÁ HRA ZAČÍNÁ (Tvůj stav: {player.Chips} tokenů) ---");
            Console.ResetColor();

            int initialBet = 0;
            while (true)
            {
                Console.Write($"Kolik tokenů chceš vsadit do této hry? (1 - {player.Chips}): ");
                string input = Console.ReadLine();
                
                try
                {
                    initialBet = int.Parse(input);
                    
                    if (initialBet > 0 && initialBet <= player.Chips)
                    {
                        player.HandBets[0] = initialBet;
                        break; 
                    }
                }
                catch (FormatException)
                {
                    // Sem program skočí jen tehdy, když int.Parse selže (uživatel nezadal číslo)
                    // Necháme blok prázdný, abychom chybu ignorovali a hra nespadla
                }
                
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Neplatná sázka! Musíš zadat číslo od 1 do {player.Chips}.");
                Console.ResetColor();
            }
            Console.Clear();

            // Animace rozdávání karet na začátku (Pauza 1 sekunda)
            Console.WriteLine("Dealer míchá a rozdává karty...");
            System.Threading.Thread.Sleep(1000);
            Console.Clear();

            // 1. karta pro hráče
            player.AddCard(deck.DrawCard(), 0);
            player.ShowHand(0);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Dealer má v ruce. Celkem bodů: 0\n[Zatím nedostal kartu]");
            Console.ResetColor();
            System.Threading.Thread.Sleep(1000);
            Console.Clear();

            // 1. karta pro dealera
            dealer.AddCard(deck.DrawCard(), 0);
            player.ShowHand(0);
            dealer.ShowHandHidden();
            System.Threading.Thread.Sleep(1000);
            Console.Clear();

            // 2. karta pro hráče
            player.AddCard(deck.DrawCard(), 0);
            player.ShowHand(0);
            dealer.ShowHandHidden();
            System.Threading.Thread.Sleep(1000);
            Console.Clear();

            // 2. karta pro dealera (druhá, skrytá)
            dealer.AddCard(deck.DrawCard(), 0);
            player.ShowHand(0);
            dealer.ShowHandHidden();
            // ------------------------------------------

            // Kolo hráče (Procházíme postupně všechny jeho ruce)
            for (int i = 0; i < player.Hands.Count; i++)
            {
                bool handTurn = true;
                bool isAceSplit = false;

                while (handTurn && player.CalculateScore(i) < 21)
                {
                    Console.Clear();
                    
                    // Vypíšeme všechny dosavadní ruce hráče, abychom viděli kontext (při Splitu)
                    for (int k = 0; k < player.Hands.Count; k++)
                    {
                        if (player.Hands.Count > 1 && k == i)
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine($"===> HAND NUMBER {k + 1} <===");
                            Console.ResetColor();
                        }
                        player.ShowHand(k);
                    }
                    
                    dealer.ShowHandHidden();

                    int currentScore = player.CalculateScore(i);
                    // Pouze pokud má hráč 2 karty, jejich součet je 9, 10 nebo 11 a má dost žetonů
                    bool canDouble = player.Hands[i].Count == 2 && (currentScore == 9 || currentScore == 10 || currentScore == 11) && player.Chips >= (GetTotalActiveBets() + player.HandBets[i]);
                    // První dvě karty stejné hodnoty a dostatek žetonů
                    bool canSplit = player.Hands[i].Count == 2 && player.Hands[i][0].Rank == player.Hands[i][1].Rank && player.Chips >= (GetTotalActiveBets() + player.HandBets[i]);

                    Console.Write($"\n[Hraje Ruka {i + 1}] Co chceš udělat? (h = hit / s = stand");
                    if (canDouble) Console.Write(" / d = double");
                    if (canSplit) Console.Write(" / p = split");
                    Console.Write("): ");

                    string choice = Console.ReadLine().ToLower().Trim();

                    // PŘIDÁNA PODPORA PRO CELÁ SLOVA POMOCÍ "||" (NEBO)
                    if (choice == "h" || choice == "hit")
                    {
                        Card drawn = deck.DrawCard();
                        player.AddCard(drawn, i);
                    }
                    else if (choice == "s" || choice == "stand")
                    {
                        handTurn = false;
                    }
                    else if ((choice == "d" || choice == "double") && canDouble)
                    {
                        player.HandBets[i] *= 2;
                        Card drawn = deck.DrawCard();
                        player.AddCard(drawn, i);
                        handTurn = false; // Po double ruka ihned končí
                    }
                    else if ((choice == "p" || choice == "split") && canSplit)
                    {
                        // Vytvoříme novou ruku a přesuneme do ní druhou kartu
                        List<Card> newHand = new List<Card>
                        {
                            player.Hands[i][1]
                        };
                        player.Hands.Add(newHand);
                        player.HandBets.Add(player.HandBets[i]); // Nová ruka přebírá stejnou sázku
                        
                        player.Hands[i].RemoveAt(1); // Odebereme kartu z původní ruky

                        // Kontrola, zda splitujeme Esa
                        if (player.Hands[i][0].Rank == "A") isAceSplit = true;

                        // Rozdáme do obou rozdělených rukou hned novou kartu
                        player.AddCard(deck.DrawCard(), i);
                        player.AddCard(deck.DrawCard(), player.Hands.Count - 1);

                        // Pokud splitujeme Esa, dostaneme pouze jednu kartu na každou ruku a tah končí
                        if (isAceSplit)
                        {
                            handTurn = false;
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Neplatná volba, vyber z možností výše.");
                        Console.ResetColor();
                        System.Threading.Thread.Sleep(1200);
                    }
                }
            }
            
            Console.Clear();
            for (int k = 0; k < player.Hands.Count; k++) player.ShowHand(k);
            dealer.ShowHandHidden();

            // Kontrola, jestli hráč nepřetáhl úplně všechny své ruce
            bool allHandsBusted = true;
            for (int i = 0; i < player.Hands.Count; i++)
            {
                if (player.CalculateScore(i) <= 21) allHandsBusted = false;
            }

            if (allHandsBusted)
            {
                System.Threading.Thread.Sleep(1000);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n Všechny tvé ruce přetáhly 21 bodů! Přicházíš o sázky.");
                Console.ResetColor();
                foreach (int betAmount in player.HandBets) player.Chips -= betAmount;
            }
            else
            {
                // Kolo dealera (Vypisuje všechny ruce hráče) ---
                Console.Clear();
                Console.WriteLine("\n--- KOLO DEALERA ---");
                for (int k = 0; k < player.Hands.Count; k++) player.ShowHand(k);
                Console.WriteLine("Dealer odkrývá svou skrytou kartu...");
                System.Threading.Thread.Sleep(1500);
                
                Console.Clear();
                Console.WriteLine("\n--- KOLO DEALERA ---");
                for (int k = 0; k < player.Hands.Count; k++) player.ShowHand(k); 
                dealer.ShowHand(0); // Dealer odhalí skrytou kartu

                while (dealer.CalculateScore(0) < 17)
                {
                    System.Threading.Thread.Sleep(1500); 
                    Card drawn = deck.DrawCard();
                    dealer.AddCard(drawn, 0);
                    
                    Console.Clear();
                    Console.WriteLine("\n--- KOLO DEALERA ---");
                    for (int k = 0; k < player.Hands.Count; k++) player.ShowHand(k); 
                    dealer.ShowHand(0);
                    Console.WriteLine($"Dealer si lízl: {drawn}");
                }

                int dealerScore = dealer.CalculateScore(0);
                System.Threading.Thread.Sleep(1200);

                // Vyhodnocení výsledků
                Console.WriteLine("\n--- VÝSLEDEK ---");

                for (int i = 0; i < player.Hands.Count; i++)
                {
                    EvaluateHand($"Ruka {i + 1}", i, dealerScore);
                }
            }

            // Kontrola sázky na druhé kolo
            if (player.Chips > 0)
            {
                while (true)
                {
                    Console.Write("\nChceš hrát další kolo? (a = ano / n = ne): ");
                    string replayChoice = Console.ReadLine().ToLower().Trim();
                    Console.Clear();

                    if (replayChoice == "a" || replayChoice == "ano") break; 
                    else if (replayChoice == "n" || replayChoice == "ne")
                    {
                        playAgain = false;
                        break; 
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Neplatná volba! Zadej prosím 'a' pro ano, nebo 'n' pro ne.");
                        Console.ResetColor();
                    }
                }
            }
        }

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\nHru končíš s výsledným stavem: {player.Chips} tokenů.");
        Console.ResetColor();
        Console.WriteLine("Díky za hru! Stiskni libovolnou klávesu pro ukončení...");
        Console.ReadKey();
    }
    // Pomocná metoda, která sečte hodnoty sázek ze všech rukou.
    private int GetTotalActiveBets()
    {
        int total = 0;
        foreach (int bet in player.HandBets) total += bet;
        return total;
    }

    private void EvaluateHand(string handName, int handIndex, int dealerScore)
    {
        int playerScore = player.CalculateScore(handIndex);
        int currentBet = player.HandBets[handIndex];
        int cardCount = player.Hands[handIndex].Count;

        if (playerScore > 21)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($" {handName} přetáhla (Bust). Prohráváš {currentBet} tokenů.");
            Console.ResetColor();
            player.Chips -= currentBet;
            return;
        }

        bool playerHasBlackjack = playerScore == 21 && cardCount == 2 && player.Hands.Count == 1;
        bool dealerHasBlackjack = dealerScore == 21 && dealer.Hands[0].Count == 2;

        if (playerHasBlackjack && dealerHasBlackjack)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($" {handName}: Oboustranný Blackjack! Remíza, sázka se vrací.");
            Console.ResetColor();
        }
        else if (playerHasBlackjack)
        {
            int winAmount = (currentBet * 5) / 2; 
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($" {handName}: Máš Blackjack! Vyhráváš {winAmount} tokenů!");
            Console.ResetColor();
            player.Chips += winAmount;
        }
        else if (dealerScore > 21 || playerScore > dealerScore)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($" {handName} vyhrává! Získáváš {currentBet} tokenů.");
            Console.ResetColor();
            player.Chips += currentBet;
        }
        else if (dealerScore > playerScore)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($" Dealer poráží {handName}. Prohráváš {currentBet} tokenů.");
            Console.ResetColor();
            player.Chips -= currentBet;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($" {handName}: Remíza s dealerem! Sázka se vrací.");
            Console.ResetColor();
        }
    }
}