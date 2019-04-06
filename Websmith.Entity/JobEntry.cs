using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Websmith.Entity
{
    public class JobEntry
    {
        #region Private Fields
        private Guid _JobID;
        private string _JobNo;
        private string _JobFurnaceNo;
        private DateTime? _StartTime;
        private DateTime? _EndTime;
        private DateTime? _Pour1Time;
        private DateTime? _Pour2Time;
        private DateTime? _Pour3Time;
        private DateTime? _Pour4Time;
        private DateTime? _Pour5Time;
        private DateTime? _Pour6Time;
        private int _IsFinish;
        private Guid _HeatFurnaceID;
        private string _Mode;
        private string _Message;
        #endregion

        #region Public Properties
        public Guid JobID
        {
            get { return _JobID; }
            set { _JobID = value; }
        }
        public string JobNo
        {
            get { return _JobNo; }
            set { _JobNo = value; }
        }
        public string JobFurnaceNo
        {
            get { return _JobFurnaceNo; }
            set { _JobFurnaceNo = value; }
        }
        public DateTime? StartTime
        {
            get { return _StartTime; }
            set { _StartTime = value; }
        }
        public DateTime? EndTime
        {
            get { return _EndTime; }
            set { _EndTime = value; }
        }
        public DateTime? Pour1Time
        {
            get { return _Pour1Time; }
            set { _Pour1Time = value; }
        }
        public DateTime? Pour2Time
        {
            get { return _Pour2Time; }
            set { _Pour2Time = value; }
        }
        public DateTime? Pour3Time
        {
            get { return _Pour3Time; }
            set { _Pour3Time = value; }
        }
        public DateTime? Pour4Time
        {
            get { return _Pour4Time; }
            set { _Pour4Time = value; }
        }
        public DateTime? Pour5Time
        {
            get { return _Pour5Time; }
            set { _Pour5Time = value; }
        }
        public DateTime? Pour6Time
        {
            get { return _Pour6Time; }
            set { _Pour6Time = value; }
        }
        public int IsFinish
        {
            get { return _IsFinish; }
            set { _IsFinish = value; }
        }
        public Guid HeatFurnaceID
        {
            get { return _HeatFurnaceID; }
            set { _HeatFurnaceID = value; }
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
