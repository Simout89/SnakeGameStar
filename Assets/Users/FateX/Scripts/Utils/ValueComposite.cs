using System;
using System.Collections.Generic;

namespace Скриптерсы.Utils
{
    public class ValueComposite<T> where T : struct
    {
        private T multiplier;
        public T Multiplier => multiplier;

        private Dictionary<string, T> additionalValues = new();

        public ValueComposite()
        {
            multiplier = (T)Convert.ChangeType(1, typeof(T));
        }

        public void AddAdditional(string name, T value)
        {
            additionalValues[name] = value;
            RecalculateMultiplier();
        }

        public void RemoveAdditional(string name)
        {
            additionalValues.Remove(name);
            RecalculateMultiplier();
        }
        
        public void UpdateAdditional(string name, T value)
        {
            if (additionalValues.ContainsKey(name))
            {
                additionalValues[name] = value;
                RecalculateMultiplier();
            }
        }

        private void RecalculateMultiplier()
        {
            if (additionalValues.Count == 0)
            {
                multiplier = (T)Convert.ChangeType(1, typeof(T));
                return;
            }

            double result = 1;
            foreach (var pair in additionalValues)
            {
                result *= Convert.ToDouble(pair.Value);
            }
            multiplier = (T)Convert.ChangeType(result, typeof(T));
        }
        
        public bool TryFindAdditional(string name)
        {
            foreach (var value in additionalValues)
            {
                if (value.Key == name)
                    return true;
            }
            return false;
        }
    }
}