using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PoolClupControl
{
    public partial class ctrlPool : UserControl
    {
        public class TableCompletedEventArgs : EventArgs { 
            
            public string TimeText { get; }
            public int TimeInSeconds {  get; }
            public int RatePerHour {  get; }
            public float TotalFees {  get; }

            public TableCompletedEventArgs(string timeText, int timeInSeconds, int ratePerHour, float totalFees)
            {
                TimeText = timeText;
                TimeInSeconds = timeInSeconds;
                RatePerHour = ratePerHour;
                TotalFees = totalFees;
            }
        }

        public event  EventHandler<TableCompletedEventArgs> OnTableComplete;

        public void RaiseOnTableComplete(string TimeText, int TimeInSeconds, int RatePerHour, float TotalFees)
        {
            RaiseOnTableComplete(new TableCompletedEventArgs(TimeText, TimeInSeconds, RatePerHour, TotalFees));

        }
        protected virtual void RaiseOnTableComplete(TableCompletedEventArgs e) { 
            OnTableComplete?.Invoke(this, e);
        }

        private string _TablePlayer = "Player";
        [
            Category("Pool Config"), Description("The Playe Name")
        ]
        public string TablePlayer
        {
            get { return _TablePlayer; }
            set { _TablePlayer = value;
                lblGameName.Text = value;
            }
        }

        private float _HourlyRate = 10.00F;

        [
        Category("Pool Config"),
        Description("Rate Per Hour.")
        ]
        public float HourlyRate
        {
            get
            {
                return _HourlyRate;
            }
            set
            {
                _HourlyRate = value;

            }
        }
        public ctrlPool()
        {
            InitializeComponent();
        }

                 int Seconds = 0;
                 int Minutes = 0;
                 int Hours = 0;
        

        private void btnStart_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private bool ConfirmEnd()
        {
            if (MessageBox.Show("Are You sure You want to confirm end!! ", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK) { 
               
                Seconds = 0;
                Minutes = 0;
                Hours = 0;
                timer1.Stop();
                return true;
            }
            return false;
        }

       
        private void btnEnd_Click(object sender, EventArgs e)
        {   int totalSeconds = Hours * 3600 + Minutes * 60 + Seconds;

         
                string timeText = $"{Hours:D2}:{Minutes:D2}:{Seconds:D2}";

                float totalFees = ((float)totalSeconds / 3600) * _HourlyRate;
            if (ConfirmEnd())
            {
               
                lblTime.Text = "0:0:0";
                

                RaiseOnTableComplete(timeText, totalSeconds, (int)_HourlyRate, totalFees);

              
                MessageBox.Show($"Time Consumed = {timeText}, total seconds = {totalSeconds}, Hourly Rate = {_HourlyRate.ToString()}, Total Fees = {totalFees}  ");
            

            }
            
          
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = 1000;
            Seconds++;

            if(Seconds == 60)
            {
                Seconds = 0;
                Minutes++;

            }
            if(Minutes == 60)
            {
                Minutes = 0;
                Hours++;
            }

            lblTime.Text = $"{Hours.ToString()}:{Minutes.ToString()}:{Seconds.ToString()}";


        }

        private void ctrlPool_Load(object sender, EventArgs e)
        {
            
        }
    }
}
