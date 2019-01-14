using System;
using System.Collections.Generic;
using System.Text;
using Rimage.Client.Api;
using Rimage.Client.Api.Exception;

namespace RMDS
{
    public class SystemListener : ISystemListener
    {
        private RMDS.frmMain mReportForm;

        public SystemListener(RMDS.frmMain reportForm)
        {
            mReportForm = reportForm;
        }

        public void OnSystemException(CSystemException rimageExecption)
        {
            this.mReportForm.Invoke(mReportForm.systemListenerDel, rimageExecption);
        }

        public void OnClusterCreated(String cluster, int serverType)
        {
            // Add code to respond to cluster being created
        }

        public void OnClusterDeleted(String cluster, int serverType)
        {
            // Add code to respond to cluster being deleted
        }
    }
}
