using System;
using System.Collections.Generic;

namespace DoctorAppointmentSystem.Helpers
{
    public static class SlotGeneratorHelper
    {
        public static List<TimeSpan> GenerateSlots(TimeSpan start, TimeSpan end, int duration)
        {
            var slots = new List<TimeSpan>();
            var current = start;

            while (current < end)
            {
                slots.Add(current);
                current = current.Add(TimeSpan.FromMinutes(duration));
            }

            return slots;
        }
    }
}