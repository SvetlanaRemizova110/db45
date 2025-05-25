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
    public partial class ViewPartners : Form
    {
        public ViewPartners()
        {
            InitializeComponent();
        }

        private void ViewPartners_Load(object sender, EventArgs e)
        {
            DataTable partnerDis = AddDiscount(Fill());
            dataGridView1.DataSource = partnerDis;
            dataGridView1.Columns["Полная стоимость"].Visible = false;
            dataGridView1.Columns["id"].Visible = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToAddRows = false;
        }
        
        public DataTable Fill()
        {
            string conStr = "host=localhost;uid=root;pwd=;database=mydb;";
            MySqlConnection con = new MySqlConnection(conStr);
            string query = "SELECT concat_ws(' ', TypePartners.TypePartnersName, '|',  PartnersName) " +
            " AS 'Тип | Наименование партнера', PartnersDirector AS 'Директор'," +
            "Partners.idPartners AS 'id', PartnersPhone As 'Телефон', PartnersRating AS 'Рейтинг'," +
            "SUM(IFNULL(Partner_Product.Partner_ProductCount, 0)) AS 'Полная стоимость'" +
            " FROM Partners " +
            " left join Partner_Product ON Partner_Product.Partner_ProductPartnerName = Partners.idPartners " +
            " inner join TypePartners ON TypePartners.idTypePartners = Partners.PartnersType" +
            " Group by Partners.idPartners; ";
            MySqlDataAdapter da = new MySqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        
        public DataTable AddDiscount(DataTable partnersData)
        {
            partnersData.Columns.Add("Скидка", typeof(int));
            foreach(DataRow row in partnersData.Rows)
            {
                double totalSales = Convert.ToDouble(row["Полная стоимость"]);
                Proc pr = new Proc();
                double discount = pr.Procc(totalSales);
                row["Скидка"] = discount;
            }
            return partnersData;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > 0)
            {
                DialogResult dr = MessageBox.Show("Перейти к редактированию?", "Редактирование", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes)
                {
                    int ed = dataGridView1.CurrentCell.RowIndex;
                    int id = Convert.ToInt32(dataGridView1.Rows[ed].Cells["id"].Value);
                    add_edit ae = new add_edit(id);
                    this.Hide();
                    ae.Show();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            add_edit ae = new add_edit();
            this.Hide();
            ae.Show();
        }

        private void historyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ed = dataGridView1.CurrentCell.RowIndex;
            int id = Convert.ToInt32(dataGridView1.Rows[ed].Cells["id"].Value);
            history h = new history(id);
            this.Hide();
            h.Show();
        }
    }
}
