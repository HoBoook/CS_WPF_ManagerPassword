using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_WPF_ManagerPassword
{
    internal class ItemRecord
    {
        private string login;
        private string password;
        private string resourse;
        public string Resource { get { return resourse; } set { resourse = value; } }
        public string Login { get { return login; } set { login = value; } }
        public string Password { get { return password; } set { password = value; } }

        public ItemRecord(string login, string password, string resourse) {
            this.login = login;
            this.password = password;
            this.resourse = resourse;
        }
    }
}
