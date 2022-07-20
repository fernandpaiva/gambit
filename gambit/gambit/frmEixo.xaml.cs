using gambit.Models;
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
using System.Windows.Shapes;

namespace gambit
{
    /// <summary>
    /// Lógica interna para frmEixo.xaml
    /// </summary>
    public partial class frmEixo : Window
    {
        frmEixo frmEixoBackup = new frmEixo();
        Eixo eixo = new Eixo(); 
        public frmEixo()
        {
            InitializeComponent();
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txteixox.Text) && !string.IsNullOrEmpty(txteixoy.Text))
            {
                eixo.EixoX = decimal.Parse(txteixox.Text);
                eixo.EixoY = decimal.Parse(txteixoy.Text);
            }

        }
    }
}
