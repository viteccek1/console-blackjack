#  Karetní hra Blackjack v C#

> Ahoj! Tohle je můj školní projekt – klasická karetní hra Blackjack (neboli Oko bere) napsaná v C# pro obyčejnou textovou konzoli. Chtěl jsem, aby to nebyla jen nudná klikačka, takže to umí i pokročilejší věci, co najdeš v opravdovém kasinu. 

---
Při vývoji hry jsem nejprve navrhl základní strukturu projektu a následně postupně přidával jednotlivé herní funkce. Umělou inteligenci (AI) jsem použil hlavně na začátku celého projektu, aby mi řekla, jak vůbec začít, jak si projekt rozvrhnout a jaké třídy (soubory) si mám připravit. Nechal jsem si poradit při funkci splitkde bylo potřeba rozdělit `Hands` na dvě samostatné ruky.

##  Co hra umí a jaké má funkce

- **Kompletní balíček 52 karet** – karty se před každou hrou samy náhodně promíchají.
- **Speciální herní akce:**
- **Double:** Hráč po prvních dvou kartách zdvojnásobí sázku, dostane jednu poslední kartu a tah automaticky končí.
- **Split:** Když dostaneš dvě stejné karty, můžeš si ruku rozdělit a hrát dvě hry naráz. Program s tím počítá a hlídá peníze pro obě ruce zvlášť.
- **Chytré počítání Es (A):** Eso má normálně 11 bodů, ale když bys měl kvůli tomu prohrát (mít přes 21), program ho sám přepne na 1 bod.
- **Hra nespadne při překlepu:** Když máš zadat sázku a omylem napíšeš písmena (třeba `sda`), hra se nevypne s chybou. Použil jsem `try-catch`, takže tě to jen upozorní a hraje se dál.
- **Barvy:** Konzole není jen černobílá – výhry jsou zelené, chyby červené a rozhraní modré, aby se to dobře hrálo.

---

##  Jak program funguje

### 1. Vytvoření a míchání balíčku
Po spuštění hry se vytvoří standardní balíček 52 karet. Před každou hrou jsou karty zamíchány pomocí algoritmu Fisher-Yates shuffle, který zajišťuje náhodné pořadí karet.


### 2. Hráč a dealer 
Hráč i dealer dědí ze společné abstraktní třídy Participant. Díky tomu sdílejí základní funkce, jako je práce s kartami nebo počítání bodů, ale každý má zároveň vlastní specifické chování.


### 3. Počítání bodů
Program průběžně počítá hodnotu karet v ruce. Speciálně je řešeno Eso, které má standardně hodnotu 11 bodů, ale pokud by hráč nebo dealer překročil 21 bodů, jeho hodnota se automaticky změní na 1 bod.

### 4. Herní akce
Během hry může hráč provádět běžné akce Blackjacku, jako je Hit nebo Stand. K dispozici jsou také pokročilejší akce Double a Split, které upravují průběh hry a práci se sázkami.

### 5. Vyhodnocení hry
Po dokončení tahu hráče odehraje svůj tah dealer podle pravidel kasina. Program následně porovná výsledky jednotlivých rukou, vyhodnotí výhru, prohru nebo remízu a upraví počet hráčových žetonů.

---

##   Jak jsou rozdělené soubory v projektu

| Soubor | Co to je | K čemu to tam je?                                                                |
| :--- | :--- |:---------------------------------------------------------------------------------|
| `Program.cs` | Spouštěč | Hlavní soubor, který jen zapne hru.                                              |
| `Card.cs` | Karta | Drží informace o jedné kartě (její barvu, hodnotu a text).                       |
| `Deck.cs` | Balíček | Vytvoří 52 karet a umí je pomocí algoritmu zamíchat.                             |
| `Participant.cs` | Společný základ | Abstraktní třída. Umí počítat body, držet karty a kreslit je textově do konzole. |
| `Player.cs` | Hráč | Dědí věci z Participanta a navíc mu hlídá peněženku s žetony (`Chips`).          |
| `Dealer.cs` | Dealer (Kasino) | Taky dědí z Participanta, ale hraje podle pravidel kasina (stojí na 17 bodech).  |
| `BlackjackGame.cs` | Hlavní mozek | Tady běží celá hra, sázky, tahání karet, Split a vyhodnocení, kdo vyhrál.        |

---

## 🎓 Použité zdroje
- **AI (LLM):** Pomohla mi na začátku vymyslet, jak projekt rozplánovat a jak poskládat logiku tříd.
- **Stack Overflow:** Odtud mám přesný matematický kód pro míchání karet (Fisher-Yates algoritmus).