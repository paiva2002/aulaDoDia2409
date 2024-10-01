using ReaLTaiizor.Controls;
using ReaLTaiizor.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aulaDoDia2409
{
    public partial class FormCadastroAluno : MaterialForm
    {
        string alunosFileName = "alunos.txt";
        bool isAlteracao = false;
        int indexSelecionado = 0;
        public FormCadastroAluno()
        {
            InitializeComponent();
        }

        private void txtSalvar_Click(object sender, EventArgs e)
        {
            if (ValidaFormulario())//faz a validação
            {
                Salvar();//chama  ometodo para salvar em arquivo
                //muda para pagina de consulta
                TabControlCadastro.SelectedIndex = 1;
            }
        }

        private bool ValidaFormulario()
        {
            if (string.IsNullOrEmpty(txtMatricula.Text))
            {
                MessageBox.Show("matricula obrigatória!", "IFSP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMatricula.Focus();
                return false;
            }

            if (!DateTime.TryParse(txtDataNascimento.Text, out DateTime date))
            {
                MessageBox.Show("data de nascimento obrigatória!", "IFSP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtDataNascimento.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtNome.Text))
            {
                MessageBox.Show("nome obrigatório!", "IFSP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtNome.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtEndereco.Text))
            {
                MessageBox.Show("endereço obrigatório", "IFSP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtEndereco.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtBairro.Text))
            {
                MessageBox.Show("bairro obrigatório", "IFSP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtBairro.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtCidade.Text))
            {
                MessageBox.Show("cidade obrigatório", "IFSP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCidade.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtSenha.Text))
            {
                MessageBox.Show("senha obrigatório", "IFSP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSenha.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(cboEstado.Text))
            {
                MessageBox.Show("estado obrigatório", "IFSP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cboEstado.Focus();
                return false;
            }

            return true;
        }
        private void Salvar()
        {
            var line = $"{txtMatricula.Text};" +
                       $"{txtDataNascimento.Text};" +
                       $"{txtNome.Text};" +
                       $"{txtEndereco.Text};" +
                       $"{txtBairro};" +
                       $"{txtCidade.Text};" +
                       $"{cboEstado.Text};" +
                       $"{txtSenha};";
            if (!isAlteracao)//novo registro{
            {
                var file = new StreamWriter(alunosFileName, true);
                file.WriteLine(line);
                file.Close();
            }
            else//alteração
            {
                //implementar alteração
                string[] alunos = File.ReadAllLines(alunosFileName);
                alunos[indexSelecionado] = line;
                File.WriteAllLines(alunosFileName, alunos);
            }
            LimpaCampos();
        }
        private void LimpaCampos()
        {
            isAlteracao = false;
            foreach (var control in tabPageCadastro.Controls)
            {
                if (control is MaterialTextBoxEdit)
                {
                    ((MaterialTextBoxEdit)control).Clear();
                }
                if (control is MaterialMaskedTextBox)
                {
                    ((MaterialMaskedTextBox)control).Clear();
                }
            }
        }
        private void CarregaListView()
        {
            Cursor.Current = Cursors.WaitCursor;
            mlvAlunos.Columns.Clear();
            mlvAlunos.Items.Clear();
            mlvAlunos.Columns.Add("Matricula");
            mlvAlunos.Columns.Add("Data de nasc");
            mlvAlunos.Columns.Add("nome");
            mlvAlunos.Columns.Add("endereço");
            mlvAlunos.Columns.Add("bairro");
            mlvAlunos.Columns.Add("cidade");
            mlvAlunos.Columns.Add("uf");

            mlvAlunos.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            string[] alunos = File.ReadAllLines(alunosFileName);

            foreach (string aluno in alunos)
            {
                var campos = aluno.Split(';');
                mlvAlunos.Items.Add(new ListViewItem(campos));
            }
            mlvAlunos.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

            Cursor.Current = Cursors.Default;
        }

        private void tabPageConsulta_Enter(object sender, EventArgs e)
        {
            CarregaListView();
        }
        private void Editar()
        {
            if (mlvAlunos.SelectedIndices.Count > 0)
            {
                indexSelecionado = mlvAlunos.SelectedItems[0].Index;
                isAlteracao = true;
                var item = mlvAlunos.SelectedItems[0];
                txtMatricula.Text = item.SubItems[0].Text;
                txtDataNascimento.Text = item.SubItems[1].Text;
                txtNome.Text = item.SubItems[2].Text;
                txtEndereco.Text = item.SubItems[3].Text;
                txtBairro.Text = item.SubItems[4].Text;
                txtCidade.Text = item.SubItems[5].Text;
                cboEstado.Text = item.SubItems[6].Text;
                TabControlCadastro.SelectedIndex = 0;
                txtMatricula.Focus();

            }

        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            Editar();
        }

        private void mlvAlunos_DoubleClick(object sender, EventArgs e)
        {
            Editar();
        }



        /*private void txtNome_Click(object sender, EventArgs e)
        {

        }

        private void txtBairro_Click(object sender, EventArgs e)
        {

        }*/
    }
}
