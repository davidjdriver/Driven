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

namespace ObserverExample.HardCodedChange
{
    /// <summary>
    /// Interaction logic for HardCodedChangeView.xaml
    /// </summary>
    public partial class HardCodedChangeView : UserControl
    {
        protected HardCodedChangeViewModel ViewModel
        {
            get { return this.DataContext as HardCodedChangeViewModel; }
            set { this.DataContext = value; }
        }

        public HardCodedChangeView()
        {
            InitializeComponent();
            this.ViewModel = new HardCodedChangeViewModel();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.ViewModel.Change();
        }
    }
}
