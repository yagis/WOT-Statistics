using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Windows.Forms;
using System.Net.Cache;
using DevExpress.XtraEditors;
using System.Threading;

namespace WOT.Stats
{

    public delegate void BytesDownloadedEventHandler(ByteArgs e);

    public class ByteArgs : EventArgs
    {
        private int _downloaded;
        private int _total;

        public int downloaded
        {
            get
            {
                return _downloaded;
            }
            set
            {
                _downloaded = value;
            }
        }

        public int total
        {
            get
            {
                return _total;
            }
            set
            {
                _total = value;
            }
        }

    }

    class webdata
    {

        public static event BytesDownloadedEventHandler bytesDownloaded;

        public static byte[] DownloadFromWeb(string URL, string file, UpdateGUIProgressBar statusBar)
        {
            try
            {

                byte[] downloadedData = new byte[0];

                //open a data stream from the supplied URL
                   
                
                WebRequest webReq = WebRequest.Create(URL + file);
                webReq.Timeout = 5000;
                webReq.Proxy.Credentials = CredentialCache.DefaultCredentials;
                webReq.CachePolicy = new System.Net.Cache.RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);

                WebResponse webResponse = webReq.GetResponse();
                Stream dataStream = webResponse.GetResponseStream();
                
                //Download the data in chuncks
                byte[] dataBuffer = new byte[1024];

                //Get the total size of the download
                int dataLength = (int)webResponse.ContentLength;

                //lets declare our downloaded bytes event args
                ByteArgs byteArgs = new ByteArgs();

                byteArgs.downloaded = 0;
                byteArgs.total = dataLength;

                //we need to test for a null as if an event is not consumed we will get an exception
                if (bytesDownloaded != null) bytesDownloaded(byteArgs);

                //Download the data
                MemoryStream memoryStream = new MemoryStream();

                while (true)
                {
                    
                    //Let's try and read the data
                    int bytesFromStream = dataStream.Read(dataBuffer, 0, dataBuffer.Length);


                    if (bytesFromStream == 0)
                    {
                        byteArgs.downloaded = dataLength;
                        byteArgs.total = dataLength;
                        if (bytesDownloaded != null) bytesDownloaded(byteArgs);

                        //Download complete
                        break;
                    }
                    else
                    {
                        //Write the downloaded data
                        memoryStream.Write(dataBuffer, 0, bytesFromStream);
                        //Thread.Sleep(100);
                        statusBar(new Tuple<int, int>(dataBuffer.Length, dataLength), 1);
                        
                        byteArgs.downloaded = bytesFromStream;
                        byteArgs.total = dataLength;
                        if (bytesDownloaded != null) bytesDownloaded(byteArgs);

                    }
                    
                }
                
                //Convert the downloaded stream to a byte array
                //Convert the downloaded stream to a byte array
                downloadedData = memoryStream.ToArray();

                //Release resources
                dataStream.Close();
                memoryStream.Close();


                return downloadedData;
            }

            catch (Exception ex)
            {
                if (ex.GetType() == typeof(FileNotFoundException))
                    throw ((FileNotFoundException)ex);
               // else
                //We may not be connected to the internet
                //Or the URL may be incorrect
                throw new Exception(ex.Message);
            }

        }

       
     
        
    }
}
