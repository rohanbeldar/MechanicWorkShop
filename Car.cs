using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MechanicWorkShop
{
    public delegate void CarDelegate();
    public class Car : Vehicle
    {
        private int noOfServiceHistory;

        private CarDelegate carDelegate;

        public Car(int noOfService, String vehicleType, decimal decimalTimeSinceLastService, double doubleModelYear) : base(decimalTimeSinceLastService, doubleModelYear, vehicleType)
        {
            this.noOfServiceHistory = noOfService;

            //calling all methods with the help of delegate from this method
            callMethodsFromDelegates();
        }

        public Car() { }
        private void callMethodsFromDelegates()
        {
            carDelegate = InspectVehicle;
            carDelegate += ProvideWorkEstimate;
            carDelegate += AssignWorkers;
            carDelegate();
        }

        public int NoOfServiceHistory { get => noOfServiceHistory; set => noOfServiceHistory = value; }

        [XmlIgnore]
        public CarDelegate CarDelegate { get => carDelegate; set => carDelegate = value; }

        //overridden base class abstract method
        public override void InspectVehicle()
        {
            try
            {
                Console.WriteLine("---- Operation: Car is is being inspected [Derived] ----");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        //overridden base class abstract method
        public override void ProvideWorkEstimate()
        {
            try
            {
                Console.WriteLine("---- Operation: Total Work duration is estimated [Derived]----");
                //Setting Base class properties from derived classes
                TotalCost = ModelYear * 2;
                Duration = this.noOfServiceHistory * 2;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        //overridden base class abstract method
        public override void AssignWorkers()
        {
            try
            {
                Console.WriteLine("---- Operation: Workers are arranges [Derived]----");
                //Setting Base class properties from derived classes
                TotalWorkers = this.noOfServiceHistory;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        //This method is specific to this type of house only
        public void AskForServiceHistory(int totalService)
        {
            try
            {
                String serviceString = $"Data from {totalService} service history received";
                Console.WriteLine(serviceString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
