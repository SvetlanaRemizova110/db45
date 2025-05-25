using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp1
{
    public partial class history : Form
    {
        int idPartners;
        public history(int id)
        {
            InitializeComponent();
            this.idPartners = id;
        }

        private void history_Load(object sender, EventArgs e)
        {
            string conStr = "host=localhost;uid=root;pwd=;database=mydb;";
            MySqlConnection con = new MySqlConnection(conStr);
            con.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT Product.ProductName AS 'Продукт', Partners.PartnersName AS 'Партнер', " +
                "Partner_ProductCount AS 'Кол-во', Partner_ProductDate AS 'Дата'"+
             " FROM Partner_Product"+
             " INNER JOIN Product ON Product.ProductArticul = Partner_Product.Partner_ProductProductART"+
             " INNER JOIN Partners ON Partners.idPartners = Partner_Product.Partner_ProductPartnerName"+
             $" WHERE idPartners = {idPartners}; ", con);
            dataGridView1.Columns.Add("Product.ProductName", "Продукт");
            dataGridView1.Columns.Add("Partners.PartnersName", "Партнер");
            dataGridView1.Columns.Add("Partner_ProductCount", "Кол-во");
            dataGridView1.Columns.Add("Partner_ProductDate", "Дата");
            //dataGridView1.Columns[].Visible = false;
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridView1, rdr[0].ToString(),rdr[1].ToString(),rdr[2].ToString(),rdr[3].ToString());
                dataGridView1.Rows.Add(row);
            }
            con.Close();
        }
    }
}
