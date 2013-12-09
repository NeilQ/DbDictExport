using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DbDictExport.WinForm.Service
{
    public class LoadingFormService
    {
        private Thread loadingThread;
        private LoadingForm frm;
        private delegate void CloseLoadingForm();
        private static readonly Object syncLock = new object();

        private static LoadingFormService _instance = null;
        public static LoadingFormService Instance
        {
            get {
                if (LoadingFormService._instance == null)
                {
                    lock (syncLock)
                    {
                        if (LoadingFormService._instance == null)
                        {
                            LoadingFormService._instance = new LoadingFormService();
                        }
                    }
                }
                return LoadingFormService._instance;
            }
        }

        public static void CreateForm()
        {
            LoadingFormService.Instance.Create();
        }

        public static void CloseFrom()
        {
            LoadingFormService.Instance.Close();
        }

        public static void SetFormCaption(string text)
        {
            LoadingFormService.Instance.SetCaption(text);
        }

        public void Create()
        {
            Close();
            frm = new LoadingForm();
            loadingThread = new Thread(new ThreadStart(delegate() { Application.Run(frm); }));
            loadingThread.Start();
        }

        private void Dispose()
        {
            if (frm != null && frm.InvokeRequired)
            {
                CloseLoadingForm closeFrm = new CloseLoadingForm(Dispose);
            }
            else
            {
                frm.Dispose();
            }
        }

        public void Close()
        {
            if (loadingThread != null)
            {
                try
                {
                    Dispose();
                    loadingThread.Abort();
                    loadingThread.Join();
                    loadingThread.DisableComObjectEagerCleanup();
                }
                catch { }
            }
        }

        public void SetCaption(string text)
        {
            if (frm != null)
            {
                try
                {
                    frm.SetText(text);
                }
                catch { }
            }
        }
    }
}
