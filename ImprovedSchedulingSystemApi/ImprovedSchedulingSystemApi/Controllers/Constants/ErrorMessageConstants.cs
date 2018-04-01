using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovedSchedulingSystemApi.Controllers.Constants
{
    public static class ErrorMessageConstants
    {
        public const string APPOINTMENT_CONFLICT = "There is a time conflict between two appointments";
        public const string MODEL_INVAILD = "Make sure all required fields are filed";
        public const string OBJECT_ID_INVALID = "Object Id is invalid";
        public const string UPDATE_FAILED = "Updated failed to save to db.";
    }
}
