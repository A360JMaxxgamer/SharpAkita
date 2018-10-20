using System.Collections.Generic;
using System.ComponentModel;

namespace SharpAkita.Data
{
#pragma warning disable S3925 // "ISerializable" should be implemented correctly
    /// <summary>
    ///     A <see cref="Dictionary{TKey,TValue}" /> which implements <see cref="INotifyPropertyChanged" /> and
    ///     <see cref="INotifyPropertyChanging" />.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class ObservableDictionary<TKey, TValue> : Dictionary<TKey, TValue>,
        INotifyPropertyChanged,
        INotifyPropertyChanging
    {
        #region Indexer

        public new TValue this[TKey key]
        {
            get => base[key];
            set
            {
                if (EqualityComparer<TValue>.Default.Equals(base[key], value)) return;

                OnPropertyChanging(key);
                base[key] = value;
                OnPropertyChanged(key);
            }
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        #endregion

        #region Public methods

        public void OnPropertyChanged(object key)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item[" + key + "]"));
        }

        public void OnPropertyChanging(object key)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs("Item[" + key + "]"));
        }

        #endregion
    }
}