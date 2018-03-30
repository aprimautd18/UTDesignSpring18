using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using ImprovedSchedulingSystemApi.Controllers;
using ImprovedSchedulingSystemApi.Models.CalenderDTO;

namespace ImprovedSchedulingSystemApi.Database.ModelAccessors
{
    public class helperClasses
    {
        public static bool appointmentListConflict(List<AppointmentModel> appointments)
        {
            bool output = false;
            for (int i = 1; i < appointments.Count; i++)
            {
                output = output || AppointmentModel.conflict(appointments[i - 1], appointments[i]);
            }

            return output;
        }

        public static List<AppointmentModel> fastSortAdd(List<AppointmentModel> list, AppointmentModel toAdd)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] >= toAdd)
                {
                    list.Insert(i,toAdd);
                    break;
                }

            }

            if (!list.Contains(toAdd))
            {
                list.Add(toAdd);
            }
            return list;
        }
    }
}
