using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        SqlConnection conexao = new SqlConnection(@"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=meubanco;Data Source=.\SQLEXPRESS");

        public Form1()
        {
            InitializeComponent();
        }

        private void txtNome_TextChanged(object sender, EventArgs e)
        {

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            CarregarDados();
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            if (!Valida_campo())
                return;
            string sql = "insert into agendacs (nome, contato, email) values (@nome, @contato, @email)";
            SqlCommand comando = new SqlCommand(sql, conexao);
            conexao.Open();
            comando.Parameters.AddWithValue("@nome", txtNome.Text);
            comando.Parameters.AddWithValue("@contato", txtTelefone.Text);
            comando.Parameters.AddWithValue("@email", txtEmail.Text);
            comando.ExecuteNonQuery();
            MessageBox.Show("Criado", "Salvar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            conexao.Close();
            CarregarDados();
            Limpar();


        }
        private void CarregarDados()
        {
            conexao.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter dados = new SqlDataAdapter("select * from agendacs", conexao);
            dados.Fill(dt);
            dgvResultado.DataSource = dt;
            conexao.Close();

        }

        private void dgvResultado_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvResultado_Click(object sender, EventArgs e)
        {
            txtNome.Text = dgvResultado.CurrentRow.Cells[0].Value.ToString();
            txtTelefone.Text = dgvResultado.CurrentRow.Cells[1].Value.ToString();
            txtEmail.Text = dgvResultado.CurrentRow.Cells[2].Value.ToString();
        }

        private void dgvResultados_DoubleClick(object sender, EventArgs e)
        {




        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (Valida_campo())
            {

                string sql = "update agendacs set contato = @contato, email = @email where nome = @nome";
                SqlCommand comando = new SqlCommand(sql, conexao);
                conexao.Open();
                comando.Parameters.AddWithValue("@nome", txtNome.Text);
                comando.Parameters.AddWithValue("@contato", txtTelefone.Text);
                comando.Parameters.AddWithValue("@email", txtEmail.Text);
                comando.ExecuteNonQuery();
                MessageBox.Show("Deseja editar esse contato", "Editar", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                conexao.Close();
                CarregarDados();
                Limpar();
            }
        }
        private void Limpar()
        {

            txtNome.Clear();
            txtTelefone.Clear();
            txtEmail.Clear();

        }

        private void btnApagar_Click(object sender, EventArgs e)
        {


            if (MessageBox.Show("Deseja apagar?", "Apagar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

                SqlCommand delete = new SqlCommand("delete from agendacs where nome = @nome", conexao);
                conexao.Open();
                delete.Parameters.AddWithValue("@nome", dgvResultado.CurrentRow.Cells[0].FormattedValue);
                delete.ExecuteNonQuery();
                conexao.Close();
                CarregarDados();


            }
            Limpar();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            conexao.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter dados = new SqlDataAdapter($"select * from agendacs where nome like '{textBox1.Text}%'", conexao);
            dados.Fill(dt);
            dgvResultado.DataSource = dt;
            conexao.Close();

        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            if (!(MessageBox.Show("Deseja sair mesmo", "sair", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No))
                this.Close();
        }
        private bool Valida_campo()
        {
            if (string.IsNullOrEmpty(txtNome.Text) && string.IsNullOrEmpty(txtTelefone.Text) && string.IsNullOrEmpty(txtEmail.Text))
            {
                MessageBox.Show("Todos campos são obrigatórios", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;

            }
            return true;
        }
    }
}

