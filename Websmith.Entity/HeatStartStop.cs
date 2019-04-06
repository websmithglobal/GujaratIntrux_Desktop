using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Websmith.Entity
{
    public class HeatStartStop
    {
        #region Private Fields
        private Guid _HeatID;
        private DateTime? _HeatStart;
        private DateTime? _HeatStop;
        private int _IsStop;
        private string _Mode;
        private string _Message;
        #endregion

        #region Public Properties
        public Guid HeatID
        {
            get { return _HeatID; }
            set { _HeatID = value; }
        }
       
        public DateTime? HeatStart
        {
            get { return _HeatStart; }
            set { _HeatStart = value; }
        }
        public DateTime? HeatStop
        {
            get { return _HeatStop; }
            set { _HeatStop = value; }
        }
        public int IsStop
        {
            get { return _IsStop; }
            set { _IsStop = value; }
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
}
