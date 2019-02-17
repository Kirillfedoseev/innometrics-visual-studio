using System.Collections.Generic;
using ApiClient;
using innometrics_visual_studio.Controller;
using System.Linq;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.TextManager.Interop;

namespace innometrics_visual_studio.Model
{
    class DataManager
    {

        public AuthData _authData;
        private List<IActivity> _activities;


        public DataManager(List<IActivity> activities)
        {
            _activities = activities;
        }



        public void OnPluginStop()
        {
            //todo check on errors
            using (var client = new Client(_authData.Email, _authData.Password))
            {
                client.SendMetrics(GetAllMetrics());
            }                   
        }


        private IEnumerable<IMetric> GetAllMetrics()
        {           
            return _activities.Select(n => n.Metric);
        }

    }

    internal struct AuthData
    {
        internal string Email;
        internal string Password;

        public AuthData(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
