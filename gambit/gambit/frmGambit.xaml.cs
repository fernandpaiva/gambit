using System;
using System.Collections.Generic;
using System.IO;
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

namespace gambit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmGambit : Window
    {
        frmEixo frmEixo = new frmEixo();
        private void ButtonAddName_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txttag.Text) && !lsttag.Items.Contains(txttag.Text))
            {
                lsttag.Items.Add(txttag.Text.ToUpper());

               
                frmEixo.Owner = this;
                frmEixo.ShowDialog();

                var param = frmEixo.Parametros;


                txttag.Clear();
            }
        }

        public void btnSalvarArquivo_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarCampos())
            {
                return;
            }
                            
            lblStatus.Content = null;
            if (SalvarArquivo())
                lblStatus.Content = "Salvo com sucesso!";
            else
                lblStatus.Content = "Erro ao salvar!";
                
        }

        public bool SalvarArquivo()
        {
            string descricao;
            if (rdbEtiqueta_Adesiva.IsChecked.HasValue)
                descricao = "ADESIVA";
            else if (rdbEtiqueta_Cartao.IsChecked.HasValue)
                descricao = "CARTAO";
            else
                return false;

            try
            {
                using FileStream fileStream = new FileStream($"MODELO-{txtarquivo.Text}-{txtModelo.Text}.txt", FileMode.OpenOrCreate, FileAccess.Write);
                using StreamWriter streamWriter = new StreamWriter(fileStream);

                streamWriter.WriteLine($"INSERT modeloetiqueta(CODIGO, MODELO, DESCRICAO, CUSTOMIZADA, ALTURA, LARGURA, DENSIDADE, VELOCIDADE, ETIQUETA, COLUNAS, GAP, TIPOETIQUETA) VALUES({txtCodigo.Text}, {txtModelo.Text}, N'ETIQUETA {descricao} - DIMENSOES {txtLargura.Text}x{txtAltura.Text}x{txtqtdeColunas.Text} - PADRAO', 1, {int.Parse(txtAltura.Text) * 8}, {int.Parse(txtLargura.Text) * 8 * int.Parse(txtqtdeColunas.Text)}, 10, 3, N'[CONFIGURACAO]");
                streamWriter.WriteLine("T30\nT16\nT16\nLPPLB\n[CABECALHO]\nN\nJB\nN\nO\nZB\nq<LARGURA>\nQ<ALTURA>\n<GAP>\nS<VELOCIDADE>\nD<DENSIDADE>\nN");
                for (int i = 0; i < int.Parse(txtqtdeColunas.Text); i++)
                {
                    streamWriter.WriteLine($"[COLUNA{i + 1}]");

                    foreach (var item in lsttag.Items)
                    {

                        if (item.ToString()?.ToUpper() == "BARRA")
                            streamWriter.WriteLine($"BAjusteX({frmEixo.txteixoy.Text}),AjusteY({frmEixo.txteixoy.Text}),0,E30,{frmEixo.txtmultVertical.Text},{frmEixo.txtmultHorizontal.Text},64,B,\"<{item.ToString()?.ToUpper()}>\"");
                        else
                            streamWriter.WriteLine($"AAjusteX({frmEixo.txteixoy.Text}),AjusteY({frmEixo.txteixoy.Text}),0,{frmEixo.txtfonte.Text},{frmEixo.txtmultVertical.Text},{frmEixo.txtmultHorizontal.Text},N,\"<{item.ToString()?.ToUpper()}>\"");
                    }
                }

                var teste = fileStream.Seek(fileStream.Position, SeekOrigin.Begin);

                streamWriter.Flush();
                fileStream.Seek(-2, SeekOrigin.Current);
                streamWriter.WriteLine($"', {txtqtdeColunas.Text}, {txtgap.Text}, 1)");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }

            return true;

        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrEmpty(txtCodigo.Text) || string.IsNullOrEmpty(txtModelo.Text) || string.IsNullOrEmpty(txtAltura.Text) || string.IsNullOrEmpty(txtLargura.Text) || string.IsNullOrEmpty(txtqtdeColunas.Text) || string.IsNullOrEmpty(txtgap.Text))
            {
                lblStatus.Content = null;
                lblStatus.Content = "Campo não pode ser vazio";
                lblStatus.Foreground = Brushes.Red;
                return true;
            }
            else
            {
                return false;
            }
        }

        private void bntLimparCampos_Click(object sender, EventArgs e)
        {
            txtCodigo.Text = "";
            txtModelo.Text = "";
            txtLargura.Text = "";
            txtAltura.Text = "";
            txtgap.Text = "";
            //txtqtdeColunas = "";
            txtarquivo.Text = "";
            txtCodigo.Text = "";
            lsttag.Items.Clear();
           
            }

    }
}
