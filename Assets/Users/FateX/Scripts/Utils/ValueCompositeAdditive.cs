using System;
using System.Collections.Generic;

namespace Скриптерсы.Utils
{
    public class ValueCompositeAdditive<T> where T : struct
    {
        private T sum;
        private T originSum;
        public T Sum => sum;

        private Dictionary<string, T> additionalValues = new();

        public ValueCompositeAdditive()
        {
            sum = (T)Convert.ChangeType(0, typeof(T));
            originSum = sum;
        }

        public ValueCompositeAdditive(T initialValue)
        {
            sum = initialValue;
            originSum = sum;
        }

        public void AddAdditional(string name, T value)
        {
            if(Convert.ToDouble(value) == 0)
                return;
            additionalValues[name] = value;
            RecalculateSum();
        }

        public void RemoveAdditional(string name)
        {
            additionalValues.Remove(name);
            RecalculateSum();
        }

        public void UpdateAdditional(string name, T value)
        {
            if (additionalValues.ContainsKey(name))
            {
                additionalValues[name] = value;
                RecalculateSum();
            }
        }

        private void RecalculateSum()
        {
            double result = Convert.ToDouble(originSum);

            foreach (var pair in additionalValues)
            {
                result += Convert.ToDouble(pair.Value);
            }

            sum = (T)Convert.ChangeType(result, typeof(T));
        }

        public bool TryFindAdditional(string name)
        {
            return additionalValues.ContainsKey(name);
        }
    }
}