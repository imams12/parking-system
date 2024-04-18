namespace ParkingLotSystem;

public class ParkingLot
{
    public int Capacity { get; }
    public Vehicle[] ParkedVehicles { get; private set;  }

    public ParkingLot(int capacity)
        {
            Capacity = capacity;
            ParkedVehicles = new Vehicle[capacity];
        }

        public bool IsFull()
        {
            return ParkedVehicles.Count(vehicle => vehicle != null) >= Capacity;
        }

        public int GetAvailableSlots()
        {
            return Capacity - ParkedVehicles.Length;
        }

        public void ParkVehicle(Vehicle vehicle)
        {
            if (!IsFull())
            {
                
                int emptySlot = Array.IndexOf(ParkedVehicles, null);
                ParkedVehicles[emptySlot] = vehicle;
                Console.WriteLine($"Vehicle with Registration No: {vehicle.PoliceNumber} parked successfully.");
            }
            else
            {
                Console.WriteLine("Sorry, parking lot is full.");
            }
        }

        public bool LeaveVehicle(int slotNumber)
        {
            if (slotNumber <= 0 || slotNumber > Capacity)
            {
                Console.WriteLine($"Slot number {slotNumber} is invalid.");
                return false;
            }

            if (ParkedVehicles[slotNumber - 1] == null)
            {
                Console.WriteLine($"Slot number {slotNumber} is already empty.");
                return false;
            }

            ParkedVehicles[slotNumber - 1] = null;
            Console.WriteLine($"Vehicle at slot {slotNumber} has left the parking lot.");
            return true;
        }

        public Vehicle[] GetParkedVehicles()
        {
            return ParkedVehicles.Where(v => v != null).ToArray();
        }

        public Vehicle[] GetVehiclesByType(VehicleType type)
        {
            return ParkedVehicles.Where(v => v != null && v.Type == type).ToArray();
        }

        public Vehicle[] GetVehiclesByColour(string colour)
        {
            return ParkedVehicles.Where(v => v != null && v.Colour.Equals(colour, StringComparison.OrdinalIgnoreCase)).ToArray();
        }

        public Vehicle GetVehicleByPoliceNumber(string policeNumber)
        {
            return ParkedVehicles.FirstOrDefault(v => v != null && v.PoliceNumber.Equals(policeNumber, StringComparison.OrdinalIgnoreCase));
        }

        public int GetSlotNumberByPoliceNumber(string policeNumber)
        {
            var vehicle = ParkedVehicles.FirstOrDefault(v => v != null && v.PoliceNumber.Equals(policeNumber, StringComparison.OrdinalIgnoreCase));
            return vehicle != null ? Array.IndexOf(ParkedVehicles, vehicle) + 1 : -1;
        }
}