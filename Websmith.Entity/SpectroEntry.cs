using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Websmith.Entity
{
    public class SpectroEntry
    {
        #region Private Fields
        private Guid _SpectroID = new Guid("00000000-0000-0000-0000-000000000000");
        private Int64 _SpectroNo=0;
        private string _KeyName = string.Empty;
        private string _KeyValue = string.Empty;
        private DateTime _EntryDate = DateTime.Now;
        private string _Mode = string.Empty;
        private string _Message=string.Empty;
        #endregion

        #region Public Properties
        public Guid SpectroID
        {
            get { return _SpectroID; }
            set { _SpectroID = value; }
        }
        public Int64 SpectroNo
        {
            get { return _SpectroNo; }
            set { _SpectroNo = value; }
        }
        public string KeyName
        {
            get { return _KeyName; }
            set { _KeyName = value; }
        }
        public string KeyValue
        {
            get { return _KeyValue; }
            set { _KeyValue = value; }
        }
        public DateTime EntryDate
        {
            get { return _EntryDate; }
            set { _EntryDate = value; }
        }
        public string Mode
        {
            get { return _Mode; }
            set { _Mode = value; }
        }
        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }
        #endregion
        
    }

    public class SpectroDetailParam
    {
        public Int64 SpectroNo { get; set; } = 0;
        public string Mode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
