using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dapper;
using System.Data.OleDb;
using DataMahasiswa.model;

namespace DataMahasiswa
{
    public partial class Form1 : Form
    {
        OleDbConnection cone = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=E:\IC\DataMahasiswa\academik.mdb;User Id=admin;
Password =;");
        BindingSource bind = new BindingSource();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            load();
        }
        private void load()
        {
            try
            {
                List<CLS_Mhs> data = cone.Query<CLS_Mhs>("select * from mahasiswa").ToList();
                bind.DataSource = data;
                dataGrid.DataSource = bind;
            }
            catch (Exception a)
            {
                MessageBox.Show(a.Message);
            }

        }
        private void empt(){
            txtNama.Text = String.Empty;
            txtNim.Text = String.Empty;
            txtAlamat.Text = String.Empty;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                cone.Execute("insert into mahasiswa(nim,nama,alamat) values(@id,@name,@addrs)", new { id = txtNim.Text, name = txtNama.Text, addrs = txtAlamat.Text });
                load();
                empt();
                MessageBox.Show("Input Berhasil");
            }
            catch(Exception a)
            {
                MessageBox.Show(a.Message);
            }
            
        }

        private void dataGrid_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                txtNim.Text = dataGrid.CurrentRow.Cells[0].Value.ToString();
                txtNama.Text = dataGrid.CurrentRow.Cells[1].Value.ToString();
                txtAlamat.Text = dataGrid.CurrentRow.Cells[2].Value.ToString();
            }
            catch(Exception a)
            {
                MessageBox.Show(a.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            DialogResult hasil = MessageBox.Show("yakin ingin memperbaiki data?", "Peringatan", MessageBoxButtons.YesNo);
            if (hasil == DialogResult.Yes)
            {
                try
                {
                   
                    cone.Execute("update mahasiswa set nim = @id, nama = @name, alamat = @addrs where nim = @id", new { id = txtNim.Text, name = txtNama.Text, addrs = txtAlamat.Text });
                    load();
                    MessageBox.Show("Edit berhasil");
                }
                catch (Exception a)
                {
                    MessageBox.Show(a.Message);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult hasil = MessageBox.Show("yakin ingin menghapus data "+txtNama.Text+"?", "peringatan", MessageBoxButtons.YesNo);
            if (hasil == DialogResult.Yes)
            {
                cone.Execute("delete from mahasiswa where nim = @id", new { id = txtNim.Text });
                load();
                empt();
                MessageBox.Show("Berhasil dihapus");

            }
        }

        private void btnCari2_Click(object sender, EventArgs e)
        {
            int se = txtNS.Text.Length;
            List<CLS_Mhs> data = cone.Query<CLS_Mhs>("select * from mahasiswa where left (nama,@s) = @name", new { s = se, name = txtNS.Text }).ToList();
            bind.DataSource = data;
            dataGrid.DataSource = bind;
        }

        private void btnCari_Click(object sender, EventArgs e)
        {
            int se = txtNim.Text.Length;
            List<CLS_Mhs> data = cone.Query<CLS_Mhs>("select * from mahasiswa where left (nim,@s) = @id", new { s = se, id = txtNim.Text }).ToList();
            bind.DataSource = data;
            dataGrid.DataSource = bind;
        }
    }
}
