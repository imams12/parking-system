namespace ParkingLotSystem;

public class Vehicle
{
    public string PoliceNumber{ get; set; }
    public string Colour{ get; set; }
    public VehicleType Type{ get; set; }

    public Vehicle(string policeNumber, string colour, VehicleType type)
    {
        PoliceNumber = policeNumber;
        Colour = colour;
        Type = type;
    }
    
}