using NAppUpdate.Framework;
using NAppUpdate.Framework.Common;
using NAppUpdate.Framework.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace FikrPos
{
    class AutoupdateEngine
    {
        public static bool automaticUpdate;
        #region autoupdate
        private static void OnPrepareUpdatesCompleted(IAsyncResult asyncResult)
        {
            try
            {
                ((UpdateProcessAsyncResult)asyncResult).EndInvoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Updates preparation failed. Check the feed and try again.{0}{1}", Environment.NewLine, ex));
                return;
            }

            // Get a local pointer to the UpdateManager instance
            UpdateManager updManager = UpdateManager.Instance;

            //MessageBox.Show("Updates are ready to install. Application will be restarted after updates", "Software updates ready");
            
            try
            {
                updManager.ApplyUpdates(true);//chkRelaunch.Checked, chkLogging.Checked, chkShowConsole.Checked);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error while trying to install software updates{0}{1}", Environment.NewLine, ex));
            }
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            for (int i = 1; (i <= 10); i++)
            {
                if ((worker.CancellationPending == true))
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    DoCheckForUpdates(source);
                    //worker.ReportProgress((i * 10));
                }
            }
        }

        static BackgroundWorker bw = new BackgroundWorker();
        private static AutoupdateEngine instance;
        public static AutoupdateEngine getInstance()
        {
            if (instance == null)
            {
                instance = new AutoupdateEngine();
            }
            return instance;
        }
        public void CheckForUpdates(IUpdateSource _source)
        {
            source = _source;            
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.WorkerSupportsCancellation = true;
            bw.WorkerReportsProgress = true;       
            if (bw.IsBusy != true)
            {
                bw.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("Autoupdate already runs in the background..");
            }
        }

        private static void DoCheckForUpdates(IUpdateSource source)
        {
            //temp dir
            string tempPath = System.IO.Path.GetTempPath();

            // Get a local pointer to the UpdateManager instance
            UpdateManager updManager = UpdateManager.Instance;
            updManager.UpdateSource = source;
            updManager.ReinstateIfRestarted(); // required to be able to restore state after app restart


            // Only check for updates if we haven't done so already
            /*if (updManager.State != UpdateManager.UpdateProcessState.NotChecked)
            {
                MessageBox.Show("Update process has already initialized; current state: " + updManager.State.ToString());
                return;
            }*/

            updManager.CleanUp();
            updManager.Config.BackupFolder = tempPath + "fikrpos\\backup";
            updManager.Config.TempFolder = tempPath + "fikrpos\\temp";
            try
            {
                // Check for updates - returns true if relevant updates are found (after processing all the tasks and
                // conditions)
                // Throws exceptions in case of bad arguments or unexpected results
                updManager.CheckForUpdates(source);
            }
            catch (Exception ex)
            {
                if (ex is NAppUpdateException)
                {
                    // This indicates a feed or network error; ex will contain all the info necessary
                    // to deal with that
                }
                else MessageBox.Show(ex.ToString());
                return;
            }


            if (updManager.UpdatesAvailable == 0)
            {   
                if (!automaticUpdate) MessageBox.Show("Your software is up to date");
                bw.CancelAsync();                
                return;
            }

            DialogResult dr = MessageBox.Show(string.Format("Updates are available to your software ({0} total). Application updates will occur\n\nDo you want to update it now?", updManager.UpdatesAvailable), "Software updates available", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                updManager.BeginPrepareUpdates(OnPrepareUpdatesCompleted, null);
            }
        }
        #endregion

        public static IUpdateSource source { get; set; }
    }
}
