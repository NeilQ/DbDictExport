using System;
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

        private static LoadingFormService _instance;
        public static LoadingFormService Instance
        {
            get {
                if (_instance == null)
                {
                    lock (syncLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new LoadingFormService();
                        }
                    }
                }
                return _instance;
            }
        }

        public static void CreateForm()
        {
            Instance.Create();
        }

        public static void CloseFrom()
        {
            Instance.Close();
        }

        public static void SetFormCaption(string text)
        {
            Instance.SetCaption(text);
        }

        public void Create()
        {
            Close();
            frm = new LoadingForm();
            loadingThread = new Thread(s => Application.Run(frm));
            loadingThread.Start();
        }

        private void Dispose()
        {
            if (frm != null)
            {
                if (frm.InvokeRequired)
                {
                    CloseLoadingForm closeFrm = Dispose;
                }
                else
                {
                    frm.Dispose();
                }
            }
            
        }

        public void Close()
        {
            if (loadingThread == null) return;
            try
            {
                Dispose();
                loadingThread.Abort();
                loadingThread.Join();
                loadingThread.DisableComObjectEagerCleanup();
            }
            catch { }
        }

        public void SetCaption(string text)
        {
            if (frm == null) return;
            try
            {
                frm.SetText(text);
            }
            catch { }
        }
    }
}
