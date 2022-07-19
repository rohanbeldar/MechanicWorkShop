using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MechanicWorkShop
{
    [XmlRoot("AppointmentList")]
    public class ListOfAppointments
    {

        List<Appointment> appointmentList;

        public ListOfAppointments()
        {
            appointmentList = new List<Appointment>();
        }

        public void Add(Appointment appointmentVo)
        {
            appointmentList.Add(appointmentVo);
        }

        public void Delete(Appointment appointmentVo)
        {
            appointmentList.Remove(appointmentVo);
        }

        public Appointment this[int i]
        {
            get { return appointmentList[i]; }
        }

        public int Count
        {
            get => appointmentList.Count;
        }
        [XmlArray("Appointments")]
        [XmlArrayItem("Appointment", typeof(Appointment))]
        public List<Appointment> AppointmentList { get => appointmentList; set => appointmentList = value; }

        //used to sort the data by firstname of customer
        public void sort()
        {
            appointmentList.Sort();
        }

        
    }
}
