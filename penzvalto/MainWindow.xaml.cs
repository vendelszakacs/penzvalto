using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace penzvalto
{
	public partial class MainWindow : Window
	{
		private Dictionary<string, double> exchangeRates = new Dictionary<string, double>
		{
			{ "EUR", 400 },
			{ "USD", 350 },
			{ "GBP", 450 }
		};

		public MainWindow()
		{
			InitializeComponent();
			this.KeyDown += new KeyEventHandler(MainWindow_KeyDown);
		}

		private void ConvertButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				double amount = Convert.ToDouble(AmountTextBox.Text);
				string selectedCurrency = ((ComboBoxItem)CurrencyComboBox.SelectedItem).Content.ToString();

				bool isReverse = ReverseCheckBox.IsChecked.GetValueOrDefault(false);
				bool applyFee = FeeCheckBox.IsChecked.GetValueOrDefault(false);

				double result = 0;

				if (applyFee)
				{
					amount *= 0.95;
				}

				if (isReverse)
				{
					result = amount * exchangeRates[selectedCurrency];
				}
				else
				{
					result = amount / exchangeRates[selectedCurrency];
				}

				ResultTextBlock.Text = $"{amount} Ft = {result} {selectedCurrency}";

				HistoryListBox.Items.Add($"{amount} Ft → {result} {selectedCurrency}");

				if (!isReverse)
				{
					string multipleCurrencies = $"{amount} Ft = ";
					foreach (var currency in exchangeRates.Keys)
					{
						double multiResult = amount / exchangeRates[currency];
						multipleCurrencies += $"{multiResult:0.##} {currency}, ";
					}
					multipleCurrencies = multipleCurrencies.TrimEnd(',', ' ');
					ResultTextBlock.Text = multipleCurrencies;
				}
			}
			catch (Exception)
			{
				MessageBox.Show("Adj meg számot!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void ClearButton_Click(object sender, RoutedEventArgs e)
		{
			AmountTextBox.Clear();
			ResultTextBlock.Text = string.Empty;
			HistoryListBox.Items.Clear();
		}

		private void MainWindow_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				ConvertButton_Click(sender, e);
			}
			else if (e.Key == Key.Escape)
			{
				ClearButton_Click(sender, e);
			}
		}
	}
}