using System;
using System.Text.RegularExpressions;

namespace Time
{
    public readonly struct Time : IEquatable<Time>, IComparable<Time>
    {
        private readonly byte hours;
        private readonly byte minutes;
        private readonly byte seconds;

        public Time(byte hours = 0, byte minutes = 0, byte seconds = 0, string input = "00:00:00")
        {          

           if (hours == 0 && minutes == 0 && seconds == 0)
           {
                if (!Regex.IsMatch(input, @"^\d{1,2}:\d{1,2}:\d{1,2}$"))
                {
                    throw new ArgumentException("Invalid time format. Expected format: 'xx:xx:xx'.");
                }

                string[] stringToValue = input.Split(':');

                hours = Byte.Parse(stringToValue[0]);
                minutes = Byte.Parse(stringToValue[1]);
                seconds = Byte.Parse(stringToValue[2]);

                if (hours >= 24 || minutes >= 60 || seconds >= 60)
                {
                    throw new ArgumentException("Invalid values.");
                }
           }

            this.hours = hours;
            this.minutes = minutes;
            this.seconds = seconds;
        }
        public readonly byte Hours => hours;
        public readonly byte Minutes => minutes;
        public readonly byte Seconds => seconds;
        public override string ToString()
        {
            return Hours.ToString("00") + ":" + Minutes.ToString("00") + ":" + Seconds.ToString("00");
        }
        public bool Equals(Time other)
        {
            if (Hours == other.Hours && Minutes == other.Minutes && Seconds == other.Seconds)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool operator ==(Time left, Time right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(Time left, Time right)
        {
            return !(left == right);
        }
        public int CompareTo(Time other)
        {
            if (Hours > other.Hours)
            {
                return 1;
            }
            else if (Hours < other.Hours)
            {
                return -1;
            }
            else
            {
                if (Minutes > other.Minutes)
                {
                    return 1;
                }
                else if (Minutes < other.Minutes)
                {
                    return -1;
                }
                else
                {
                    if (Seconds > other.Seconds)
                    {
                        return 1;
                    }
                    else if (Seconds < other.Seconds)
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }
        public static bool operator <(Time left, Time right)
        {
            return left.CompareTo(right) < 0;
        }
        public static bool operator <=(Time left, Time right)
        {
            return left.CompareTo(right) <= 0;
        }
        public static bool operator >(Time left, Time right)
        {
            return left.CompareTo(right) > 0;
        }
        public static bool operator >=(Time left, Time right)
        {
            return left.CompareTo(right) >= 0;
        }
        public Time Plus(TimePeriod timePeriod)
        {
            var totalSeconds = ((Hours * 3600) + (Minutes * 60) + Seconds + timePeriod.TimePassed) % 86400;
            var hours = (byte)(totalSeconds / 3600);
            var minutes = (byte)((totalSeconds / 60) % 60);
            var seconds = (byte)(totalSeconds % 60);
            return new Time(hours, minutes, seconds);
        }
        public static Time Plus(Time time, TimePeriod timePeriod)
        {
            return time.Plus(timePeriod);
        }
        public static Time operator +(Time time, TimePeriod timePeriod)
        {
            return time.Plus(timePeriod);
        }
        public Time Minus(TimePeriod timePeriod)
        {
            var totalSeconds = ((Hours * 3600) + (Minutes * 60) + Seconds - timePeriod.TimePassed) % 86400;
            var hours = (byte)((totalSeconds + 86400) / 3600 % 24);
            var minutes = (byte)((totalSeconds / 60) % 60);
            var seconds = (byte)(totalSeconds % 60);
            return new Time(hours, minutes, seconds);
        }
        public static Time Minus(Time time, TimePeriod timePeriod)
        {
            return time.Minus(timePeriod);
        }
        public static Time operator -(Time time, TimePeriod timePeriod)
        {
            return time.Minus(timePeriod);
        }
    }

    public readonly struct TimePeriod : IEquatable<TimePeriod>, IComparable<TimePeriod>
    {
        private readonly long timepassed;
        private readonly Time t1;
        private readonly Time t2;

        public TimePeriod(Time t1 = default(Time) , Time t2 = default(Time),
            byte t1Hours = 0, byte t1Minutes = 0, byte t1Seconds = 0, string t1Input = "",
            byte t2Hours = 0, byte t2Minutes = 0, byte t2Seconds = 0, string t2Input = ""
            )
        {
            if (t1 == default(Time))
            {
                if (t1Hours == 0 && t1Minutes == 0 && t1Seconds == 0)
                {
                    string[] stringToValue = t1Input.Split(':');

                    byte hours = Byte.Parse(stringToValue[0]);
                    byte minutes = Byte.Parse(stringToValue[1]);
                    byte seconds = Byte.Parse(stringToValue[2]);

                    if (!Regex.IsMatch(t1Input, @"^\d{1,2}:\d{1,2}:\d{1,2}$"))
                    {
                        throw new ArgumentException("Invalid time format. Expected format: 'xx:xx:xx'.");
                    }

                    t1 = new Time(hours, minutes, seconds);
                }
                else
                {
                    t1 = new Time(t1Hours, t1Minutes, t1Seconds);
                }
                
            }

            if (t2 == default(Time))
            {
                if (t2Hours == 0 && t2Minutes == 0 && t2Seconds == 0)
                {
                    string[] stringToValue = t2Input.Split(':');

                    byte hours = Byte.Parse(stringToValue[0]);
                    byte minutes = Byte.Parse(stringToValue[1]);
                    byte seconds = Byte.Parse(stringToValue[2]);

                    if (!Regex.IsMatch(t2Input, @"^\d{1,2}:\d{1,2}:\d{1,2}$"))
                    {
                        throw new ArgumentException("Invalid time format. Expected format: 'xx:xx:xx'.");
                    }

                    t2 = new Time(hours, minutes, seconds);
                }
                else
                {
                    t2 = new Time(t2Hours, t2Minutes, t2Seconds);
                }

                
            }

            if (t1Hours >= 24 || t1Minutes >= 60 || t1Seconds >= 60 || t2Hours >= 24 || t2Minutes >= 60 || t2Seconds >= 60)
            {
                throw new ArgumentException("Invalid values.");
            }

            if (t1 > t2)
            {
                timepassed = 86400 + (t2.Hours * 60 * 60 - t1.Hours * 60 * 60) + (t2.Minutes * 60 - t1.Minutes * 60) + (t2.Seconds - t1.Seconds);
            }
            else
            {
                timepassed = (t2.Hours * 60 * 60 - t1.Hours * 60 * 60) + (t2.Minutes * 60 - t1.Minutes * 60) + (t2.Seconds - t1.Seconds);
            }
          
            this.t1 = t1;
            this.t2 = t2;
        }

        public readonly long TimePassed => timepassed;
        public readonly Time T1 => t1;
        public readonly Time T2 => t2;
        public override string ToString()
        {
            var hours = (byte)(TimePassed / 3600);
            var minutes = (byte)(TimePassed % 3600 / 60);
            var seconds = (byte)(TimePassed % 60);
            return hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
        }
        public bool Equals(TimePeriod other)
        {
            if (TimePassed == other.TimePassed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool operator ==(TimePeriod left, TimePeriod right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(TimePeriod left, TimePeriod right)
        {
            return !(left == right);
        }
        public int CompareTo(TimePeriod other)
        {
            if (TimePassed > other.TimePassed)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        public static bool operator <(TimePeriod left, TimePeriod right)
        {
            return left.CompareTo(right) < 0;
        }
        public static bool operator <=(TimePeriod left, TimePeriod right)
        {
            return left.CompareTo(right) <= 0;
        }
        public static bool operator >(TimePeriod left, TimePeriod right)
        {
            return left.CompareTo(right) > 0;
        }
        public static bool operator >=(TimePeriod left, TimePeriod right)
        {
            return left.CompareTo(right) >= 0;
        }
        public TimePeriod Plus(TimePeriod other)
        {
            var totalSeconds = TimePassed + other.TimePassed;
            var T2hours = (byte)(totalSeconds / 3600);
            var T2minutes = (byte)((totalSeconds / 60) % 60);
            var T2seconds = (byte)(totalSeconds % 60);

            return new TimePeriod(t1Input: "00:00:00", t2Hours: T2hours, t2Minutes: T2minutes, t2Seconds: T2seconds);
        }
        public static TimePeriod Plus(TimePeriod timePeriod1, TimePeriod timePeriod2)
        {
            return timePeriod1.Plus(timePeriod2);
        }
        public static TimePeriod operator +(TimePeriod timePeriod1, TimePeriod timePeriod2)
        {
            return timePeriod1.Plus(timePeriod2);
        }
        public TimePeriod Minus(TimePeriod other)
        {
            var totalSeconds = TimePassed - other.TimePassed;
            var T2hours = (byte)(totalSeconds / 3600);
            var T2minutes = (byte)((totalSeconds / 60) % 60);
            var T2seconds = (byte)(totalSeconds % 60);

            return new TimePeriod(t1Input: "00:00:00", t2Hours: T2hours, t2Minutes: T2minutes, t2Seconds: T2seconds);
        }
        public static TimePeriod Minus(TimePeriod timePeriod1, TimePeriod timePeriod2)
        {
            return timePeriod1.Plus(timePeriod2);
        }
        public static TimePeriod operator -(TimePeriod timePeriod1, TimePeriod timePeriod2)
        {
            return timePeriod1.Plus(timePeriod2);
        }
    }
}