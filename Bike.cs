using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechanicWorkShop
{
    public delegate void BikeDelegate();

    public class Bike : Vehicle
    {
        private int numOfRepairJobs;
        private BikeDelegate bikeDelegate;

        public int NumOfRepairJobs { get => numOfRepairJobs; set => numOfRepairJobs = value; }
        public Bike(int numOfStoneInWorkingArea, String vehicleType, decimal decimalVehicleMake, double doubleModelYear) : base(decimalVehicleMake, doubleModelYear, vehicleType)
        {
            this.numOfRepairJobs = numOfStoneInWorkingArea;

            //calling all methods with the help of delegate from this method
            callMethodsFromDelegates();
        }

        public Bike() { }
        private void callMethodsFromDelegates()
        {
            bikeDelegate = InspectVehicle;
            bikeDelegate += ProvideWorkEstimate;
            bikeDelegate += AssignWorkers;
            bikeDelegate();
        }

        //overridden base class abstract method
        public override void InspectVehicle()
        {
            try
            {
                Console.WriteLine("---- Operation: Bike is being inspected[Derived] ----");
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
                Duration = this.numOfRepairJobs * 2;
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
                TotalWorkers = this.numOfRepairJobs * 4;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        //This method is specific to this type of house only
        public void DoRepairJobs(int numOfRepairJobs)
        {
            try
            {
                String repairString = $"{numOfRepairJobs} Repairs are done to your bike";
                Console.WriteLine(repairString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
