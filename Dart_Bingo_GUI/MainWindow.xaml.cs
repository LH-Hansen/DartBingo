using Dart_Bingo;
using System.IO;
using System.Windows;

namespace Dart_Bingo_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PdfService pdfService = new PdfService();

        private string _userInput;



        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            string currentDir = AppContext.BaseDirectory;
            string projectRoot = Path.GetFullPath(Path.Combine(currentDir, @"..\..\..\..\"));
            string targetDir = Path.Combine(projectRoot, @"Bingo PDF");

            _userInput = UserInputTextBox.Text;

            PdfService pdfService = new PdfService();

            if (!Directory.Exists(targetDir))
                Directory.CreateDirectory(targetDir);

            if (int.TryParse(_userInput, out int number))
            {
                try
                {
                    string pdfLocation = pdfService.CreatePDF(targetDir, number);
                    MessageBox.Show($"PDF generated at: {pdfLocation}");
                }
                catch (Exception)
                {
                    MessageBox.Show("An error has occurred");
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid number");
            }



        }
    }
}