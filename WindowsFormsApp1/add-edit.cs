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
    public partial class add_edit : Form
    {
        string conStr = "host=localhost;uid=root;pwd=;database=mydb;";
        int PartnerID;
        string PartnerTypeID = string.Empty;

        public add_edit()
        {
            InitializeComponent();
            button1.Text = "Добавить";
            FillCombbo();
        }
        public add_edit(int id)
        {
            InitializeComponent();
            button1.Text = "Редактировать";
            this.PartnerID = id;

            FillData();
            FillComboBox();
        }

        private void add_edit_Load(object sender, EventArgs e)
        {

        }
        void FillCombbo()
        {
            MySqlConnection con = new MySqlConnection(conStr);
            con.Open();
            string queryType = "SELECT TypePartnersName FROM TypePartners;";
            MySqlCommand cmd = new MySqlCommand(queryType, con);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                comboBox1.Items.Add(rdr["TypePartnersName"].ToString());
            }
            con.Close();
        }
        void FillComboBox()
        {
            MySqlConnection con = new MySqlConnection(conStr);
            con.Open();
            string queryType = "SELECT idTypePartners, TypePartnersName FROM TypePartners;";
            MySqlDataAdapter ad = new MySqlDataAdapter(queryType,con);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "TypePartnersName";
            comboBox1.ValueMember = "idTypePartners";
            comboBox1.SelectedValue = PartnerTypeID;
            con.Close();
        }
        void FillData()
        {
            MySqlConnection con = new MySqlConnection(conStr);
            con.Open();
            string query = $"SELECT * FROM Partners WHERE idPartners = {PartnerID};";
            MySqlCommand cmd = new MySqlCommand(query,con);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                PartnerTypeID = rdr["PartnersType"].ToString();
                textBoxName.Text = rdr["PartnersName"].ToString();
                textBoxDir.Text = rdr["PartnersDirector"].ToString();
                textBoxEmail.Text = rdr["PartnersEmail"].ToString();
                textBoxNumb.Text = rdr["PartnersPhone"].ToString();
                textBoxAddress.Text = rdr["PartnersAddress"].ToString();
                textBoxRating.Text = rdr["PartnersRating"].ToString();
            }
            con.Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            ViewPartners ae = new ViewPartners();
            this.Hide();
            ae.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Добавить")
            {

                if (Convert.ToInt32(textBoxRating.Text) < 0)
                {
                    MessageBox.Show("Рейтинг должен быть не отрицательным!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Add();
                }
            }
            if (button1.Text == "Редактировать")
            {
                Edit();
            }
        }
        void Add()
        {
            MySqlConnection con = new MySqlConnection(conStr);
            con.Open();
            int typeid;
            string queryType = "SELECT idTypePartners FROM TypePartners WHERE TypePartnersName = @TypePartnersName;";
            using (MySqlCommand cmdType = new MySqlCommand(queryType, con))
            {
                cmdType.Parameters.AddWithValue("@TypePartnersName", comboBox1.Text);
                object res = cmdType.ExecuteScalar();
                typeid = res != null ? Convert.ToInt32(res) : 0;
            }

            MySqlCommand cmd = new MySqlCommand("INSERT Partners (PartnersType, PartnersName, PartnersDirector, " +
                "PartnersEmail, PartnersPhone, PartnersAddress, PartnersRating) " +
                "Values (@PartnersType, @PartnersName, @PartnersDirector, @PartnersEmail, @PartnersPhone, " +
                "@PartnersAddress, @PartnersRating);", con);
            cmd.Parameters.AddWithValue("PartnersType", typeid);
            cmd.Parameters.AddWithValue("PartnersName", textBoxName.Text);
            cmd.Parameters.AddWithValue("PartnersDirector", textBoxDir.Text);
            cmd.Parameters.AddWithValue("PartnersEmail", textBoxEmail.Text);
            cmd.Parameters.AddWithValue("PartnersPhone", textBoxNumb.Text);
            cmd.Parameters.AddWithValue("PartnersAddress", textBoxAddress.Text);

            cmd.Parameters.AddWithValue("PartnersRating", textBoxRating.Text);
            cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("Запись добавлена", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ViewPartners ae = new ViewPartners();
            this.Hide();
            ae.Show();

        }
        void Edit()
        {
            MySqlConnection con = new MySqlConnection(conStr);
            con.Open();
            int typeid;
            string queryType = "SELECT idTypePartners FROM TypePartners WHERE TypePartnersName = @TypePartnersName;";
            using (MySqlCommand cmdType = new MySqlCommand(queryType, con))
            {
                cmdType.Parameters.AddWithValue("@TypePartnersName", comboBox1.Text);
                object res = cmdType.ExecuteScalar();
                typeid = res != null ? Convert.ToInt32(res) : 0;
            }
            MySqlCommand cmd = new MySqlCommand("UPDATE Partners " +
                "SET PartnersType=@PartnersType, " +
                "PartnersName=@PartnersName, " +
                "PartnersDirector=@PartnersDirector, " +
                "PartnersEmail=@PartnersEmail, " +
                "PartnersPhone=@PartnersPhone," +
                "PartnersAddress=@PartnersAddress, " +
                "PartnersRating=@PartnersRating" +
                $" WHERE idPartners={PartnerID}", con);
            cmd.Parameters.AddWithValue("@PartnersType", typeid);
            cmd.Parameters.AddWithValue("@PartnersName", textBoxName.Text);
            cmd.Parameters.AddWithValue("@PartnersDirector", textBoxDir.Text);
            cmd.Parameters.AddWithValue("@PartnersEmail", textBoxEmail.Text);
            cmd.Parameters.AddWithValue("@PartnersPhone", textBoxNumb.Text);
            cmd.Parameters.AddWithValue("@PartnersAddress", textBoxAddress.Text);
            cmd.Parameters.AddWithValue("@PartnersRating", textBoxRating.Text);
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Запись изменена", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ViewPartners ae = new ViewPartners();
            this.Hide();
            ae.Show();
        }
    }
}
