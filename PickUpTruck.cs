using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechanicWorkShop
{
    public delegate void PickUpTruckDelegate();
    public class PickUpTruck : Vehicle
    {
        private int numOfOperations;
        private PickUpTruckDelegate pickUpTruckDelegate;

        public int NumOfOperations { get => numOfOperations; set => numOfOperations = value; }

        public PickUpTruck(int numOfOperations, String vehicleType, decimal decimalTimeSinceLastService, double doubleModelYear) : base(decimalTimeSinceLastService, doubleModelYear, vehicleType)
        {
            this.numOfOperations = numOfOperations;

            //calling all methods with the help of delegate from this method
            callMethodsFromDelegates();
        }

        public PickUpTruck() { }

        private void callMethodsFromDelegates()
        {
            pickUpTruckDelegate = InspectVehicle;
            pickUpTruckDelegate += ProvideWorkEstimate;
            pickUpTruckDelegate += AssignWorkers;
            pickUpTruckDelegate();
        }

        //overridden base class abstract method
        public override void InspectVehicle()
        {
            try
            {
                Console.WriteLine("---- Operation: Create Work Design [Derived] ----");
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
                Console.WriteLine("---- Operation: Work Estimate [Derived]----");

                //Setting Base class properties from derived classes
                TotalCost = ModelYear * 2;
                Duration = this.numOfOperations * 2;
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
                Console.WriteLine("---- Operation: Arrange Workers [Derived]----");
                //Setting Base class properties from derived classes
                TotalWorkers = this.numOfOperations * 2;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        //This method is specific to this type of house only
        public void DoOperations(int noOfOperations)
        {
            try
            {
                String entranceString = $"{noOfOperations} repair work is done for yout pick-up.";
                Console.WriteLine(entranceString);
                Console.WriteLine("Transmission is tuned");
                Console.WriteLine("radiator is flushed");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
