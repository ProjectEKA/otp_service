using System;
using System.Collections.Generic;
using In.ProjectEKA.OtpService.Common;

namespace In.ProjectEKA.OtpService.Otp
{
    public class OtpGenerationDetail
    {
        private static Dictionary<Action, String> templateIDs;
        static OtpGenerationDetail()
        {
            templateIDs = new Dictionary<Action, string>();
            templateIDs.Add(Action.LINK_PATIENT_CARECONTEXT, "1007160803219758234");
            templateIDs.Add(Action.FORGOT_PIN, "1007160803221956026");
        }
        public string SystemName { get; set; }
        public Action Action { get; set; }

        public OtpGenerationDetail(string systemName, string action)
        {
            SystemName = systemName;
            Action = EnumUtil.ParseEnum<Action>(action);
        }
        
        public String GetTemplateID()
        {
            return templateIDs[Action];
        }
    }
}