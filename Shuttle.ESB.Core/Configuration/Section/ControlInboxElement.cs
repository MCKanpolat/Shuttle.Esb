using System;
using System.ComponentModel;
using System.Configuration;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public class ControlInboxElement : ConfigurationElement
    {
        [ConfigurationProperty("workQueueUri", IsRequired = true, DefaultValue = "")]
        public string WorkQueueUri
        {
            get { return (string) this["workQueueUri"]; }
        }

		[ConfigurationProperty("errorQueueUri", IsRequired = true, DefaultValue = "")]
        public string ErrorQueueUri
        {
            get { return (string) this["errorQueueUri"]; }
        }

        [ConfigurationProperty("threadCount", IsRequired = false, DefaultValue = 1)]
        public int ThreadCount
        {
            get { return (int)this["threadCount"]; }
        }

        [TypeConverter(typeof(StringDurationArrayConverter))]
		[ConfigurationProperty("durationToSleepWhenIdle", IsRequired = false, DefaultValue = null)]
        public TimeSpan[] DurationToSleepWhenIdle
        {
            get
            {
                return (TimeSpan[])this["durationToSleepWhenIdle"];
            }
        }

        [TypeConverter(typeof(StringDurationArrayConverter))]
		[ConfigurationProperty("durationToIgnoreOnFailure", IsRequired = false, DefaultValue = null)]
        public TimeSpan[] DurationToIgnoreOnFailure
        {
            get
            {
                return (TimeSpan[])this["durationToIgnoreOnFailure"];
            }
        }

        [ConfigurationProperty("maximumFailureCount", IsRequired = false, DefaultValue = 5)]
        public int MaximumFailureCount
        {
            get
            {
                return (int)this["maximumFailureCount"];
            }
        }
    }
}