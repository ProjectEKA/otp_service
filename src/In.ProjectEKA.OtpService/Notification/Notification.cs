using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace In.ProjectEKA.OtpService.Notification
{
    public class Notification
    {
        private static Dictionary<Action, String> templateIDs;
        static Notification()
        {
            templateIDs = new Dictionary<Action, string>();
            templateIDs.Add(Action.ConsentRequestCreated, "1007160803224559069");
            templateIDs.Add(Action.ConsentManagerIdRecovered, "1007160803226874870");
            templateIDs.Add(Action.HIPVisitSMSNotification, "1007161123126671321");
        }
        
        public string Id { get; }
        public Communication Communication { get; }
        public JObject Content { get; }
        public Action Action { get; }

        public Notification(string id, Communication communication, JObject content, Action action)
        {
            Id = id;
            Communication = communication;
            Content = content;
            Action = action;
        }
        
        public String GetTemplateID()
        {
            return templateIDs[Action];
        }
    }
}
