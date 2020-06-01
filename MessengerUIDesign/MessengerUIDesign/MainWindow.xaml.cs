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

namespace MessengerUIDesign
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void mnuShowParticipants_Click(object sender, RoutedEventArgs e)
        {
            Window window = new ParticipantsView();
            window.Show();
            Close();
            return;
        }
        private void mnu_QuitChat_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Aici o sa scriem si trimitem qt! ca sa oprim stream-ul");
            Close();
        }
        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtChat.Text))
                txtChat.Text = txtMessage.Text;
            else
                txtChat.Text = txtChat.Text + "\n" + txtMessage.Text;
            txtMessage.Clear();
        }

    }
}
