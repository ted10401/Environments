
namespace JSLCore.Utils
{
    public class BindableProperty<T>
    {
        public delegate void ValueChangedHandler(T oldValue, T newValue);
        public ValueChangedHandler OnValueChanged;

        private T m_value;
        public T Value
        {
            get
            {
                return m_value;
            }
            set
            {
                if (!Equals(m_value, value))
                {
                    T old = m_value;
                    m_value = value;
                    ValueChanged(old, m_value);
                }
            }
        }

        private void ValueChanged(T oldValue, T newValue)
        {
            if (OnValueChanged != null)
            {
                OnValueChanged(oldValue, newValue);
            }
        }

        public override string ToString()
        {
            return (Value != null ? Value.ToString() : "null");
        }
    }
}