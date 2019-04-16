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
            }
            else
            {
                IsAuthenticated = false;
            }
        }


        public void OnSendMetrics(IActivity activity)
        {
            File.AppendAllLines("output.txt", activity.Metrics.Select(n => n.ToString()).ToArray());
            using (var client = new Client.Client(_authData.Email, _authData.Password))
            {
                //client.SendMetrics(activity.Metrics);
            }      
            SaveData();
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
