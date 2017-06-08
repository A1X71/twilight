using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CheckBoxTest
{
    public enum Test
    {
        A,
        B,
        C,
        D,
        E,
        F
    }

    // ViewModel for Test enumeration
    public class CheckValues
    {
        public Test Value { get; set; }

        public string Text
        {
            get { return Value.ToString(); }
        }

        public bool IsChecked { get; set; }
    }

    // ViewModel for Exclusive Test enumeration
    public class ExclusiveCheckValues
    {
        public static MainViewModel Owner { get; set; }

        public Test Value { get; set; }
        public string Text { get { return Value.ToString(); } }
        public bool IsChecked
        {
            get { return Value == Owner.SelectedValue; }
            set
            {
                if (value)
                    Owner.SelectedValue = Value;
            }
        }
    }

    // Main ViewModel for window
    public class MainViewModel : INotifyPropertyChanged
    {
        public List<CheckValues> EnumValues { get; private set; }
        public List<ExclusiveCheckValues> EnumValues2 { get; private set; }

        private Test _selectedValue;
        public Test SelectedValue
        {
            get { return _selectedValue; }
            set { _selectedValue = value; OnPropertyChanged("SelectedValue"); }
        }

        public MainViewModel()
        {
            EnumValues = new List<CheckValues>();
            EnumValues2 = new List<ExclusiveCheckValues>();
            foreach (object t in Enum.GetValues(typeof(Test)))
            {
                EnumValues.Add(new CheckValues { Value = (Test)t });
                EnumValues2.Add(new ExclusiveCheckValues { Value = (Test)t });
            }

            SelectedValue = Test.B;
            ExclusiveCheckValues.Owner = this;
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private void OnPropertyChanged(string propName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}