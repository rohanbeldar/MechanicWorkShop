using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechanicWorkShop
{
    interface IVehicle
    {
        // Declared properties of Base class which is Vehicle in our case
        decimal TimeSinceLastService { get; set; }
        double ModelYear { get; set; }
        int Duration { get; set; }
        double TotalCost { get; set; }
        int TotalWorkers { get; set; }

        void InspectVehicle();
        void ProvideWorkEstimate();
        void AssignWorkers();
    }
}
