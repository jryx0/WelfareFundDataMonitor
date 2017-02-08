using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace FileTranser.MTOM.ClassLibrary
{
    public class FileTransferBase : BackgroundWorker
    {
        protected FileTranserServiceProxy.FileTransferWebService fileWebservices;            // the web service object
        public bool AutoSetChunkSize = true;            // take a sample of 5 small chunks and then change the chunk size to suit the bandwidth capacity.  bigger capacity = bigger chunks = more efficient.
        public int ChunkSize = 16 * 1024;               // 16k by default
        public int MaxRetries = 50;                     // max number of corrupted chunks or failed transfers to allow before giving up
        protected int NumRetries = 0;
        public int ChunkSizeSampleInterval = 15;        // interval to update the chunk size, used in conjunction with AutoSetChunkSize. 
        public bool IncludeHashVerification = true;     // checks the local file hash against the uploaded file hash to verify that the files are identical
        public int PreferredTransferDuration = 800;     // milliseconds, the timespan the class will attempt to achieve for each chunk, to give responsive feedback on the progress bar.
        protected long Offset = 0;                      // used in persisting the transfer position
        protected string LocalFileName;
        protected DateTime StartTime;
        protected Thread HashThread;
        public string LocalFileHash;            // the user can access the local file hash after the upload is complete and the hash has been calculated
        public string RemoteFileHash;           // as above
        public string LocalFilePath;            // this variable must be set prior to starting an Upload.  
        public long MaxRequestLength = 4096;    // default, this is updated so that the transfer class knows how much the server will accept
        public string Guid;                     // used to track events when multiple file transfers are happening

        public event EventHandler ChunkSizeChanged;
        private CookieContainer cookies = new CookieContainer();

        public FileTransferBase()
        {
            base.WorkerReportsProgress = true;
            base.WorkerSupportsCancellation = true;
        }

        public FileTranserServiceProxy.FileTransferWebService theWebservices
        {
            get
            {
                if (this.fileWebservices == null)
                {
                    this.fileWebservices = new FileTranserServiceProxy.FileTransferWebService();
                    this.fileWebservices.Credentials = System.Net.CredentialCache.DefaultCredentials;
                    this.fileWebservices.CookieContainer = cookies;
                }
                return this.fileWebservices;
            }
            set
            {
                this.fileWebservices = value;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (this.HashThread != null && this.HashThread.IsAlive)
                this.HashThread.Abort();

            base.Dispose(disposing);
        }

        protected override void OnRunWorkerCompleted(RunWorkerCompletedEventArgs e)
        {
            if (this.HashThread != null && this.HashThread.IsAlive)
                this.HashThread.Abort();
            base.OnRunWorkerCompleted(e);
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            // make sure we can connect to the web service.  if this step is not done here, it will retry 50 times because of the retry code...
            try
            {
                // BUG here when failed log in is followed by successful login. suspect cookies getting mixed up, but not understood...
                long webConfigSetting = this.theWebservices.GetMaxRequestLength();
                this.MaxRequestLength = Math.Max(1, (webConfigSetting * 1024) - (2 * 1024));    // set the max buffer size to slightly less than the request length to allow for SOAP message headers etc.  
            }
            catch (Exception ex)
            {
                this.MaxRequestLength = 4000;
            }
            base.OnDoWork(e);
        }

        /// <summary>
        /// Returns a description of a number of bytes, in appropriate units.
        /// e.g. 
        ///		passing in 1024 will return a string "1 Kb"
        ///		passing in 1230000 will return "1.23 Mb"
        /// Megabytes and Gigabytes are formatted to 2 decimal places.
        /// Kilobytes are rounded to whole numbers.
        /// If the rounding results in 0 Kb, "1 Kb" is returned, because Windows behaves like this also.
        /// </summary>
        public static string CalcFileSize(long numBytes)
        {
            string fileSize = "";

            if (numBytes > 1073741824)
                fileSize = String.Format("{0:0.00} Gb", (double)numBytes / 1073741824);
            else if (numBytes > 1048576)
                fileSize = String.Format("{0:0.00} Mb", (double)numBytes / 1048576);
            else
                fileSize = String.Format("{0:0} Kb", (double)numBytes / 1024);

            if (fileSize == "0 Kb")
                fileSize = "1 Kb";  // min.							
            return fileSize;
        }

        /// <summary>
        /// This method is intended to be run on a background thread. 
        /// </summary>
        public static string CalcFileHash(string FilePath)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] hash;
            using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096))
                hash = md5.ComputeHash(fs);
            return BitConverter.ToString(hash);
        }

        protected void CheckLocalFileHash()
        {
            this.LocalFileHash = CalcFileHash(this.LocalFilePath);
        }

        protected void CalcAndSetChunkSize()
        {
            /* chunk size calculation is defined as follows 
			 *		in the examples below, the preferred transfer time is 1500ms, taking one sample.
			 *		
			 *									  Example 1									Example 2
			 *		Initial size				= 16384 bytes	(16k)						16384
			 *		Transfer time for 1 chunk	= 800ms										2000 ms
			 *		Average throughput / ms		= 16384b / 800ms = 20.48 b/ms				16384 / 2000 = 8.192 b/ms
			 *		How many bytes in 1500ms?	= 20.48 * 1500 = 30720 bytes				8.192 * 1500 = 12228 bytes
			 *		New chunksize				= 30720 bytes (speed up)					12228 bytes (slow down from original chunk size)
			 */
            double transferTime = DateTime.Now.Subtract(this.StartTime).TotalMilliseconds;
            double averageBytesPerMilliSec = this.ChunkSize / transferTime;
            double preferredChunkSize = averageBytesPerMilliSec * this.PreferredTransferDuration;
            this.ChunkSize = (int)Math.Min(this.MaxRequestLength, Math.Max(4 * 1024, preferredChunkSize));  // set the chunk size so that it takes 1500ms per chunk (estimate), not less than 4Kb and not greater than 4mb // (note 4096Kb sometimes causes problems, probably due to the IIS max request size limit, choosing a slightly smaller max size of 4 million bytes seems to work nicely)			

            string statusMessage = String.Format("Chunk size: {0}{1}", CalcFileSize(this.ChunkSize), (this.ChunkSize == this.MaxRequestLength) ? " (max)" : "");
            if (this.ChunkSizeChanged != null)
                this.ChunkSizeChanged(statusMessage, EventArgs.Empty);
        }
    }
}
