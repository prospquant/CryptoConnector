using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HQConnector.Dto
{
    public class PropertyChangedBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Protected Methods

        /// <summary>
        /// Fires the event <see cref="PropertyChanged"/>
        /// </summary>
        /// <param name="propertyName">The name of the changed property.</param>
        protected void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            //if (PropertyChanged != null)
            //{
            storage = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            //}

        }

        protected void Set([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        #endregion
    }
}
