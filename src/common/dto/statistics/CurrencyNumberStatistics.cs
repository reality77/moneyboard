using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace dto.statistics
{
    public class CurrencyNumberStatisticsByString :  CurrencyNumberStatistics<string>
    {
    }

    public class CurrencyNumberStatisticsByInt :  CurrencyNumberStatistics<int>
    {
    }

    public class CurrencyNumberStatisticsByDate :  CurrencyNumberStatistics<DateTime>
    {
    }

    public abstract class CurrencyNumberStatistics<T> 
    {
        private Dictionary<T, int> _dicIndexByXValue;
        private Dictionary<int, Dictionary<int, CurrencyNumber>> _dicDataPoints;

        public List<T> XValues { get; set; }

        public List<string> SeriesNames { get; set; }

        public CurrencyNumber?[][] DataPoints { get; set; }

        public CurrencyNumberStatistics()
        {
        }

        public void Init()
        {
            this.XValues = new List<T>();
            _dicIndexByXValue = new Dictionary<T, int>();

            this.SeriesNames = new List<string>();

            _dicDataPoints = new Dictionary<int, Dictionary<int, CurrencyNumber>>();
        }

        public int SetXValue(T value)
        {
            if(_dicIndexByXValue.ContainsKey(value))
                return _dicIndexByXValue[value];

            int index = this.XValues.Count;

            this.XValues.Add(value);
            _dicIndexByXValue.Add(value, index);
            return index;
        }

        public int AddSerie(string serieName)
        {
            int index = this.SeriesNames.Count;

            this.SeriesNames.Add(serieName);
            _dicDataPoints.Add(index, new Dictionary<int, CurrencyNumber>());

            return index;
        }

        public void SetValue(int xValueIndex, int serieIndex, CurrencyNumber datapoint)
        {
            _dicDataPoints[serieIndex][xValueIndex] = datapoint;
        }

        public void GenerateDataPoints()
        {
            this.DataPoints = new CurrencyNumber?[this.SeriesNames.Count][];

            for(int s = 0; s < this.SeriesNames.Count; s++)
            {
                this.DataPoints[s] = new CurrencyNumber?[this.XValues.Count];

                for(int i = 0; i < this.XValues.Count; i++)
                {
                    if(_dicDataPoints.ContainsKey(s) && _dicDataPoints[s].ContainsKey(i))
                        this.DataPoints[s][i] = _dicDataPoints[s][i];
                    else
                        this.DataPoints[s][i] = null;
                }
            }


        }
    }
}
