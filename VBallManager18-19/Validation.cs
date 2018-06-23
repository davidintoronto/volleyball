using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VballManager
{
    public class Validation
    {
        public static bool IsDropinSpotOpening(DateTime date, Pool pool, VolleyballClub manager)
        {
            DateTime reserveDate = TimeZoneInfo.ConvertTimeFromUtc(date, TimeZoneInfo.FindSystemTimeZoneById(manager.TimeZoneName));
            reserveDate = reserveDate.AddDays(-1 * pool.DaysBeforeReserve).AddHours(-1 * reserveDate.Hour + manager.DropinSpotOpeningHour);
            DateTime now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(manager.TimeZoneName));
            return now >= reserveDate;
        }
        public static bool IsReservationLocked(DateTime date, VolleyballClub manager)
        {
            DateTime lockDate = TimeZoneInfo.ConvertTimeFromUtc(date, TimeZoneInfo.FindSystemTimeZoneById(manager.TimeZoneName));
            lockDate = lockDate.AddHours(-1 * lockDate.Hour + manager.LockReservationHour);
            DateTime now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(manager.TimeZoneName));
            return now >= lockDate;
        }
        public static bool DropinSpotAvailable(Pool pool, DateTime date)
        {
            if (!pool.HasCap)
            {
                return true;
            }
            int memberPlayers = pool.GetNumberOfAttendingMembers(date);
            int dropinPlayers = pool.GetNumberOfDropins(date);
            return memberPlayers + dropinPlayers < pool.MaximumPlayerNumber;
        }

        public static bool DropinSpotAvailableForCoop(Pool pool, DateTime date)
        {
            if (!pool.HasCap)
            {
                return true;
            }
            int reservedCoop = pool.GetNumberOfReservedCoops(date);
            if (reservedCoop >= pool.MaxCoopPlayers)
            {
                return false;
            }
            int memberPlayers = pool.GetNumberOfAttendingMembers(date);
            int dropinPlayers = pool.GetNumberOfDropins(date);
            return memberPlayers + dropinPlayers <pool.LessThanPayersForCoop;
        }
        public static bool MemberSpotAvailable(Pool pool, DateTime date)
        {
            if (!pool.HasCap)
            {
                return true;
            }
            int memberPlayers = pool.GetNumberOfAttendingMembers(date);
            int dropinPlayers = pool.GetNumberOfDropins(date);
            if (pool.MaximumPlayerNumber > pool.Members.Count)
            {
                return memberPlayers + dropinPlayers < pool.MaximumPlayerNumber;
            }
            else
            {
                return memberPlayers + dropinPlayers < pool.Members.Count;
            }
        }
    }
}