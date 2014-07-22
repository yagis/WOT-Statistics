using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;



namespace WOTStatistics.Core
{
    public delegate void DossierWatcher_Changed(object sender, FileSystemEventArgs e);

    public class DossierWatcher
    {
        private readonly FileSystemWatcher _fileMonitor = new FileSystemWatcher();
        private MessageQueue _messages;
        private readonly string _watchPath = "";
        private readonly string _playerName = "";
        private DateTime lastwrite = DateTime.Now;

        private readonly Dictionary<string, DateTime> m_pendingEvents = new Dictionary<string, DateTime>();
        private readonly Timer m_timer;
        private bool m_timerStarted = false;

        public event DossierWatcher_Changed Changed;

        protected virtual void OnChanged(object source, FileSystemEventArgs e)
        {
            
            if (Changed != null)
                Changed(this, e);
        }

        public DossierWatcher(string watchPath, string playerName, MessageQueue messages, System.Windows.Forms.Form owner)
        {
            _playerName = playerName;
            _watchPath = watchPath;
            FileInfo fi = new FileInfo(watchPath);
            _fileMonitor.SynchronizingObject = owner;
            _fileMonitor.Path = fi.DirectoryName;
            _fileMonitor.NotifyFilter = NotifyFilters.LastWrite;
            _fileMonitor.Filter = "*.dat";
            _fileMonitor.EnableRaisingEvents = false;

            fi = null;

            _fileMonitor.Changed += new FileSystemEventHandler(_fileMonitor_Changed);

            m_timer = new Timer(); m_timer.Tick += new EventHandler(m_timer_Tick);


            _messages = messages;

            //_messages.Add(string.Format("Info : Monitoring File : {0}", _playerName));
        }

        void m_timer_Tick(object sender, EventArgs e)
        {
            List<string> paths;

            lock (m_pendingEvents)
            {
                paths = FindReadyPaths(m_pendingEvents);

                // Remove paths that are going to be used now
                paths.ForEach(delegate(string path)
                {
                    m_pendingEvents.Remove(path);
                });

                // Stop the timer if there are no more events pending
                if (m_pendingEvents.Count == 0)
                {
                    m_timer.Stop();
                    m_timerStarted = false;
                }
            }

            paths.ForEach(delegate(string path)
            {
                FileInfo fi = new FileInfo(path);

                OnChanged(null, new FileSystemEventArgs(WatcherChangeTypes.Changed, fi.DirectoryName, fi.Name));
            });
        }

        public DossierWatcher(string watchPath, string playerName, MessageQueue messages)
        {
            _playerName = playerName;
            _watchPath = watchPath;
            FileInfo fi = new FileInfo(watchPath);
            _fileMonitor.Path = fi.DirectoryName;
            _fileMonitor.NotifyFilter = NotifyFilters.LastWrite;
            _fileMonitor.Filter = "*.dat";
            _fileMonitor.EnableRaisingEvents = false;

            fi = null;

            _fileMonitor.Changed += new FileSystemEventHandler(_fileMonitor_Changed);
            m_timer = new Timer(); m_timer.Tick += new EventHandler(m_timer_Tick);
        }

        void _fileMonitor_Changed(object sender, FileSystemEventArgs e)
        {

            lock (m_pendingEvents)
            {
                m_pendingEvents[e.FullPath] = DateTime.Now;

                if (!m_timerStarted)
                {
                    m_timer.Interval = 100;
                    m_timer.Start();
                    m_timerStarted = true;
                }

            }              

        }

        private List<string> FindReadyPaths(Dictionary<string, DateTime> events)
        {
            List<string> results = new List<string>();
            DateTime now = DateTime.Now;

            foreach (KeyValuePair<string, DateTime> entry in events)
            {
                // If the path has not received a new event in the last 75ms
                // an event for the path should be fired
                double diff = now.Subtract(entry.Value).TotalMilliseconds;
                if (diff >= 100)
                {
                    results.Add(entry.Key);
                }
            }

            return results;
        }

        public void Start()
        {
            _fileMonitor.EnableRaisingEvents = true;
            _messages.Add(string.Format("Info : Monitor Started ({0})", _playerName));
        }

        public void Stop()
        {
            _fileMonitor.EnableRaisingEvents = false;
            _messages.Add(string.Format("Monitor Stoped ({0})", _playerName));
        }

    }
}
