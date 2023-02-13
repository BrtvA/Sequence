using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sequence.Models
{
    class InputModel
    {
        [JsonIgnore]
        public string Path { get; set; }
        public int BitADC { get; set; }
        public int NumberLSB { get; set; }
        public int TimeShift { get; set; }
        public bool IsCustomTimeSeries { get; set; }
        public int TimeSeries { get; set; }


        public InputModel(string path, string bitADC, string numberLSB, string timeShift, bool isCustomTimeSeries, string timeSeries)
        {
            Path = path;

            int.TryParse(bitADC, out int adc);
            BitADC = adc;

            int.TryParse(numberLSB, out int lsb);
            NumberLSB = lsb;

            int.TryParse(timeShift, out int shift);
            TimeShift = shift;

            IsCustomTimeSeries = isCustomTimeSeries;

            int.TryParse(timeSeries, out int series);
            TimeSeries = series;
        }

        public InputModel()
        {

        }
    }
}
