using gambit.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Xml.Serialization;

namespace gambit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmGambit : Window
    {
        frmEixo frmEixo = new();
        EtiquetaConfig etiquetaConfig = new();
        private string nomeArquivo;


        //public frmGambit()
        //{
        //    nomeArquivo = $"MODELO-{txtarquivo.Text}-{txtModelo.Text}";
        //}

        private void ButtonAddName_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txttag.Text) && !lsttag.Items.Contains(txttag.Text))
            {
                lsttag.Items.Add(txttag.Text.ToUpper());

               
                //frmEixo.Owner = this;
                frmEixo.Show();

                var param = frmEixo.Parametros;


                txttag.Clear();
            }
        }

        public void btnSalvarArquivo_Click(object sender, RoutedEventArgs e)
        {
            etiquetaConfig.Codigo = int.Parse(txtCodigo.Text);
            etiquetaConfig.Modelo = int.Parse(txtModelo.Text); 
            etiquetaConfig.Altura = int.Parse(txtAltura.Text);
            etiquetaConfig.Largura = int.Parse(txtLargura.Text);
            etiquetaConfig.Gap = int.Parse(txtgap.Text);
            etiquetaConfig.Colunas = int.Parse(txtqtdeColunas.Text);
            etiquetaConfig.Eixo.EixoX = int.Parse(frmEixo.txteixox.Text);
            etiquetaConfig.Eixo.EixoY = int.Parse(frmEixo.txteixoy.Text);
            etiquetaConfig.Eixo.Fonte = int.Parse(frmEixo.txtfonte.Text);
            etiquetaConfig.Eixo.multVertical = int.Parse(frmEixo.txtmultVertical.Text);
            etiquetaConfig.Eixo.multHorizontal = int.Parse(frmEixo.txtmultHorizontal.Text);



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
                using FileStream fileStream = new FileStream($"{nomeArquivo}.txt", FileMode.OpenOrCreate, FileAccess.Write);
                using StreamWriter streamWriter = new StreamWriter(fileStream);

                streamWriter.WriteLine($"INSERT modeloetiqueta(CODIGO, MODELO, DESCRICAO, CUSTOMIZADA, ALTURA, LARGURA, DENSIDADE, VELOCIDADE, ETIQUETA, COLUNAS, GAP, TIPOETIQUETA) VALUES({etiquetaConfig.Codigo}, {txtModelo.Text}, N'ETIQUETA {descricao} - DIMENSOES {etiquetaConfig.Largura}x{etiquetaConfig.Altura}x{etiquetaConfig.Colunas} - PADRAO', 1, {etiquetaConfig.Altura * 8}, {etiquetaConfig.Largura * 8 * etiquetaConfig.Colunas} , 10, 3, N'[CONFIGURACAO]");
                streamWriter.WriteLine("T30\nT16\nT16\nLPPLB\n[CABECALHO]\nN\nJB\nN\nO\nZB\nq<LARGURA>\nQ<ALTURA>\n<GAP>\nS<VELOCIDADE>\nD<DENSIDADE>\nN");
                for (int i = 0; i < (etiquetaConfig.Colunas); i++)
                {
                    streamWriter.WriteLine($"[COLUNA{i + 1}]");

                    foreach (var item in lsttag.Items)
                    {

                        if (item.ToString()?.ToUpper() == "BARRA")
                            streamWriter.WriteLine($"BAjusteX({etiquetaConfig.Eixo.EixoX}),AjusteY({etiquetaConfig.Eixo.EixoY}),0,E30,{etiquetaConfig.Eixo.multVertical},{etiquetaConfig.Eixo.multHorizontal},64,B,\"<{item.ToString()?.ToUpper()}>\"");
                        else
                            streamWriter.WriteLine($"AAjusteX({etiquetaConfig.Eixo.EixoX}),AjusteY({etiquetaConfig.Eixo.EixoY}),0,{etiquetaConfig.Eixo.Fonte},{etiquetaConfig.Eixo.multVertical},{etiquetaConfig.Eixo.multHorizontal},N,\" <{item.ToString()?.ToUpper()}>\"");
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
            txtqtdeColunas.Text = "";
            txtarquivo.Text = "";
            txtCodigo.Text = "";
            lsttag.Items.Clear();
           
        }

        private void BtnImportar_Click(object sender, RoutedEventArgs e)
        {
            //var serializer = new XmlSerializer(typeof(EtiquetaConfig));
            //using var streamWriter = new StreamWriter($"{nomeArquivo}.xml");
            //serializer.Serialize(streamWriter, etiquetaConfig);

            //var serializer = new XmlSerializer(typeof(EtiquetaConfig));
            //using var streamReader = new StreamReader($"{nomeArquivo}.xml");
            //etiquetaConfig = (EtiquetaConfig)serializer?.Deserialize(streamReader);
        }

        private void ConsiderarNum(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
