using GarageConsoleApp.Models;

namespace GarageConsoleApp.Tests;

public class GarageTests
{
    // A car takes 3 units, which is exactly one parking space
    private static Car NewCar(string registrationNumber = "ABC123", string color = "Black") =>
        new(registrationNumber, "Volvo V70", color, FuelType.Diesel);

    // A motorcycle takes 1 unit, a third of a parking space
    private static Motorcycle NewMotorcycle(string registrationNumber = "CDE12F", string color = "Black") =>
        new(registrationNumber, "Honda CB500F", color, TransmissionType.Manual);

    [Fact]
    public void Park_FreeSpace_ReturnsParked()
    {
        var garage = new Garage<Vehicle>("Test", capacity: 1);
        Car car = NewCar();

        ParkResult result = garage.Park(car);

        Assert.Equal(ParkResult.Parked, result);
    }

    [Fact]
    public void Park_GarageIsFull_ReturnsNotEnoughSpace()
    {
        var garage = new Garage<Vehicle>("Test", capacity: 1);
        garage.Park(NewCar("ABC123"));

        ParkResult result = garage.Park(NewCar("ABC124"));

        Assert.Equal(ParkResult.NotEnoughSpace, result);
    }

    [Fact]
    public void Park_SameRegistrationNumberTwice_ReturnsAlreadyParked()
    {
        var garage = new Garage<Vehicle>("Test", capacity: 2);
        garage.Park(NewCar("ABC123"));

        ParkResult result = garage.Park(NewCar("abc123"));

        Assert.Equal(ParkResult.AlreadyParked, result);
    }

    [Fact]
    public void Park_ThreeMotorcycles_FillsOneParkingSpace()
    {
        var garage = new Garage<Vehicle>("Test", capacity: 1);
        garage.Park(NewMotorcycle("AAA111"));
        garage.Park(NewMotorcycle("BBB222"));
        garage.Park(NewMotorcycle("CCC333"));

        ParkResult result = garage.Park(NewMotorcycle("DDD444"));

        Assert.Equal(ParkResult.NotEnoughSpace, result);
        Assert.Equal(0, garage.FreeUnits);
    }

    [Fact]
    public void Remove_ParkedVehicle_ReturnsTrueAndFreesSpace()
    {
        var garage = new Garage<Vehicle>("Test", capacity: 1);
        garage.Park(NewCar("ABC123"));

        bool removed = garage.Remove("ABC123");

        Assert.True(removed);
        Assert.Equal(garage.TotalUnits, garage.FreeUnits);
    }

    [Fact]
    public void Remove_UnknownRegistrationNumber_ReturnsFalse()
    {
        var garage = new Garage<Vehicle>("Test", capacity: 1);

        bool removed = garage.Remove("XXX999");

        Assert.False(removed);
    }

    [Fact]
    public void Find_LowerCaseRegistrationNumber_FindsVehicle()
    {
        var garage = new Garage<Vehicle>("Test", capacity: 1);
        Car car = NewCar("ABC123");
        garage.Park(car);

        Vehicle? found = garage.Find("abc123");

        Assert.Same(car, found);
    }

    [Fact]
    public void Find_UnknownRegistrationNumber_ReturnsNull()
    {
        var garage = new Garage<Vehicle>("Test", capacity: 1);
        garage.Park(NewCar("ABC123"));

        Vehicle? found = garage.Find("XXX999");

        Assert.Null(found);
    }

    [Fact]
    public void Search_ColorAndWheels_ReturnsOnlyMatches()
    {
        var garage = new Garage<Vehicle>("Test", capacity: 5);
        Car blackCar = NewCar("AAA111", "Black");
        garage.Park(blackCar);
        garage.Park(NewCar("BBB222", "Red"));
        garage.Park(NewMotorcycle("CCC333", "Black"));
        var filter = new VehicleFilter { Color = "Black", NumberOfWheels = 4 };

        List<Vehicle> matches = garage.Search(filter);

        Assert.Single(matches);
        Assert.Same(blackCar, matches[0]);
    }

    [Fact]
    public void Search_NoMatch_ReturnsEmptyList()
    {
        var garage = new Garage<Vehicle>("Test", capacity: 1);
        garage.Park(NewCar("AAA111", "Black"));
        var filter = new VehicleFilter { Color = "Pink" };

        List<Vehicle> matches = garage.Search(filter);

        Assert.Empty(matches);
    }

    [Fact]
    public void CountByType_MixedVehicles_CountsPerType()
    {
        var garage = new Garage<Vehicle>("Test", capacity: 5);
        garage.Park(NewCar("AAA111"));
        garage.Park(NewCar("BBB222"));
        garage.Park(NewMotorcycle("CCC333"));

        List<(string TypeName, int Count)> counts = garage.CountByType();

        Assert.Equal(2, counts.Count);
        Assert.Contains(("Car", 2), counts);
        Assert.Contains(("Motorcycle", 1), counts);
    }

    [Fact]
    public void Constructor_ZeroCapacity_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Garage<Vehicle>("Test", capacity: 0));
    }

    [Fact]
    public void GetEnumerator_ParkedVehicles_LoopsOverThem()
    {
        var garage = new Garage<Vehicle>("Test", capacity: 2);
        Car car = NewCar("AAA111");
        Motorcycle motorcycle = NewMotorcycle("BBB222");
        garage.Park(car);
        garage.Park(motorcycle);

        List<Vehicle> looped = [];
        foreach (Vehicle vehicle in garage)
            looped.Add(vehicle);

        Assert.Equal(2, looped.Count);
        Assert.Contains(car, looped);
        Assert.Contains(motorcycle, looped);
    }

    [Fact]
    public void Register_DuplicateRegistrationNumber_ReturnsFalse()
    {
        var garage = new Garage<Vehicle>("Test", capacity: 1);
        garage.Register(NewCar("ABC123"));

        bool registered = garage.Register(NewCar("abc123"));

        Assert.False(registered);
        Assert.Single(garage.GetRegisteredVehicles());
    }

    [Fact]
    public void CanHold_VehiclesFitInEmptyGarage_ReturnsTrue()
    {
        // three motorcycles take one parking space together
        var garage = new Garage<Vehicle>("Test", capacity: 1);
        List<Vehicle> vehicles = [NewMotorcycle("AAA111"), NewMotorcycle("BBB222"), NewMotorcycle("CCC333")];

        bool canHold = garage.CanHold(vehicles);

        Assert.True(canHold);
    }

    [Fact]
    public void CanHold_VehiclesTooBig_ReturnsFalse()
    {
        // two cars need two parking spaces
        var garage = new Garage<Vehicle>("Test", capacity: 1);
        List<Vehicle> vehicles = [NewCar("AAA111"), NewCar("BBB222")];

        bool canHold = garage.CanHold(vehicles);

        Assert.False(canHold);
    }

    [Fact]
    public void SetParked_SwapOneVehicleForAnother_RemovesFirstThenParksSecond()
    {
        // the garage is full, so the second car only fits after the first has left
        var garage = new Garage<Vehicle>("Test", capacity: 1);
        Car first = NewCar("AAA111");
        Car second = NewCar("BBB222");
        garage.Park(first);
        garage.Register(second);

        List<ParkingReport> reports = garage.SetParked([second]);

        Assert.False(garage.IsParked("AAA111"));
        Assert.True(garage.IsParked("BBB222"));
        Assert.Equal(2, reports.Count);
    }

    [Fact]
    public void SetParked_NotEnoughSpaceForWantedVehicle_ReportsNotEnoughSpace()
    {
        var garage = new Garage<Vehicle>("Test", capacity: 1);
        Car parked = NewCar("AAA111");
        Car extra = NewCar("BBB222");
        garage.Park(parked);
        garage.Register(extra);

        List<ParkingReport> reports = garage.SetParked([parked, extra]);

        ParkingReport report = Assert.Single(reports);
        Assert.Equal(ParkingChange.NotEnoughSpace, report.Change);
        Assert.False(garage.IsParked("BBB222"));
    }
}