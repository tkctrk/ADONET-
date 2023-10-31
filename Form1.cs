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

namespace ADONETNEDİR
{
   
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        

        SqlConnection connection = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Northwind;Integrated Security=True");

        private void Form1_Load(object sender, EventArgs e)
        {
            //orm : object relational mapping : veriye erişim ıcın kullanılır. bır cok yontem vardır burada amacımız dbdekı detayı ılgılı projemıze aktarmaktadır.
          
         
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Urunler",connection);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource= dt;
            dataGridView1.Columns["KategoriID"].Visible = false;
            dataGridView1.Columns["TedarikciID"].Visible = false;
            dataGridView1.Columns["UrunID"].Visible = false;
            dataGridView1.Columns["Sonlandi"].Visible = false;
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            //3 ADET DEGISKEN TANIMLADIK KULLANICIDAN BILGI ALMAK ICIN KULLANICDAN ALMIS OLDUGUMUZ BILGILERI URUN BILGISI OLARAK DB EKLEYECEGIZ
            string urunAdi = txtUrunadi.Text;
            decimal fiyat = nudFiyat.Value;
            short stok = Convert.ToInt16(nudStok.Value);//BURADA SAYISAL BIR DEGER OLDUGU ICIN GELEN VALUE ALIYORUZ.
            if (urunAdi==""||fiyat ==0 || stok==0)// EGER ÜRÜN BILGISI YAZZILMADAN GONDERILIYORSA BUNU DENETLEDIK VE ONUNE GECTIK BOŞ YADA 0 DEGER KABUL ETMEYECEGIZ.
            {
                MessageBox.Show("Tüm Alanları Doldurunuz");
            }
            //BURADA SQK KOMUTU YAZACAGIMIZ ICIN SQLCOMMAND ISTANCE URETTIK
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = string.Format("INSERT URUNLER (UrunAdi,BirimFiyati,HedefStokDuzeyi) values ('{0}',{1},{2})",urunAdi,fiyat,stok);

            ///COMMANDTEXT : ICERISINDE SQL KOMUTUU YAZDIK CUNKU SQLDE CALISACAK BU KOMUT VE STRING.FORMAT KULLANDIK CUNKU SQL KOMUTU ICERISINDE {} İNDEX MANTIGI ILE KODUMUZU YAZDIK BUNDAN DOLAYI ILERLEYEN ZAMANLARDA VE BU BIZE BIR SORUN YARATMAYACAKTIR.

            cmd.Connection = connection;//BURADA ISE SQLE BAGLANDIK
            connection.Open();//BAGLANTIYI ACTIK
            int etki = cmd.ExecuteNonQuery();//BURADA ISE YAZMIS OLDUGUMUZ SQL KOMUTUNU SQLDE CALISTIRDIK SQLDE VAR OLAN EXECUTE TUSUNA ES DEGERDIR.EGER SQL DE BIZE 1 ROW EFFECTED DIYE BIR DEGER DONERSE SQLDE YAZMIS OLDUGUMUZ KOD BASARILI BIR SEKILDE OLUSTURULMUSTUR.

            if (etki>0)
            {
                MessageBox.Show("Urun Basarılı Bır sekılde Eklenmıstır");
                //EGER URUNU BIZ GORMEK ISTIYORSAK DATAGRIDVIEW ICERISINDE YUKARDA YAZMIS OLDUMUZ URUNLISTELE CAGIRMALIYIZ TEKRARDAN.
            }
            else
            {
                MessageBox.Show("Bir Hata Oluştu");
            }

            connection.Close();





        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
           

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = string.Format("update Urunler set UrunAdi='{0}',BirimFiyati={1},HedefStokDuzeyi={2} WHERE UrunID={3}", txtUrunadi.Text, nudFiyat.Value.ToString().Replace(",",""), nudStok.Value.ToString().Replace(",",""), txtUrunadi.Tag);
            cmd.Connection = connection;
            connection.Open();
            int etki = cmd.ExecuteNonQuery();
            RowAffect(cmd);
            connection.Close();


        }
        private void RowAffect(SqlCommand cmd)
        {
            int etki = cmd.ExecuteNonQuery();
            if (etki > 0)
            {
                MessageBox.Show($"{etki} row affected. Ürün başarılı bir şekilde eklenmiştir");


            }
            else
            {
                MessageBox.Show("Bir hata oluştu");
            }
        }

            private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var row = dataGridView1.CurrentRow;
            txtUrunadi.Text = row.Cells["UrunAdi"].Value.ToString();
            txtUrunadi.Tag = row.Cells["UrunID"].Value.ToString();
            nudFiyat.Value = (decimal)row.Cells["BirimFiyati"].Value;
            nudStok.Value = (short)row.Cells["HedefStokDuzeyi"].Value;


        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow!=null)
            {
                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["UrunID"].Value);
                SqlCommand cmd = new SqlCommand(string.Format("delete from Urunler where UrunID={0}",id),connection);
                connection.Open();
                int etki = cmd.ExecuteNonQuery();
                if (etki>0)
                {
                    MessageBox.Show("Ürün Silinmiştir");
                }
                else
                {
                    MessageBox.Show("Bir Hata Olustu..");
                }

                connection.Close();
            }
        }

        private void btnKategori_Click(object sender, EventArgs e) 
        {
            this.Hide();
            Kategori kt = new Kategori();
            kt.ShowDialog();
            connection.Open();
            connection.Close() ;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            connection.Open();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType= CommandType.Text;
            cmd.CommandText = "select * from Urunler where UrunAdi like'%"+textBox1.Text+"%'";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
            connection.Close();
            


        }
    }
}
