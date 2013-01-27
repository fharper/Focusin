using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Focusin.Model
{
    [DataContract]
    public class Session : INotifyPropertyChanged
    {
        private int _number;

        [DataMember]
        public int Number
        {
            get { return _number; }
            set
            {
                _number = value;
                RaisePropertyChanged("Number");
            }
        }
        
        private TimeSpan _minutes;

        [DataMember]
        public TimeSpan Minutes
        {
            get { return _minutes; }
            set
            {
                _minutes = value;
                RaisePropertyChanged("Minutes");
            }
        }

        [DataMember]
        public bool IsFreeTime { get; set; }

        public Session(int number, TimeSpan minutes, bool isFreeTime=false)
        {
            Number = number;
            Minutes = minutes;
            IsFreeTime = isFreeTime;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}