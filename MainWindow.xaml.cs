using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace MechanicWorkShop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<int, String> timeslots = new Dictionary<int, String>();

        ListOfAppointments apptList = new ListOfAppointments();

        ListOfAppointments takenAppointments = new ListOfAppointments();

        List<Appointment> finalList = new List<Appointment>();

        List<String> appointmentEntries = new List<String>();

        Appointment appointmentObj = new Appointment();

        int selectedAppointmentTime = 0;

        public List<Appointment> FinalList { get => finalList; set => finalList = value; }
        public Appointment AppointmentObj { get => appointmentObj; set => appointmentObj = value; }

        public MainWindow()
        {

            InitializeComponent();

            //Filling house type combo
            cmbVehicleType.Items.Add("Bike");
            cmbVehicleType.Items.Add("Car");
            cmbVehicleType.Items.Add("Pick-up Truck");

            //Filling appoint type combo
            //FillAppointmentCombo();
            cmbAppointmentTime.Items.Add("07 AM");
            cmbAppointmentTime.Items.Add("08 AM");
            cmbAppointmentTime.Items.Add("09 AM");
            cmbAppointmentTime.Items.Add("04 PM");
            cmbAppointmentTime.Items.Add("05 PM");
            cmbAppointmentTime.Items.Add("06 PM");

            if (checkIfFileExists())
            {
                FillAppointmentCombo();
            }
        }

        private void RemoveTakenSlotsFromCombo(Appointment item)
        {
            if (item.Time == "7 AM")
            {
                cmbAppointmentTime.Items.Remove("07 AM");
            }
            else if (item.Time == "8 AM")
            {
                cmbAppointmentTime.Items.Remove("08 AM");
            }
            else if (item.Time == "9 AM")
            {
                cmbAppointmentTime.Items.Remove("09 AM");
            }
            else if (item.Time == "4 PM")
            {
                cmbAppointmentTime.Items.Remove("04 PM");
            }
            else if (item.Time == "5 PM")
            {
                cmbAppointmentTime.Items.Remove("05 PM");
            }
            else if (item.Time == "6 PM")
            {
                cmbAppointmentTime.Items.Remove("06 PM");
            }
        }

        private bool checkIfFileExists()
        {
            String currentDirectory = Directory.GetCurrentDirectory();
            Directory.CreateDirectory(currentDirectory);
            String filePath = currentDirectory + @"\CustomerData.xml";
            if (File.Exists(filePath))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void btnDisplay_Click(object sender, RoutedEventArgs e)
        {
            if (checkIfFileExists())
            {
                finalList = ReadDataToXMLFile().AppointmentList;
                CustomerDataGrid.ItemsSource = finalList;
            }
        }
        private void FillAppointmentCombo()
        {
            finalList = ReadDataToXMLFile().AppointmentList;
            foreach (var item in finalList)
            {
                RemoveTakenSlotsFromCombo(item);
            }
        }

        private void Button_Submit(object sender, RoutedEventArgs e)
        {
            try
            {
                String fname = txtFname.Text.Trim();
                String lname = txtLname.Text.Trim();
                String creditCard = txtCreditCard.Text.Trim();
                String timeSinceLastService = txtLastService.Text.Trim();
                decimal decimaltimeSinceLastService = 0m;
                String modelYear = txtModelYear.Text.Trim();
                double doubleModelYear = 0.0;
                int intBikeInput = 0;
                String bikeInput = txtBikeInput.Text.Trim();
                int intCarInput = 0;
                String carInput = txtCarInput.Text.Trim();
                int intTruckInput = 0;
                String truckInput = txtTruckInput.Text.Trim();

                if (validateUserInputs(fname, lname, creditCard, timeSinceLastService, ref decimaltimeSinceLastService, modelYear, ref doubleModelYear, ref intBikeInput, bikeInput, ref intCarInput, carInput, ref intTruckInput, truckInput))
                {
                    Customer customerObj = new Customer();
                    customerObj.Name = fname;
                    customerObj.Surname = lname;
                    customerObj.CreditCard = ConcealedCreditCardNumber(creditCard);

                    Vehicle vehicleObj = null;
                    switch (cmbVehicleType.SelectedIndex)
                    {
                        case 0:
                            BikeCase(customerObj, decimaltimeSinceLastService, doubleModelYear, out vehicleObj, intBikeInput);
                            break;

                        case 1:
                            CarCase(customerObj, decimaltimeSinceLastService, doubleModelYear, out vehicleObj, intTruckInput);
                            break;

                        case 2:
                            PickUpTruckCase(customerObj, decimaltimeSinceLastService, doubleModelYear, out vehicleObj, intCarInput);
                            break;

                    }
                    FillAppointmentListWithoutCustomer();
                    appointmentObj = new Appointment();
                    appointmentObj.Time = timeslots[selectedAppointmentTime];
                    appointmentObj.Customer = customerObj;
                    apptList.Add(appointmentObj);
                    foreach (var item in finalList)
                    {
                        apptList.Add(item);
                    }

                    timeslots.Remove(selectedAppointmentTime);

                    //appointmentEntries.Remove(appointmentEntries[selectedAppointmentTime]);

                    cmbAppointmentTime.Items.RemoveAt(selectedAppointmentTime);

                    if (apptList.Count > 0)
                    {
                        apptList.sort();
                    }

                    ClearForm();

                    WriteDataToXMLFile();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void WriteDataToXMLFile()
        {

            XmlSerializer serializer = new XmlSerializer(typeof(ListOfAppointments), new Type[] { typeof(Vehicle) });
            TextWriter tw = new StreamWriter("CustomerData.xml");
            serializer.Serialize(tw, apptList);
            tw.Close();
        }

        private ListOfAppointments ReadDataToXMLFile()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ListOfAppointments), new Type[] { typeof(Vehicle) });
            TextReader tw = new StreamReader("CustomerData.xml");
            ListOfAppointments apps = (ListOfAppointments)serializer.Deserialize(tw);
            takenAppointments = apps;
            tw.Close();
            return apps;
        }

        private static void BikeCase(Customer customerObj, decimal decimaltimeSinceLastService, double doubleModelYear, out Vehicle vehicleObj, int numOfRepairJobs)
        {
            vehicleObj = new Bike(numOfRepairJobs, "Bike", decimaltimeSinceLastService, doubleModelYear);

            //calling child specific method
            ((Bike)vehicleObj).DoRepairJobs(numOfRepairJobs);

            customerObj.Vehicle = vehicleObj;
        }

        private static void PickUpTruckCase(Customer customerObj, decimal decimalTimeSinceLastService, double doubleModelYear, out Vehicle vehicleObj, int numOfOperations)
        {
            vehicleObj = new PickUpTruck(numOfOperations, "Pick-up Truck", decimalTimeSinceLastService, doubleModelYear);

            ((PickUpTruck)vehicleObj).DoOperations(numOfOperations);

            customerObj.Vehicle = vehicleObj;
        }

        private void CarCase(Customer customerObj, decimal decimalTimeSinceLastService, double doubleModelYear, out Vehicle vehicleObj, int totalService)
        {
            vehicleObj = new Car(totalService, "Car", decimalTimeSinceLastService, doubleModelYear);

            ((Car)vehicleObj).AskForServiceHistory(totalService);

            customerObj.Vehicle = vehicleObj;
        }

        private bool validateUserInputs(string fname, string lname, string creditCard, string timeSinceLastService, ref decimal decimalTimeSinceLastService, string modelYear, ref double doubleModelYear, ref int intBikeInput, string bikeInput, ref int intCarInput, string carInput, ref int intTruckInput, string truckInput)
        {
            //-------------- Input validations Start --------------
            if ((!(bool)Appointment_Yes.IsChecked) && (!(bool)Appointment_No.IsChecked))
            {
                displayValidation(lblAppointmentValidation, "Please make a choice here.");
                return false;
            }

            if (cmbAppointmentTime.SelectedIndex == -1)
            {
                displayValidation(lblAppointmentTimeValidation, "Please select the appointment time.");
                return false;
            }

            if (fname.Length == 0)
            {
                displayValidation(lblFnameValidation, "Please enter First name.");
                return false;
            }

            if (lname.Length == 0)
            {
                displayValidation(lblLnameValidation, "Please enter Last name.");
                return false;
            }

            if (creditCard.Length == 0)
            {
                displayValidation(lblCreditCardValidation, "Please enter credit card number.");
                return false;
            }

            if (!ValidateCardNumber(creditCard))
            {
                txtCreditCard.Foreground = Brushes.Red;
                displayValidation(lblCreditCardValidation, "Please enter only 16 digits credit card number.");
                return false;
            }

            if (cmbVehicleType.SelectedIndex == -1)
            {
                displayValidation(lblVehicleTypeValidation, "Please select the Vehicle type.");
                return false;
            }

            if ((cmbVehicleType.SelectedIndex == 0) && (bikeInput.Length == 0))
            {
                displayValidation(lblBikeInputValidation, "Please enter the no of repair Jobs");
                return false;
            }

            if ((cmbVehicleType.SelectedIndex == 1) && (carInput.Length == 0))
            {
                displayValidation(lblCarInputValidation, "Please enter the no of service history.");
                return false;
            }

            if ((cmbVehicleType.SelectedIndex == 2) && (truckInput.Length == 0))
            {
                displayValidation(lblTruckInputValidation, "Please enter the no of operations.");
                return false;
            }

            if ((cmbVehicleType.SelectedIndex == 0) && (!int.TryParse(bikeInput, out intBikeInput)))
            {
                txtBikeInput.Foreground = Brushes.Red;
                displayValidation(lblBikeInputValidation, "Please enter valid no of Repair Jobs.");
                return false;
            }

            if ((cmbVehicleType.SelectedIndex == 1) && (!int.TryParse(carInput, out intCarInput)))
            {
                txtCarInput.Foreground = Brushes.Red;
                displayValidation(lblCarInputValidation, "Please enter valid no of operations.");
                return false;
            }

            if ((cmbVehicleType.SelectedIndex == 2) && (!int.TryParse(truckInput, out intTruckInput)))
            {
                txtTruckInput.Foreground = Brushes.Red;
                displayValidation(lblTruckInputValidation, "Please enter valid no of service history");
                return false;
            }

            if (timeSinceLastService.Length == 0)
            {
                displayValidation(lblLastServiceValidation, "Please enter duration.");
                return false;
            }

            if (!decimal.TryParse(timeSinceLastService, out decimalTimeSinceLastService))
            {
                txtLastService.Foreground = Brushes.Red;
                displayValidation(lblLastServiceValidation, "Please enter valid duration.");
                return false;
            }

            if (decimalTimeSinceLastService < 0 || decimalTimeSinceLastService > 24)
            {
                txtLastService.Foreground = Brushes.Red;
                displayValidation(lblLastServiceValidation, "Please enter time since your last service (between 0 - 24 months)");
                return false;
            }

            if (modelYear.Length == 0)
            {
                displayValidation(lblModelYearValidation, "Please enter model year.");
                return false;
            }

            if (!double.TryParse(modelYear, out doubleModelYear))
            {
                txtModelYear.Foreground = Brushes.Red;
                displayValidation(lblModelYearValidation, "Please enter valid model year.");
                return false;
            }

            if (doubleModelYear < 2005 || doubleModelYear > 2021)
            {
                txtModelYear.Foreground = Brushes.Red;
                displayValidation(lblModelYearValidation, "Please enter model year between 2005 to 2021");
                return false;
            }

            //if (decimal.Parse(doubleModelYear.ToString()) < 2005)
            //{
            //    txtModelYear.Foreground = Brushes.Red;
            //    displayValidation(lblModelYearValidation, "Model year cannot be less than 2005");
            //    return false;
            //}
            return true;
            //-------------- Input validations End --------------
        }

        private void FillAppointmentListWithoutCustomer()
        {
            if (timeslots.Count == 0)
            {
                for (int i = 0; i < cmbAppointmentTime.Items.Count; i++)
                {
                    String time = ConvertSlot((String)cmbAppointmentTime.Items.GetItemAt(i));
                    int slotIndex = cmbAppointmentTime.Items.IndexOf((String)cmbAppointmentTime.Items.GetItemAt(i));
                    timeslots.Add(slotIndex, time);
                }
            }
        }

        private string ConvertSlot(String slot)
        {
            String convertedString = String.Empty;
            String dayZone = String.Empty;
            String hour = String.Empty;
            try
            {
                dayZone = slot.Substring(3, 2);

                if (int.Parse(slot.Substring(0, 2)) > 12)
                {
                    hour = (int.Parse(slot.Substring(0, 2)) - 12).ToString();
                }
                else
                {
                    hour = slot.Substring(1, 1);
                }

                convertedString = hour + "" + " " + dayZone;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return convertedString;
        }

        private bool ValidateCardNumber(String cardNumber)
        {
            bool isValidCard = false;
            try
            {
                if (cardNumber.Length == 16)
                {
                    Char[] cardNo = cardNumber.ToCharArray();
                    for (int i = 0; i < cardNo.Length; i++)
                    {
                        if (Char.IsDigit(cardNo[i]))
                        {
                            isValidCard = true;
                        }
                        else
                        {
                            isValidCard = false;
                            break;
                        }
                    }
                }
                else
                {
                    isValidCard = false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return isValidCard;
        }



        private void HandleAppointmentSelection(object sender, RoutedEventArgs e)
        {
            clearAppointmentComboValidation();
            selectedAppointmentTime = cmbAppointmentTime.SelectedIndex;
        }

        private void HandleVehicleTypeCheck(object sender, RoutedEventArgs e)
        {
            if (cmbVehicleType.SelectedIndex != -1)
            {
                lblVehicleTypeValidation.Visibility = Visibility.Collapsed;

                clearBikeValidation();
                clearCarValidation();
                clearTruckValidation();
                if (cmbVehicleType.SelectedIndex == 0)
                {
                    //For Bike
                    bikePanel.Visibility = Visibility.Visible;
                    carPanel.Visibility = Visibility.Collapsed;
                    pickUptruckPanel.Visibility = Visibility.Collapsed;
                }
                else if (cmbVehicleType.SelectedIndex == 1)
                {
                    //For Car
                    carPanel.Visibility = Visibility.Visible;
                    bikePanel.Visibility = Visibility.Collapsed;
                    pickUptruckPanel.Visibility = Visibility.Collapsed;
                }
                else if (cmbVehicleType.SelectedIndex == 2)
                {
                    //For Truck
                    pickUptruckPanel.Visibility = Visibility.Visible;
                    carPanel.Visibility = Visibility.Collapsed;
                    bikePanel.Visibility = Visibility.Collapsed;
                }
            }

        }

        private void showHideData_selectionChanged(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (rb.Name == "Appointment_No")
            {
                btnSubmit.Visibility = Visibility.Collapsed;
                lblAppointmentValidation.Visibility = Visibility.Collapsed;
            }
            else
            {
                btnSubmit.Visibility = Visibility.Visible;
                lblAppointmentValidation.Visibility = Visibility.Collapsed;
            }
        }
        private void txtglobal_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb.Name == "txtFname")
            {
                if (tb.Text.Trim().Length > 0)
                {
                    txtFname.Foreground = Brushes.Black;
                    clearFirstNameValidation();
                }
            }
            if (tb.Name == "txtLname")
            {
                if (tb.Text.Trim().Length > 0)
                {
                    txtLname.Foreground = Brushes.Black;
                    clearLastNameValidation();
                }
            }
            if (tb.Name == "txtCreditCard")
            {
                if (tb.Text.Trim().Length > 0)
                {
                    txtCreditCard.Foreground = Brushes.Black;
                    clearCreditCardValidation();
                }
            }
            else if (tb.Name == "txtBikeInput")
            {
                if ((cmbVehicleType.SelectedIndex == 0) && (txtBikeInput.Text.Trim().Length > 0))
                {
                    txtBikeInput.Foreground = Brushes.Black;
                    clearBikeValidation();
                }
            }
            else if (tb.Name == "txtCarInput")
            {
                if ((cmbVehicleType.SelectedIndex == 1) && (txtCarInput.Text.Trim().Length > 0))
                {
                    txtCarInput.Foreground = Brushes.Black;
                    clearCarValidation();
                }
            }
            else if (tb.Name == "txtTruckInput")
            {
                if ((cmbVehicleType.SelectedIndex == 2) && (txtTruckInput.Text.Trim().Length > 0))
                {
                    txtTruckInput.Foreground = Brushes.Black;
                    clearTruckValidation();
                }
            }
            else if (tb.Name == "txtLastService")
            {
                if (tb.Text.Trim().Length > 0)
                {
                    txtLastService.Foreground = Brushes.Black;
                    clearLastServicedValidation();
                }
            }
            else if (tb.Name == "txtModelYear")
            {
                if (tb.Text.Trim().Length > 0)
                {
                    txtModelYear.Foreground = Brushes.Black;
                    clearModelYearValidation();
                }
            }
        }
        private String ConcealedCreditCardNumber(String cardNumber)
        {
            String concealedString = String.Empty;
            try
            {
                Char[] cardNo = cardNumber.ToCharArray();
                for (int i = 0; i < cardNo.Length; i++)
                {
                    if ((i >= 4) && (i <= 11))
                    {
                        concealedString += "X";
                    }
                    else
                    {
                        concealedString += cardNo[i];
                    }
                    if ((concealedString.Length == 4) || (concealedString.Length == 9) || (concealedString.Length == 14))
                    {
                        concealedString += " ";
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return concealedString;
        }

        private void ClearForm()
        {
            clearAppointmentSelection();
            clearAppointmentCombo();
            clearFirstNameTxt();
            clearLastNameTxt();
            clearCreditCardTxt();
            clearVehicleTypeCombo();
            clearBikeInputTxt();
            clearCarInputTxt();
            clearTruckInputTxt();
            clearLastServicedTxt();
            clearModelYearTxt();
            clearAppointmentValidation();
            clearAppointmentComboValidation();
            clearFirstNameValidation();
            clearLastNameValidation();
            clearCreditCardValidation();
            clearHouseTypeValidation();
            clearBikeValidation();
            clearCarValidation();
            clearTruckValidation();
            clearLastServicedValidation();
            clearModelYearValidation();
        }

        private void clearAppointmentSelection()
        {
            Appointment_Yes.IsChecked = false;
            Appointment_No.IsChecked = false;
        }
        private void clearAppointmentCombo()
        {
            cmbAppointmentTime.SelectedIndex = -1;
        }
        private void clearFirstNameTxt()
        {
            txtFname.Text = "";
        }
        private void clearLastNameTxt()
        {
            txtLname.Text = "";
        }
        private void clearCreditCardTxt()
        {
            txtCreditCard.Text = "";
        }
        private void clearVehicleTypeCombo()
        {
            cmbVehicleType.SelectedIndex = -1;
        }
        private void clearBikeInputTxt()
        {
            txtBikeInput.Text = "";
        }
        private void clearCarInputTxt()
        {
            txtCarInput.Text = "";
        }
        private void clearTruckInputTxt()
        {
            txtTruckInput.Text = "";
        }
        private void clearLastServicedTxt()
        {
            txtLastService.Text = "";
        }

        private void clearModelYearTxt()
        {
            txtModelYear.Text = "";
        }
        private void clearAppointmentValidation()
        {
            lblAppointmentValidation.Content = "";
            lblAppointmentValidation.Visibility = Visibility.Collapsed;
        }
        private void clearAppointmentComboValidation()
        {
            lblAppointmentTimeValidation.Content = "";
            lblAppointmentTimeValidation.Visibility = Visibility.Collapsed;
        }

        private void clearFirstNameValidation()
        {
            lblFnameValidation.Content = "";
            lblFnameValidation.Visibility = Visibility.Collapsed;
        }
        private void clearLastNameValidation()
        {
            lblLnameValidation.Content = "";
            lblLnameValidation.Visibility = Visibility.Collapsed;
        }

        private void clearCreditCardValidation()
        {
            lblCreditCardValidation.Content = "";
            lblCreditCardValidation.Visibility = Visibility.Collapsed;
        }
        private void clearHouseTypeValidation()
        {
            lblVehicleTypeValidation.Content = "";
            lblVehicleTypeValidation.Visibility = Visibility.Collapsed;
        }
        private void clearBikeValidation()
        {
            lblBikeInputValidation.Content = "";
            lblBikeInputValidation.Visibility = Visibility.Collapsed;
        }
        private void clearCarValidation()
        {
            lblCarInputValidation.Content = "";
            lblCarInputValidation.Visibility = Visibility.Collapsed;
        }
        private void clearTruckValidation()
        {
            lblTruckInputValidation.Content = "";
            lblTruckInputValidation.Visibility = Visibility.Collapsed;
        }
        private void clearLastServicedValidation()
        {
            lblLastServiceValidation.Content = "";
            lblLastServiceValidation.Visibility = Visibility.Collapsed;
        }
        private void clearModelYearValidation()
        {
            lblModelYearValidation.Content = "";
            lblModelYearValidation.Visibility = Visibility.Collapsed;
        }
        
        private void displayValidation(Label lblName, String validationMessage)
        {
            lblName.Visibility = Visibility.Visible;
            lblName.Foreground = Brushes.Red;
            lblName.Content = validationMessage;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string userInput = txtCustomerNameSearch.Text.Trim();
            if (userInput.Length > 0)
            {
                var resultList = from person in FinalList
                                 where person.Customer.Name == userInput
                                 select person;

                CustomerDataGrid.ItemsSource = resultList;

            }
            else
            {
                CustomerDataGrid.ItemsSource = FinalList;
            }
        }

        void DeleteData(object sender, RoutedEventArgs e)
        {
            if (CustomerDataGrid.SelectedIndex >= 0) 
            {
                Appointment obj = CustomerDataGrid.SelectedItem as Appointment;
                finalList.Remove(obj);
                takenAppointments.Delete(obj);
                UpdateXMLFile();
                FillAppointmentCombo();
                CustomerDataGrid.ItemsSource = finalList;
            }
        }
        private void UpdateXMLFile()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ListOfAppointments), new Type[] { typeof(Vehicle) });
            TextWriter tw = new StreamWriter("CustomerData.xml");
            serializer.Serialize(tw, takenAppointments);
            tw.Close();
        }

        private void CustomerDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            Console.WriteLine((e.Row.Item as Appointment).Customer.Vehicle.ModelYear);
            var app = e.Row.Item as Appointment;

            finalList.Remove(app);

            if (e.EditAction == DataGridEditAction.Commit)
            {
                var column = e.Column as DataGridBoundColumn;
                if (column != null)
                {
                    var bindingPath = (column.Binding as Binding).Path.Path;
                    int rowIndex = e.Row.GetIndex();
                    var el = e.EditingElement as TextBox;
                    
                    if (bindingPath == "Customer.Name")
                    {
                        app.Customer.Name = el.Text;
                    }
                    if (bindingPath == "Customer.Surname")
                    {
                        app.Customer.Surname = el.Text;
                    }
                    if (bindingPath == "Customer.CreditCard")
                    {
                        app.Customer.CreditCard = el.Text;
                    }
                    if (bindingPath == "Customer.Vehicle.VehicleType")
                    {
                        app.Customer.Vehicle.VehicleType = el.Text;
                    }
                    if (bindingPath == "Customer.Vehicle.TimeSinceLastService")
                    {
                        app.Customer.Vehicle.TimeSinceLastService = Decimal.Parse(el.Text);
                    }
                    if (bindingPath == "Customer.Vehicle.ModelYear")
                    {
                        app.Customer.Vehicle.ModelYear = Double.Parse(el.Text);
                    }
                    if (bindingPath == "Customer.Vehicle.Duration")
                    {
                        app.Customer.Vehicle.Duration = int.Parse(el.Text);
                    }
                    if (bindingPath == "Customer.Vehicle.TotalCost")
                    {
                        app.Customer.Vehicle.TotalCost = Double.Parse(el.Text);
                    }
                    if (bindingPath == "Customer.Vehicle.TotalWorkers")
                    {
                        app.Customer.Vehicle.TotalWorkers = int.Parse(el.Text);
                    }

                }
            }
            finalList.Add(app);
            UpdateXMLFile();
        }
    }
}
