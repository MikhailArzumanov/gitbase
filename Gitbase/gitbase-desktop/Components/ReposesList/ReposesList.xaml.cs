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

namespace gitbase_desktop.Components.ReposesList {
    
    public partial class ReposesList : UserControl {
        private MainWindow TheParent { get; set; } = null;
        public ReposesList(MainWindow theParent) {
            InitializeComponent();
            TheParent = theParent;
        }


    }
}
