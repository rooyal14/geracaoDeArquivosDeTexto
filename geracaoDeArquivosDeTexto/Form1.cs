using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace geracaoDeArquivosDeTexto
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            inicializar();
        }

        public void inicializar()
        {
            dgvRegistros.ColumnCount = 2;
            dgvRegistros.Columns[0].HeaderText = "Nome";
            dgvRegistros.Columns[0].Width = 230;
            dgvRegistros.Columns[1].HeaderText = "Salário";
            dgvRegistros.Columns[1].Width = 67;
        }

        private void btnCriarLinhas_Click(object sender, EventArgs e)
        {
            if (fmQtdFuncionarios.TextLength > 0)
            {
                int numeroFuncionarios = Convert.ToInt16(fmQtdFuncionarios.Text);
                if (numeroFuncionarios < 1)
                {
                    numeroFuncionarios = 1;
                }
                
                int i = 0;
                do
                {
                    var linhaTabela = new DataGridViewRow();
                    linhaTabela.Cells.Add(new DataGridViewTextBoxCell { Value = string.Empty });
                    linhaTabela.Cells.Add(new DataGridViewTextBoxCell { Value = 0 });
                    dgvRegistros.Rows.Add(linhaTabela);
                } while (++i < numeroFuncionarios);

                fmQtdFuncionarios.Enabled = false;
                btnCriarArquivo.Enabled = true;
                btnReiniciar.Enabled = true;
                btnCriarLinhas.Enabled = false;
            }
        }

        private void btnReiniciar_Click(object sender, EventArgs e)
        {
            dgvRegistros.Rows.Clear();
            fmQtdFuncionarios.Text = string.Empty;
            fmQtdFuncionarios.Enabled = true;
            btnCriarArquivo.Enabled = false;
            btnReiniciar.Enabled = false;
            btnCriarLinhas.Enabled = true;
        }

        private void btnCriarArquivo_Click(object sender, EventArgs e)
        {
            /*
            btnCriarArquivo.Text = dgvRegistros.Rows[0].Cells[0].Value.ToString();
            btnReiniciar.Text = dgvRegistros.Rows[1].Cells[0].Value.ToString();
            btnCriarLinhas.Text = dgvRegistros.Rows[2].Cells[0].Value.ToString();
            */

            if (!ValidaDados())
            {
                MessageBox.Show("Os dados possuem problemas.Verifique " +
                    "se não deixou nenhum nome em branco ou se existe um " +
                    "valor correto para os salários de cada um");
            }
            else if (sfdGravarArquivo.ShowDialog() == DialogResult.OK)
            {
                GerarArquivo();
                MessageBox.Show("Arquivo gerado com sucesso");
            }

        }
        private void GerarArquivo()
        {
            StreamWriter wr = new StreamWriter(sfdGravarArquivo.FileName, true);
            for (int j = 0; j < dgvRegistros.Rows.Count; j++)
            {
                wr.WriteLine(dgvRegistros.Rows[j].Cells[0].Value.ToString() + ";" 
                    + dgvRegistros.Rows[j].Cells[1].Value.ToString());
            }
            wr.Close();
        }
        private bool ValidaDados()
        {
            int i = 0;
            bool dadosValidados = true;
            double stringToDouble;
            //System.Windows.Forms.DataGridViewCell.Value.get returned null.
            do
            {

                if (string.IsNullOrWhiteSpace(dgvRegistros.Rows[i].Cells[0].Value.ToString()))
                {
                    dadosValidados = false;
                }
                

                if (!Double.TryParse(dgvRegistros.Rows[i].Cells[1].Value.ToString(), out stringToDouble))
                {
                    dadosValidados = false;
                }

            } while (++i < dgvRegistros.Rows.Count);
            return dadosValidados;
        }

        private void fmQtdFuncionarios_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}