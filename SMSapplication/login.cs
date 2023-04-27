using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMSapplication
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        SqlConnection oSqlConnection;
        SqlCommand oSqlCommand;
        SqlDataAdapter oSqlDataAdapter;
        public string sqlQuery;
        public string UserValue="";

        private void txtUserName_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Return))
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void txtPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Return))
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnLogging_Click(object sender, EventArgs e)
        {
            DataSet odsvoltxndata = new DataSet();
            byte[] encPwd = EncriptPassword(txtPassword.Text);
            String strDBCon = "Data Source = 135.22.210.105; Initial Catalog = MMS; User ID = mmsuser; Password = password;";
            oSqlConnection = new SqlConnection(strDBCon);
            oSqlCommand = new SqlCommand();
            sqlQuery = "Select * from  [MMS].[dbo].[Users]  where UserName='"+txtUserName.Text +"' and Pass=@bn  ";

            oSqlCommand.Connection = oSqlConnection;
            oSqlCommand.CommandText = sqlQuery;
            oSqlCommand.Parameters.Add("@bn", SqlDbType.VarBinary, 8000).Value = encPwd;

            oSqlDataAdapter = new SqlDataAdapter(oSqlCommand);
            oSqlConnection.Open();
            oSqlDataAdapter.Fill(odsvoltxndata);
            oSqlConnection.Close();
            if (odsvoltxndata.Tables[0].Rows.Count > 0)
            {
                UserValue =odsvoltxndata.Tables[0].Rows[0]["RoleID"].ToString();

            }
            if (UserValue== "R001")
            {
                
                this.Close();


            }
           
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void login_Load(object sender, EventArgs e)
        {
            txtUserName.Select();
        }

        public byte[] EncriptPassword(string Passwd)
        {
            MD5CryptoServiceProvider MD5Pass = new MD5CryptoServiceProvider();
            byte[] HashPass;
            UTF8Encoding Encoder = new UTF8Encoding();
            HashPass = MD5Pass.ComputeHash(Encoder.GetBytes(Passwd));
            return HashPass;
        }
    }
}
