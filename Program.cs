using System.Runtime.InteropServices.JavaScript;
using ParkingLotSystem;
using System.Text.RegularExpressions;

public class Program
{
    private static ParkingLot parkingLot;
    
    public static void Main(string[] args)
    {
        RunParkingSystem();
    }
    
    private static void RunParkingSystem()
    {
        Console.WriteLine("Welcome to Parking System!");
        Console.WriteLine("Type \"help\" for the list command");
        while (true)
        {
            Console.Write("Enter command: ");
            string command = Console.ReadLine();
            ProcessCommand(command);
        }
    }

    private static void ProcessCommand(string command)
    {
        string[] parts = command.Split(' ');
        string action = parts[0];

        switch (action.ToLower())
            {
                case "help":
                    string result = """
                                    ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                                                                   LIST COMMAND
                                    ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                                    1. create_parking_lot "capacity (ex: 6)"
                                    2. park "police_number color vehicle_type (ex: B-333-SSS Putih Mobil)"
                                    3. leave "slot_number (ex: 3)"
                                    4. status
                                    5. type_of_vehicles "vehicle_type (Motor/ Mobil)"
                                    6. registration_numbers_for_vehicles_with_ood_plate
                                    7. registration_numbers_for_vehicles_with_event_plate
                                    8. registration_numbers_for_vehicles_with_colour "color (ex: Putih)"
                                    9. slot_numbers_for_vehicles_with_colour "color (ex: Putih)"
                                    10. slot_number_for_registration_number "police_number (ex: B-3141-ZZZ)"
                                    11. exit
                                    """;
                    Console.WriteLine(result);
                    break;
                case "create_parking_lot":
                    
                        if(!Regex.IsMatch(parts[1], @"^-?\d+$"))
                        {
                            Console.WriteLine("Please input a integer");
                            break;
                        }
                        int capacity = int.Parse(parts[1]);
                        if (capacity <= 0)
                        {
                            Console.WriteLine("Please input a positive integer");
                            break;
                        }
                        parkingLot = new ParkingLot(capacity);
                        Console.WriteLine($"Created a parking lot with {capacity} slots");
                        break;
                    
                case "park":
                    if (parkingLot == null)
                    {
                        Console.WriteLine("Parking lot has not been created. Please create a parking lot first.");
                        break;
                    }
                    
                    string policeNumber = parts[1];
                    string colour = parts[2];
                    if (Enum.TryParse(parts[3], true, out VehicleType type))
                    {
                        if (parkingLot.IsFull())
                        {
                            Console.WriteLine("Sorry, parking lot is full");
                            break;
                        }
                        Vehicle vehicle = new Vehicle(policeNumber, colour, type);
                        parkingLot.ParkVehicle(vehicle);
                        int slotNumber = parkingLot.GetSlotNumberByPoliceNumber(policeNumber);
                        Console.WriteLine($"Allocated slot number: {slotNumber}");
                    }
                    else
                    {
                        Console.WriteLine("This parking lot only accepts cars and motorcycles");
                    }
                    break;

                case "leave":
                    if (parkingLot == null)
                    {
                        Console.WriteLine("Parking lot has not been created. Please create a parking lot first.");
                        break;
                    }

                    int leavingSlot = int.Parse(parts[1]);
                    bool success = parkingLot.LeaveVehicle(leavingSlot);
                    if (success)
                    {
                        Console.WriteLine($"Slot number {leavingSlot} is free");
                    }
                    else
                    {
                        Console.WriteLine($"Slot number {leavingSlot} not found");
                    }
                    break;

                case "status":
                    if (parkingLot == null)
                    {
                        Console.WriteLine("Parking lot has not been created. Please create a parking lot first.");
                        break;
                    }
                    PrintStatus();
                    break;

                case "type_of_vehicles":
                    if (parkingLot == null)
                    {
                        Console.WriteLine("Parking lot has not been created. Please create a parking lot first.");
                        break;
                    }

                    string vehicleType = parts[1];
                    if (Enum.TryParse(vehicleType, true, out VehicleType enumVehicleType))
                    {
                        int count = parkingLot.GetVehiclesByType(enumVehicleType).Length;
                        Console.WriteLine(count);
                    }
                    else
                    {
                        Console.WriteLine("Invalid vehicle type");
                    }
                    break;

                case "registration_numbers_for_vehicles_with_ood_plate":
                    if (parkingLot == null)
                    {
                        Console.WriteLine("Parking lot has not been created. Please create a parking lot first.");
                        break;
                    }

                    List<Vehicle> oddPlateVehicles = parkingLot.GetParkedVehicles().Where(v => IsOddPlate(v.PoliceNumber)).ToList();
                    PrintPoliceNumbers(oddPlateVehicles);
                    break;

                case "registration_numbers_for_vehicles_with_event_plate":
                    if (parkingLot == null)
                    {
                        Console.WriteLine("Parking lot has not been created. Please create a parking lot first.");
                        break;
                    }

                    List<Vehicle> evenPlateVehicles = parkingLot.GetParkedVehicles().Where(v => !IsOddPlate(v.PoliceNumber)).ToList();
                    PrintPoliceNumbers(evenPlateVehicles);
                    break;

                case "slot_numbers_for_vehicles_with_colour":
                    if (parkingLot == null)
                    {
                        Console.WriteLine("Parking lot has not been created. Please create a parking lot first.");
                        break;
                    }

                    string searchColourSlot = parts[1];
                    Vehicle[] colourSlotMatchedVehicles = parkingLot.GetVehiclesByColour(searchColourSlot);
                    PrintSlotNumbers(colourSlotMatchedVehicles);
                    break;

                case "registration_numbers_for_vehicles_with_colour":
                    if (parkingLot == null)
                    {
                        Console.WriteLine("Parking lot has not been created. Please create a parking lot first.");
                        break;
                    }

                    string searchColour = parts[1];
                    Vehicle[] colourMatchedVehicles = parkingLot.GetVehiclesByColour(searchColour);
                    PrintPoliceNumbers(colourMatchedVehicles.ToList());
                    break;


                case "slot_number_for_registration_number":
                    if (parkingLot == null)
                    {
                        Console.WriteLine("Parking lot has not been created. Please create a parking lot first.");
                        break;
                    }

                    string searchRegNumber = parts[1];
                    int slot = parkingLot.GetSlotNumberByPoliceNumber(searchRegNumber);
                    if (slot > 0)
                    {
                        Console.WriteLine(slot);
                    }
                    else
                    {
                        Console.WriteLine("Not found");
                    }
                    break;

            case "exit":
                Environment.Exit(0);
                break;

            default:
                Console.WriteLine("Invalid command");
                break;
        }
    }
    
    private static bool IsOddPlate(string plate)
    {
        string numericalPart = new string(plate.Where(char.IsDigit).ToArray());

        if (!string.IsNullOrEmpty(numericalPart))
        {
            // Parse the numerical part as an integer
            int number = int.Parse(numericalPart);
            return number % 2 != 0;
        }
        return false;
    }

    private static void PrintPoliceNumbers(List<Vehicle> vehicles)
    {
        string result = string.Join(", ", vehicles.Select(v => v.PoliceNumber));
        Console.WriteLine(result);
    }
    
    private static void PrintSlotNumbers(Vehicle[] vehicles)
    {
        string result = string.Join(", ", vehicles.Select(v => parkingLot.GetSlotNumberByPoliceNumber(v.PoliceNumber)));
        Console.WriteLine(result);
    }

    private static void PrintStatus()
    {
        Console.WriteLine("Slot No. Police Number Type   Colour");
        var parkedVehicles = parkingLot.GetParkedVehicles();
        foreach (var vehicle in parkedVehicles)
        {
            int slotNumber = parkingLot.GetSlotNumberByPoliceNumber(vehicle.PoliceNumber);
            Console.WriteLine($"{slotNumber,-9}{vehicle.PoliceNumber,-14}{vehicle.Type,-7}{vehicle.Colour}");    
            
        }
    }
}