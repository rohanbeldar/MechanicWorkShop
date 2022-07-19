using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MechanicWorkShop
{
    [XmlInclude(typeof(Car))]
    [XmlInclude(typeof(Bike))]
    [XmlInclude(typeof(PickUpTruck))]
    [Serializable]
    public abstract class Vehicle : IVehicle
    {
        private decimal timeSinceLastService;
        private double modelYear;
        private int duration;
        private double totalCost;
        private int totalWorkers;
        private String vehicleType;

        public Vehicle()
        {
        }

        public decimal TimeSinceLastService { get => timeSinceLastService; set => timeSinceLastService = value; }
        public double ModelYear { get => modelYear; set => modelYear = value; }
        public int Duration { get => duration; set => duration = value; }
        public double TotalCost { get => totalCost; set => totalCost = value; }
        public int TotalWorkers { get => totalWorkers; set => totalWorkers = value; }
        public string VehicleType { get => vehicleType; set => vehicleType = value; }

        public Vehicle(decimal vehicleMake, double modelYear, String vehicleType)
        {
            //setting instance variable values
            this.timeSinceLastService = vehicleMake;
            this.modelYear = modelYear;
            this.vehicleType = vehicleType;
        }
        //kept base class methods as abstract and abstracts methods are virtual, they will be overridden in the derived class
        public abstract void InspectVehicle();
        public abstract void ProvideWorkEstimate();
        public abstract void AssignWorkers();


    }
}
