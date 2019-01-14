using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Rimage.Client.Api;

namespace RMDS
{
    public class OrderListener : IOrderStatusListener
    {
        RMDS.frmMain mReportForm; // report status to Form1

        public OrderListener(RMDS.frmMain reportForm)
        {
            mReportForm = reportForm;
        }

        public void OnStatus(string xmlOrderStatus)
        {
            try
            {
                mReportForm.Invoke(mReportForm.StatusDel, xmlOrderStatus);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }
    }
}
