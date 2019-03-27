using System;
using System.Collections.Generic;
using System.IO;
using ApiClient;
using innometrics_visual_studio.Controller;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace innometrics_visual_studio.Model
{
    public class DataManager
    {
        public bool IsAuthenticated { get; private set; }

        private AuthData _authData;

        private List<IActivity> _activities;


        public DataManager()
        {
            LoadData();
            Authenticate(_authData.Email,_authData.Password);
        }


        public DataManager(List<IActivity> activities)
        {
            _activities = activities;
            LoadData();
            Authenticate(_authData.Email, _authData.Password);
        }


        public void UnAuthenticate()
        {
            IsAuthenticated = false;
            _authData.Clear();
        }


        public void Authenticate(string email, string password)
        {
            if (Client.IsAuthDataCorrect(email, password))
            {
                _authData = new AuthData(email, password);
                IsAuthenticated = true;
            }
            else
            {
                IsAuthenticated = false;
            }
        }


        public void OnPluginStop()
        {
            //todo check on errors
            using (var client = new Client(_authData.Email, _authData.Password))
            {
                client.SendMetrics(GetAllMetrics());
            }      
            SaveData();
        }


        private IEnumerable<IMetric> GetAllMetrics()
        {           
            return _activities.SelectMany(n => n.Metrics);
        }


        private void LoadData()
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream fs = new FileStream("auth.dat", FileMode.Open);
                _authData = (AuthData) formatter.Deserialize(fs);
                fs.Dispose();
            }
            catch (Exception e)
            {
                //todo fix newtone.json error and change 
                //_authData = new AuthData();
                _authData = new AuthData("kirill1998fed@yandex.ru", "fkmlbyf123");
            }
            finally { }

        }


        private void SaveData()
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream fs = new FileStream("auth.dat", FileMode.Open);
                formatter.Serialize(fs, _authData);
                fs.Dispose();
            }
            catch (Exception e)
            {
                _authData = new AuthData();
            }
            finally { }
        }
    }

    internal struct AuthData
    {
        internal string Email { get; private set; }
        internal string Password { get; private set; }


        public AuthData(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public void Clear()
        {
            Email = "";
            Password = "";
        }
    }
}
