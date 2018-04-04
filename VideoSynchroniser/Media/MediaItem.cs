using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Media
{
    [DataContract]
    public class MediaItem : INotifyPropertyChanged
    {
        #region Fields
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Constructor(s)
        public MediaItem(string name)
        {
            Name = name;
        }
        #endregion

        #region Methods
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Properties
        [DataMember]
        public string Name
        {
            get; private set;
        }
        #endregion
    }
}
