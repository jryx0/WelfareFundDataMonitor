using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;


namespace FileTranser.MTOM.ClassLibrary
{
    /// <summary>
	/// A class to upload a file to a web server using WSE 3.0 web services, with the MTOM standard.
	/// To use this class, drag/drop an instance onto a windows form, and create event handlers for ProgressChanged
	/// and RunWorkerCompleted.  
	/// Make sure to specify the LocalFilePath before you call RunWorkerAsync() to begin the upload
	/// </summary>
	public class FileTransferUpload : FileTransferBase
    {
        internal string UploadToken;
        internal string LoginToken;

        /// <summary>
        /// Start the upload operation synchronously.
        /// The argument must be the start position, usually 0
        /// </summary>
        public void RunWorkerSync(DoWorkEventArgs e)
        {
            try
            {
                OnDoWork(e);
                base.OnRunWorkerCompleted(new RunWorkerCompletedEventArgs(UploadToken, null, false));
            }
            catch (Exception ex)
            {
                base.OnRunWorkerCompleted(new RunWorkerCompletedEventArgs(UploadToken, ex, false));
            }
        }

        /// <summary>
        /// This method starts the uplaod process. It supports cancellation, reporting progress, and exception handling.
        /// The argument is the start position, usually 0
        /// </summary>
        protected override void OnDoWork(DoWorkEventArgs e)
        {
            base.OnDoWork(e);

            this.Offset = Int64.Parse(e.Argument.ToString());
            int numIterations = 0;  // this is used with a modulus of the sampleInterval to check if the chunk size should be adjusted.  it is started at 1 so that the first check will be skipped because it may involve an initial delay in connecting to the web service
            this.LocalFileName = Path.GetFileName(this.LocalFilePath);
            if (this.AutoSetChunkSize)
                this.ChunkSize = 256 * 1024; // 16Kb default

            if (!File.Exists(LocalFilePath))
                throw new Exception(String.Format("Could not find file {0}", LocalFilePath));

            long FileSize = new FileInfo(LocalFilePath).Length;
            string FileSizeDescription = CalcFileSize(FileSize); // e.g. "2.4 Gb" instead of 240000000000000 bytes etc...			
            byte[] Buffer = new byte[ChunkSize];    // this buffer stores each chunk, for sending to the web service via MTOM

            MaxRetries = 2;
            using (FileStream fs = new FileStream(this.LocalFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                fs.Position = this.Offset;
                int BytesRead;  // = fs.Read(Buffer, 0, ChunkSize);	// read the first chunk in the buffer
                                // send the chunks to the web service one by one, until FileStream.Read() returns 0, meaning the entire file has been read.
                do
                {
                    BytesRead = fs.Read(Buffer, 0, ChunkSize);  // read the next chunk (if it exists) into the buffer.  the while loop will terminate if there is nothing left to read

                    // check if this is the last chunk and resize the bufer as needed to avoid sending a mostly empty buffer (could be 10Mb of 000000000000s in a large chunk)
                    if (BytesRead != Buffer.Length)
                    {
                        this.ChunkSize = BytesRead;
                        byte[] TrimmedBuffer = new byte[BytesRead];
                        Array.Copy(Buffer, TrimmedBuffer, BytesRead);
                        Buffer = TrimmedBuffer; // the trimmed buffer should become the new 'buffer'
                    }
                    if (Buffer.Length == 0)
                        break;  // nothing more to send
                    try
                    {
                        // send this chunk to the server.  it is sent as a byte[] parameter, but the client and server have been configured to encode byte[] using MTOM. 
                        // this.theWebservices.AppendChunk(this.LocalFileName, Buffer, this.Offset);
                        this.theWebservices.AppendChunk(LoginToken, UploadToken, Buffer, this.Offset);
                        if (this.AutoSetChunkSize)
                        {
                            // sample every 15th transfer by default
                            int currentIntervalMod = numIterations % this.ChunkSizeSampleInterval;
                            if (currentIntervalMod == 0)    // start the timer for this chunk
                                this.StartTime = DateTime.Now;
                            else if (currentIntervalMod == 1)
                            {
                                this.CalcAndSetChunkSize(); // the sample chunk has been transferred, calc the time taken and adjust the chunk size accordingly
                                Buffer = new byte[this.ChunkSize];    // reset the size of the buffer for the new chunksize
                            }
                        }

                        // Offset is only updated AFTER a successful send of the bytes. this helps the 'retry' feature to resume the upload from the current Offset position if AppendChunk fails.
                        this.Offset += BytesRead;   // save the offset position for resume

                        // update the user interface
                        string SummaryText = String.Format("Transferred {0} / {1}", CalcFileSize(this.Offset), FileSizeDescription);
                        this.ReportProgress((int)(((decimal)Offset / (decimal)FileSize) * 100), SummaryText);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Exception: " + ex.ToString());

                        // rewind the filestream and keep trying
                        fs.Position -= BytesRead;

                        if (NumRetries++ >= MaxRetries) // too many retries, bail out
                        {
                            e.Result = -1;
                            fs.Close();
                            throw new Exception(String.Format("Error occurred during upload, too many retries. \n{0}", ex.ToString()));
                        }
                    }
                    numIterations++;

                    Thread.Sleep(200);

                } while (BytesRead > 0 && !this.CancellationPending);
            }
            this.ReportProgress(100, "开始校验...");
            if (this.IncludeHashVerification)
            {
                //this.ReportProgress(99, "Checking file hash...");

                // start calculating the local hash
                this.HashThread = new Thread(new ThreadStart(this.CheckLocalFileHash));
                this.HashThread.Start();

                // request the server hash
                this.theWebservices.Timeout = 1000 * 60 * 10;   // 10 mins, big files takes ages to hash
                //this.RemoteFileHash = this.theWebservices.CheckFileHash(this.LocalFileName);
                this.RemoteFileHash = this.theWebservices.CheckFileHash(LoginToken, UploadToken);
                this.theWebservices.Timeout = 1000 * 30;    // reset back to 30 seconds

                // wait for the local hash to complete
                this.HashThread.Join();

                if (this.LocalFileHash == this.RemoteFileHash)
                    e.Result = 10; // String.Format("Hashes match exactly\r\nLocal hash:  {0}\r\nServer hash: {1}", LocalFileHash, RemoteFileHash);
                else    // could throw an exception here if you want.  different hashes indicates a corrupt upload
                {
                    e.Result = -2;
                    throw new Exception(String.Format("Hashes are different!\r\nLocal hash:  {0}\r\nServer hash: {1}", LocalFileHash, RemoteFileHash));
                }
            }
        }
    }
}
