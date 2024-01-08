using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Touride.Framework.Abstractions.TaskScheduling;

namespace Touride.Framework.TaskScheduling.Hangfire
{
    public class TaskSchedulingHelper
    {
        public static int NormalizeDayOfMonth(int dayOfMonth)
        {
            if (dayOfMonth < 1)
            {
                return 1;
            }

            else if (dayOfMonth > 28)
            {
                return 28;
            }

            else
            {
                return dayOfMonth;
            }
        }
        public static int NormalizeHour(int hour)
        {
            if (hour < 0)
            {
                return 0;
            }

            else if (hour > 23)
            {
                return 0;
            }

            else
            {
                return hour;
            }
        }
        public static int NormalizeMinute(int min)
        {
            if (min < 0)
            {
                return 0;
            }

            else if (min > 59)
            {
                return 0;
            }

            else
            {
                return min;
            }
        }
    }

    public class CronMaker
    {
        public static string SimpleCron(string min, string hour, string dayOfMonth, string month, string dayOfWeek)
        {
            return string.Format("{0} {1} {2} {3} {4}", min, hour, dayOfMonth, month, dayOfWeek);
        }

        public static string CronWithExactTimeWorksDaily(int hour, int min)
        {
            return string.Format("{0} {1} * * *", min, hour);
        }

        public static string CronWithExactDayTimeWorksWeekly(List<CronExpressionDay> days, int hour, int min)
        {
            if (days != null && days.Any())
            {
                string commaSeparatedDays = string.Join(",", days.Select(d => ((int)d).ToString()).ToArray());

                return string.Format("{0} {1} * * {2}", min, hour, commaSeparatedDays);
            }
            else
            {
                return string.Format("{0} {1} * * *", min, hour);
            }

        }

        public static string CronWithExactDayTimeWorksMonthly(List<int> days, int hour, int min)
        {
            if (days != null && days.Any())
            {
                string commaSeparatedDays = string.Join(",", days.Select(d => d.ToString()).ToArray());

                return string.Format("{0} {1} {2} * *", min, hour, commaSeparatedDays);
            }
            else
            {
                return string.Format("{0} {1} * * *", min, hour);
            }

        }

        public static string CronWithMinuteInterval(int intervalInMinutes)
        {
            return string.Format("*/{0} * * * *", intervalInMinutes);
        }

        public static string CronWithHourInterval(int intervalInHours, int min = 0)
        {
            return string.Format("{0} */{1} * * *", min, intervalInHours);
        }

        public static string CronWithDayInterval(int intervalInDays, int hour = 0, int min = 0)
        {
            return string.Format("{0} {1} */{2} * *", min, hour, intervalInDays);
        }
    }
    public class CronDescriptor
    {
        public static string DescribeCron(string cron)
        {
            if (string.IsNullOrEmpty(cron))
                return null;

            return CronExpressionDescriptor.ExpressionDescriptor.GetDescription(cron, new CronExpressionDescriptor.Options() { Use24HourTimeFormat = true });
        }

        public static CronConfiguration ParseCron(string cron)
        {
            CronExpressionDescriptor.ExpressionParser parser = new CronExpressionDescriptor.ExpressionParser(cron, new CronExpressionDescriptor.Options() { Use24HourTimeFormat = true });
            string[] cronFields = parser.Parse().Select(cf => cf.Trim()).Where(cf => !string.IsNullOrEmpty(cf)).ToArray();
            CronConfiguration config = new CronConfiguration();

            if (Regex.IsMatch(cronFields[0], "\\*/\\d"))
            {
                try
                {
                    config.Minute = null;
                    config.MinuteInterval = int.Parse(cronFields[0].Split('/')[1]);
                }
                catch (Exception)
                {
                    config.MinuteInterval = null;
                }
            }
            else
            {
                try
                {
                    config.Minute = int.Parse(cronFields[0]);
                    config.MinuteInterval = null;
                }
                catch (Exception)
                {
                    // no op, leave null
                    config.Minute = null;
                }
            }

            if (Regex.IsMatch(cronFields[1], "\\*/\\d"))
            {
                try
                {
                    config.Hour = null;
                    config.HourInterval = int.Parse(cronFields[1].Split('/')[1]);
                }
                catch (Exception)
                {
                    // no op, leave null
                    config.HourInterval = null;
                }
            }
            else
            {
                try
                {
                    config.Hour = int.Parse(cronFields[1]);
                    config.HourInterval = null;
                }
                catch (Exception)
                {
                    config.Hour = null;
                }
            }

            if (Regex.IsMatch(cronFields[2], "\\*/\\d"))
            {
                try
                {
                    config.Days = null;
                    config.DayInterval = int.Parse(cronFields[2].Split('/')[1]);
                }
                catch (Exception)
                {
                    // no op, leave null
                    config.DayInterval = null;
                }
            }
            else
            {
                try
                {
                    config.Days = cronFields[2].Split(',').Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).Select(x => int.Parse(x)).ToList();
                    config.DayInterval = null;
                }
                catch (Exception)
                {
                    config.Days = null;
                }
            }
            try
            {
                config.Weekdays = cronFields[4].Split(',').Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).Select(x => (CronExpressionDay)int.Parse(x)).ToList();
            }
            catch (Exception)
            {
                config.Weekdays = null;
            }

            if (config.DayInterval != null)
            {
                config.Type = CronConfiguration.DAY_INTERVAL;
            }

            else if (config.HourInterval != null)
            {
                config.Type = CronConfiguration.HOUR_INTERVAL;
            }

            else if (config.MinuteInterval != null)
            {
                config.Type = CronConfiguration.MINUTE_INTERVAL;
            }
            else if (config.Weekdays != null && config.Weekdays.Any())
            {
                config.Type = CronConfiguration.EXACT_DAY_TIME_WORKS_WEEKLY;
            }

            else if (config.Days != null && config.Days.Any())
            {
                config.Type = CronConfiguration.EXACT_DAY_TIME_WORKS_MONTHLY;
            }

            else
            {
                config.Type = CronConfiguration.EXACT_TIME_WORKS_DAILY;
            }

            return config;
        }
    }
}
