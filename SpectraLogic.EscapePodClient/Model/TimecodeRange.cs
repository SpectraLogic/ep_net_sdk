using System.Runtime.Serialization;

namespace SpectraLogic.EscapePodClient.Model
{
    [DataContract]
    public class TimecodeRange
    {
        [DataMember(Order = 1)] private long Start;
        [DataMember(Order = 2)] private long Stop;

        public TimecodeRange(long start, long stop)
        {
            Start = start;
            Stop = stop;
        }
    }
}