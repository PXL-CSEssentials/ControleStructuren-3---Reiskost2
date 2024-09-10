using System;
using System.Collections.Generic;
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

namespace ControleStructuren_3___Reiskost2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            flightClassLabel.Visibility = Visibility.Hidden;
        }

        private void calculateButton_Click(object sender, RoutedEventArgs e)
        {
            // Declaratie variabelen         
            float totalFlightPrice;
            float totalResidencePrice;
            float totalTravelPrice;
            float amountToPay;

            // Test of vereiste gegevens correct ingevuld zijn

            bool testBase = float.TryParse(basePriceTextBox.Text, out float dayPrice);
            bool testFlight = float.TryParse(baseFlightTextBox.Text, out float baseFlightPrice);
            bool testPersons = int.TryParse(numberOfPersonsTextBox.Text, out int numberOfPersons);
            bool testDays = int.TryParse(numberOfDaysTextBox.Text, out int numberOfDays);
            bool testReduction = float.TryParse(reductionPercentageTextBox.Text, out float reduction);
            bool testFlightClass = int.TryParse(flightClassTextBox.Text, out int flightClass);

            // NIEUW in deze oefening:
            // MessageBox tonen als één van de gegevens ontbreekt. (if)
            // Anders: doe de berekeningen (else)        
            if (!testBase || !testFlight || !testPersons || !testDays || !testReduction || !testFlightClass)
            {
                MessageBox.Show("Niet alle gegevens zijn correct ingevuld!",
                    "Ontbrekende gegevens.",
                    MessageBoxButton.OK,
                    MessageBoxImage.Stop);
            }
            else
            {
                // Berekening prijzen als alle gegevens zijn ingevuld         
                totalFlightPrice = baseFlightPrice * numberOfPersons;
                switch (flightClass)
                {
                    case 1:
                        //vluchtprijs = vluchtprijs * 1.3f;
                        totalFlightPrice *= 1.3f; // vluchtprijs 30% verhoogd
                        break;
                    case 3:
                        totalFlightPrice *= 0.8f; // vluchtprijs 20% verlaagd                    
                        break;
                        // geen case 2 nodig, want voor vluchtklasse 2 blijft de vluchtprijs hetzelfde
                }

                switch (numberOfPersons)
                {
                    case 0:
                    case 1:
                    case 2:
                        totalResidencePrice = dayPrice * numberOfDays * numberOfPersons;
                        break;
                    case 3:
                        // Vanaf 3de persoon: ipv. maal 3 personen nu voor de laatste persoon 50% goedkoper
                        // Dus maal 2,5 persoon in feite.
                        totalResidencePrice = dayPrice * numberOfDays * 2.5f;
                        break;
                    default:
                        // Meer dan 3 personen
                        totalResidencePrice = dayPrice * numberOfDays * 2.5f;  // 3 personen 
                        // Tel hier nog bij: de overige personen (aantalPersonen - 3 eerste)
                        // De overige personen krijgen daling van 70%, dus moeten maar 30% betalen (* 0.3f)
                        totalResidencePrice += dayPrice * numberOfDays * (numberOfPersons - 3) * 0.3f;  // > 3 personen                    
                        break;
                }
                totalTravelPrice = totalFlightPrice + totalResidencePrice;
                reduction = totalTravelPrice * (reduction / 100);
                amountToPay = totalTravelPrice - reduction;

                // Afdruk              
                resultTextBox.Text = $"REISKOST VOLGENS BESTEMMING NAAR {destinationTextBox.Text} \r\n\r\n" +
                    $"Totale vluchtprijs: {totalFlightPrice:c} \r\n" +
                    $"Totale verblijfsprijs: {totalResidencePrice:c} \r\n" +
                    $"Totale reisprijs: {totalTravelPrice:c} \r\n" +
                    $"Korting: {reduction:c} \r\n\r\n" +
                    $"Te betalen : {amountToPay:c}";
            }
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            baseFlightTextBox.Text = "0";
            destinationTextBox.Clear(); // of: TxtBestemming.Text = string.Empty;
            flightClassTextBox.Text = "2";
            basePriceTextBox.Text = "0";
            numberOfDaysTextBox.Text = "0";
            numberOfPersonsTextBox.Text = "0";
            reductionPercentageTextBox.Text = "0";
            resultTextBox.Clear();

            destinationTextBox.Focus();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void flightClassTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            flightClassLabel.Visibility = Visibility.Visible;
        }

        private void flightClassTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            flightClassLabel.Visibility = Visibility.Hidden;
        }
    }
}
