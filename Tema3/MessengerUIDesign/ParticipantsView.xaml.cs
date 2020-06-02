using System.Windows;

namespace MessengerUIDesign
{
    public partial class ParticipantsView : Window
    {
        public ParticipantsView()
        {
            InitializeComponent();
        }
        private void btnHideParticipants_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
