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
        //frmEixo frmEixoBackup = new frmEixo();
        public paramdto Parametros { get; set; }

        public frmEixo()
        {
            InitializeComponent();
            Parametros = new paramdto();
        }

        public void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txteixox.Text) && !string.IsNullOrEmpty(txteixoy.Text) && !string.IsNullOrEmpty(txtfonte.Text) && !string.IsNullOrEmpty(txtmultVertical.Text) && !string.IsNullOrEmpty(txtmultHorizontal.Text))
            {
                Parametros.EixoX = decimal.Parse(txteixox.Text);
                Parametros.EixoY = decimal.Parse(txteixoy.Text);
                Parametros.Fonte = decimal.Parse(txtfonte.Text);
                Parametros.multVertical = decimal.Parse(txtmultVertical.Text);
                Parametros.multHorizontal = decimal.Parse(txtmultHorizontal.Text);
            }

            this.Close();
        }

    }
}
