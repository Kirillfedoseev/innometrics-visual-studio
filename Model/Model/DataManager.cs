using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Client;

namespace Model.Model
{
    public class DataManager
    {
        public bool IsAuthenticated { get; private set; }

        private AuthData _authData;


        public DataManager()
        {
            LoadData();
            Authenticate(_authData.Email,_authData.Password);
        }


        public void UnAuthenticate()
        {
            IsAuthenticated = false;
            _authData.Clear();
        }


        public void Authenticate(string email, string password)
        {
            if (Client.Client.IsAuthDataCorrect(email, password))
            {
                _authData = new AuthData(email, password);
                IsAuthenticated = true;
                SaveData();
            }
            else
            {
                IsAuthenticated = false;
            }
        }


        public void SendMetrics(List<IActivity> activities)
        {
            Metric[] metrics = activities.SelectMany(n => n.Metrics).ToArray();

            File.AppendAllLines("output.txt", metrics.Select(n => n.ToString()).ToArray());

            using (var client = new Client.Client(_authData.Email, _authData.Password))
            {
                client.SendMetrics(metrics);
            }

            activities.ForEach(n => n.CleanMetricsStorage());
        }

        private void LoadData()
        {
            FileStream fs = null;
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                fs = new FileStream("auth.dat", FileMode.Open);
                _authData = (AuthData) formatter.Deserialize(fs);
            }
            catch (Exception e)
            {
                //todo fix newtone.json error and change 
                //_authData = new AuthData();
                _authData = new AuthData("kirill1998fed@yandex.ru", "fkmlbyf123");
            }
            finally { fs?.Dispose();}

        }


        private void SaveData()
        {
            FileStream fs = null;
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                fs = new FileStream("auth.dat", FileMode.Open);
                formatter.Serialize(fs, _authData);
            }
            catch (Exception e)
            {
                _authData = new AuthData();
            }
            finally { fs?.Dispose(); }
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
