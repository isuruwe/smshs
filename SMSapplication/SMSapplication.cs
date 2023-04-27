/*
 * Created by: Syeda Anila Nusrat. 
 * Date: 1st August 2009
 * Time: 2:54 PM 
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace SMSapplication
{
    public partial class SMSapplication : Form
    {

        #region Constructor
        public SMSapplication()
        {
            InitializeComponent();
        }
        #endregion
        SqlConnection oSqlConnection;
        SqlCommand oSqlCommand;
        SqlDataAdapter oSqlDataAdapter;
        public string sqlQuery;
        public string UserValue ="";
        #region Private Variables
        SerialPort port = new SerialPort();
        clsSMS objclsSMS = new clsSMS();
        ShortMessageCollection objShortMessageCollection = new ShortMessageCollection();
        #endregion

        #region Private Methods

        #region Write StatusBar
        private void WriteStatusBar(string status)
        {
            try
            {
                statusBar1.Text = "Message: " + status;
            }
            catch (Exception ex)
            {
                
            }
        }
        #endregion

        #endregion

        #region Private Events


        private void LoadPersonal()
        {
            DateTime batchdate = new DateTime();
            DataSet odsLoadLoanAppliedPersonal = new DataSet();
            odsLoadLoanAppliedPersonal.Clear();
            dataGridView1.Rows.Clear();
            DataSet odsvoltxndata = new DataSet();

           
               String strDBCon = "Data Source = 135.22.210.105; Initial Catalog = MMS; User ID = mmsuser; Password = password;";
                oSqlConnection = new SqlConnection(strDBCon);
                oSqlCommand = new SqlCommand();
                sqlQuery = "SELECT * FROM[MMS].[dbo].[Lab_sms] where status='0'";

                oSqlCommand.Connection = oSqlConnection;
                oSqlCommand.CommandText = sqlQuery;
               
                oSqlDataAdapter = new SqlDataAdapter(oSqlCommand);
                oSqlConnection.Open();
                oSqlDataAdapter.Fill(odsLoadLoanAppliedPersonal);
                oSqlConnection.Close();
            if (odsLoadLoanAppliedPersonal.Tables[0].Rows.Count > 0)
            {
                for (int count = 0; count < odsLoadLoanAppliedPersonal.Tables[0].Rows.Count; count++)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[count].Cells["smsid"].Value = odsLoadLoanAppliedPersonal.Tables[0].Rows[count]["smsid"].ToString();
                    dataGridView1.Rows[count].Cells["phone"].Value = odsLoadLoanAppliedPersonal.Tables[0].Rows[count]["phoneno"].ToString();
                    dataGridView1.Rows[count].Cells["msg"].Value = odsLoadLoanAppliedPersonal.Tables[0].Rows[count]["massegetext"].ToString();

                   
                }
            }


        }


        private void SMSapplication_Load(object sender, EventArgs e)
        {
            LoadPersonal();
            try
            {
                #region Display all available COM Ports
                string[] ports = SerialPort.GetPortNames();

                // Add all port names to the combo box:
                foreach (string port in ports)
                {
                    this.comboBox1.Items.Add(port);
                }
                #endregion

                //Remove tab pages
              //  this.tabSMSapplication.TabPages.Remove(tbSendSMS);
              

                this.btnDisconnect.Enabled = false;
            }
            catch(Exception ex)
            {
                ErrorLog(ex.Message);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
           
        }
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                this.comboBox1.Enabled = true;
                objclsSMS.ClosePort(this.port);

                //Remove tab pages
              
              

                this.lblConnectionStatus.Text = "Not Connected";
                this.btnDisconnect.Enabled = false;

            }
            catch (Exception ex)
            {
                ErrorLog(ex.Message);
            }
        }

        private void btnSendSMS_Click(object sender, EventArgs e)
        {

            //.............................................. Send SMS ....................................................
           
        }
    
        #endregion

        #region Error Log
        public void ErrorLog(string Message)
        {
            StreamWriter sw = null;

            try
            {
                WriteStatusBar(Message);

                string sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";
                //string sPathName = @"E:\";
                string sPathName = @"SMSapplicationErrorLog_";

                string sYear = DateTime.Now.Year.ToString();
                string sMonth = DateTime.Now.Month.ToString();
                string sDay = DateTime.Now.Day.ToString();

                string sErrorTime = sDay + "-" + sMonth + "-" + sYear;

                sw = new StreamWriter(sPathName + sErrorTime + ".txt", true);

                sw.WriteLine(sLogFormat + Message);
                sw.Flush();

            }
            catch (Exception ex)
            {
                //ErrorLog(ex.ToString());
            }
            finally
            {
                if (sw != null)
                {
                    sw.Dispose();
                    sw.Close();
                }
            }
            
        }
        #endregion 

        private void button1_Click(object sender, EventArgs e)
        {
            

            ShortMessageCollection sss;

            //sss = objclsSMS.ReadALLSMS(this.port);

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            try
            {
               // bool value = objclsSMS.sendMsg(this.port, this.txtSIM.Text, this.txtMessage.Text);
                //if (value)
                //{
                //    //MessageBox.Show("Message has sent successfully");
                //    this.statusBar1.Text = "Message has sent successfully";
                //}
                //else
                //{
                //    //MessageBox.Show("Failed to send message");
                //    this.statusBar1.Text = "Failed to send message";
                //}

            }
            catch (Exception ex)
            {
                //this.port.Close();
                ErrorLog(ex.Message);
            }
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            try
            {
                //Open communication port 
                this.port = objclsSMS.OpenPort(this.comboBox1.Text, 0, 0, 0, 0);

                if (this.port != null)
                {
                    this.comboBox1.Enabled = false;

                    //MessageBox.Show("Modem is connected at PORT " + this.cboPortName.Text);
                    this.statusBar1.Text = "Modem is connected at PORT " + this.comboBox1.Text;

                    //Add tab pages


                    this.lblConnectionStatus.Text = "Connected at " + this.comboBox1.Text;
                    this.btnDisconnect.Enabled = true;
                }

                else
                {
                    //MessageBox.Show("Invalid port settings");
                    this.statusBar1.Text = "Invalid port settings";
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex.Message);
            }
        }

        private void btnSendSMS_Click_1(object sender, EventArgs e)
        {
            try
            {
                //bool value = objclsSMS.sendMsg(this.port, this.txtSIM.Text, this.txtMessage.Text);
                //if (value)
                //{
                //    //MessageBox.Show("Message has sent successfully");
                //    this.statusBar1.Text = "Message has sent successfully";
                //}
                //else
                //{
                //    //MessageBox.Show("Failed to send message");
                //    this.statusBar1.Text = "Failed to send message";
                //}

            }
            catch (Exception ex)
            {
                //this.port.Close();
                ErrorLog(ex.Message);
            }
        }
        static IEnumerable<string> ChunksUpto(string str, int maxChunkSize)
        {
            for (int i = 0; i < str.Length; i += maxChunkSize)
                yield return str.Substring(i, Math.Min(maxChunkSize, str.Length - i));
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {try
            {
                var senderGrid = (DataGridView)sender;
                if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
                {
                    string p1, p2;
                    
                    string[] p3 = new string[100];
                    bool value=false;
                    string sdf = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["msg"].Value.ToString();
                  int msdf= sdf.Length;
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < sdf.Length; i++)
                    {
                        if (i % 100 == 0)
                            sb.Append('~');
                        sb.Append(sdf[i]);
                    }
                    string formatted = sb.ToString();
                   p3= formatted.Split('~');

                    var temp = new List<string>();
                    foreach (var s in p3)
                    {
                        if (!string.IsNullOrEmpty(s))
                            temp.Add(s);
                    }
                    p3 = temp.ToArray();
                    for (int x = 0; x < p3.Length; x++)
                    {
                        p2 = p3[x];
                        value = objclsSMS.sendMsg(this.port, dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["phone"].Value.ToString(), p2);

                    }

                    

                   
                    if (value)
                    {
                        

                        String strDBCon = "Data Source = 135.22.210.105; Initial Catalog = MMS; User ID = mmsuser; Password = password;";
                        oSqlConnection = new SqlConnection(strDBCon);
                        oSqlCommand = new SqlCommand();
                        sqlQuery = "UPDATE  [MMS].[dbo].[Lab_sms] SET senttime=GETDATE(), status='1' where smsid='" + dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["smsid"].Value.ToString() + "'  ";

                        oSqlCommand.Connection = oSqlConnection;
                        oSqlCommand.CommandText = sqlQuery;

                        oSqlDataAdapter = new SqlDataAdapter(oSqlCommand);
                        oSqlConnection.Open();
                        oSqlCommand.ExecuteNonQuery();
                        oSqlConnection.Close();
                        LoadPersonal();
                        this.statusBar1.Text = "Message has sent successfully";
                        MessageBox.Show("Message has sent successfully");
                    }
                    else
                    {
                        MessageBox.Show("Failed to send message");
                        this.statusBar1.Text = "Failed to send message";
                    }

                }
            }catch(Exception ex)
            {

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadPersonal();
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            login oform = new login();
            oform.ShowDialog();
            UserValue = oform.UserValue;
            EnebleMenues(UserValue);
        }

        public void EnebleMenues(string usevalue)
        {
            if (UserValue == "R001")
            {
                button1.Enabled = true;
                dataGridView1.Enabled = true;
                button2.Enabled = true;
                btnDisconnect.Enabled = true;
            }
        }



            }
}