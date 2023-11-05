using System;
using System.Collections.Generic;
using System.Windows;
using static GameOfLife.Enums;

namespace GameOfLife
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        public Menu()
        {
            InitializeComponent();

            foreach (string name in Enum.GetNames(typeof(Pattern)))
                InitialPattern.Items.Add(name);

            InitialPattern.SelectedIndex = (int)Pattern.Empty;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Pattern initialPattern = (Pattern)Enum.Parse(typeof(Pattern), InitialPattern.SelectedItem.ToString()!);
            string boardSizeStr = BoardSize.Text;
            string maxNeighboursStr = MaxNeighbours.Text;
            string minNeighboursStr = MinNeighbours.Text;
            Dictionary<string, int> parsedValues = new Dictionary<string, int>();
            List<string> errors = new List<string>();
            errors.Add("The following errors occured:");

            bool validationCorrect = true;

            if (!ValidateTextBoxNumericValue(boardSizeStr, parsedValues, errors))
                validationCorrect = false;
            if (!ValidateMinMaxNeighboursValues(minNeighboursStr, maxNeighboursStr, parsedValues, errors))
                validationCorrect = false;

            if (validationCorrect)
            {
                Window mainWindow = new MainWindow(parsedValues["BoardSize"], 
                    initialPattern,
                    parsedValues["MinNeighbours"],
                    parsedValues["MaxNeighbours"]);

                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show(String.Join("\n", errors),
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private bool ValidateTextBoxNumericValue(string strValue, 
            Dictionary<string, int> parsedValues,
            List<string> errors)
        {
            int numericValue;
            if (Int32.TryParse(strValue, out numericValue))
            {
                parsedValues["BoardSize"] = numericValue;
                return true;
            }

            errors.Add("Invalid value for board size");
            return false;
        }

        private bool ValidateMinMaxNeighboursValues(string strMinValue, string strMaxValue, 
            Dictionary<string, int> parsedValues, List<string> errors)
        {
            int minNumericValue, maxNumericValue;
            bool parsedCorrectly = true;

            if (!Int32.TryParse(strMinValue, out minNumericValue))
            {
                parsedCorrectly = false;
                errors.Add("Invalid value for minimum number of neighbours");
            }
            if (!Int32.TryParse(strMaxValue, out maxNumericValue))
            {
                parsedCorrectly = false;
                errors.Add("Invalid value for maximum number of neighbours");
            }

            if (parsedCorrectly)
            {
                if (maxNumericValue < minNumericValue)
                {
                    errors.Add("Maximum number of neighbours can't be less than the minimum");
                    parsedCorrectly = false;
                }
                if (minNumericValue < 0 || minNumericValue > 8)
                {
                    errors.Add("Minimum number of neighbours must be in range [0, 8]");
                    parsedCorrectly = false;
                }
                if (maxNumericValue < 0 || maxNumericValue > 8)
                {
                    errors.Add("Maximum number of neighbours must be in range [0, 8]");
                    parsedCorrectly = false;
                }

                if (!parsedCorrectly)
                    return false;

                parsedValues["MinNeighbours"] = minNumericValue;
                parsedValues["MaxNeighbours"] = maxNumericValue;
                return true;
            }

            return false;
        }
    }
}
