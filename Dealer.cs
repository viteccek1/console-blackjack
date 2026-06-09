// Dealer taky dědí z Participant, ale nemá "peněženku" (kasino má bezednou kapsu)
public class Dealer : Participant
{
    public Dealer() : base("Dealer")
    {
    }
}