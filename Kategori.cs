using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ADONETNEDİR
{
    public partial class Kategori : Form
    {
        public Kategori()
        {
            InitializeComponent();
        }
        
        private void Kategori_Load(object sender, EventArgs e)
        {
            SqlDataAdapter adapter = new SqlDataAdapter("Select * from Kategoriler", connection);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dtvKategori.DataSource = dt;
            connection.Open();
            connection.Close();

        }
        SqlConnection connection = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Northwind;Integrated Security=True");
       
       
    }
}
