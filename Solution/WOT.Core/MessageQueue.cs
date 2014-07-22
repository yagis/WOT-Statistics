using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WOTStatistics.Core
{

    public delegate void MessageQueue_OnAdd(object sender, MessageEventArgs e);

    public class MessageQueue:Dictionary<int, string>
    {
        public new int Count { get { return base.Count; } }

        public event MessageQueue_OnAdd ItemAdded;

        public void Add(string value)
        {
            int max;
            if (base.Count == 0)
                max = 0;
            else
            {
                max = (from m in base.Keys
                           select m).Max();
            }

           base.Add(max + 1, string.Format("{0:yyyy-MM-dd HH:mm:ss} : {1}",DateTime.Now, value));
           OnAdd(new MessageEventArgs(max + 1, string.Format("{0:yyyy-MM-dd HH:mm:ss} : {1}", DateTime.Now, value)));
           WOTHelper.AddToLog(value);
        }

        public new void Remove(int key)
        {
            if (base.ContainsKey(key))
            {
                base.Remove(key);
             
            }
           
        }

        protected virtual void OnAdd(MessageEventArgs e)
        {
            if (ItemAdded != null)
                ItemAdded(this, e);

            if (base.ContainsKey(e.Key))
                base.Remove(e.Key);
        }


    }

    public class MessageEventArgs : EventArgs
    {

        public int Key { get; private set; }
        public string Value { get; private set; }

        public MessageEventArgs(int key, string value)
        {
            Key = key;
            Value = value;
        }


        
    }
}
