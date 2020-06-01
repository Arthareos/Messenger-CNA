using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MessengerUIDesign
{
    /// <summary>
    /// Interaction logic for ParticipantsView.xaml
    /// </summary>
    public partial class ParticipantsView : Window
    {
        public ParticipantsView()
        {
            InitializeComponent();
        }
        private void mnuHideParticipants1_Click(object sender, RoutedEventArgs e)
        {
            Window window = new MainWindow();
            window.Show();
            Close();
            return;
        }

        private void mnuQuitChat_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Aici o sa scriem si trimitem qt! ca sa oprim stream-ul");
            Close();
        }
    }
}
