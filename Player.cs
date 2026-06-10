// Player dědí z Participant, takže umí všechno s kartami a body
public class Player : Participant
{
    public int Chips { get; set; }
    
    public Player() : base("Player")
    {
    }
}