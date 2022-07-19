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
    public partial class MainWindow : Window
    {

        private void ButtonAddName_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txttag.Text) && !lsttag.Items.Contains(txttag.Text))
            {
                lsttag.Items.Add(txttag.Text.ToUpper());
                txttag.Clear();
            }
        }

        private void btnSalvarArquivo_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarCampos())
            {
                return;
            }


            string descricao;
            if (rdbEtiqueta_Adesiva.IsChecked.Value)
                descricao = "ADESIVA";
            else
                descricao = "CARTAO";
            //using (StreamWriter streamWriter = new StreamWriter($"Modelo{txtNomeLoja.Text}.txt"))
            using FileStream fileStream = new FileStream($"MODELO-{txtarquivo.Text}.txt", FileMode.OpenOrCreate, FileAccess.Write);
            using StreamWriter streamWriter = new StreamWriter(fileStream);

            streamWriter.WriteLine($"INSERT modeloetiqueta(CODIGO, MODELO, DESCRICAO, CUSTOMIZADA, ALTURA, LARGURA, DENSIDADE, VELOCIDADE, ETIQUETA, COLUNAS, GAP, TIPOETIQUETA) VALUES({txtCodigo.Text}, {txtModelo.Text}, N'ETIQUETA {descricao} - DIMENSOES {txtAltura.Text}x{txtLargura.Text}x{txtqtdeColunas.Text} - PADRAO', 1, {int.Parse(txtAltura.Text) * 8}, {int.Parse(txtLargura.Text) * 8 * int.Parse(txtqtdeColunas.Text)}, 10, 3, N'[CONFIGURACAO]");
            streamWriter.WriteLine("T30\nT16\nT16\nLPPLB\n[CABECALHO]\nN\nJB\nN\nO\nZB\nq<LARGURA>\nQ<ALTURA>\n<GAP>\nS<VELOCIDADE>\nD<DENSIDADE>\nN");
            for (int i = 0; i < int.Parse(txtqtdeColunas.Text); i++)
            {
                streamWriter.WriteLine($"[COLUNA{i + 1}]");

                foreach (var item in lsttag.Items)
                    streamWriter.WriteLine($"AAjusteX(0),AjusteY(0),0,1,1,2,N,\"<{item.ToString()?.ToUpper()}>\"");
            }

            var teste = fileStream.Seek(fileStream.Position, SeekOrigin.Begin);

            ////fileStream.w
            //streamWriter.WriteLine($"', 2, {gap.Text}, 1");

            lblStatus.Content = null;
            lblStatus.Content = "Salvo com sucesso!";
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrEmpty(txtCodigo.Text) || string.IsNullOrEmpty(txtModelo.Text) != null || string.IsNullOrEmpty(txtAltura.Text)  || string.IsNullOrEmpty(txtLargura.Text) || string.IsNullOrEmpty(txtqtdeColunas.Text))
            {
                lblStatus.Content = null;
                lblStatus.Content = "Campo não pode ser vazio";
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
