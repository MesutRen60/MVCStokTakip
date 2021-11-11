using MVC_StokTakip.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace MVC_StokTakip.Roller
{
    public class KullaniciRolleri : RoleProvider
    {
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            MVC_StokTakipEntities db = new MVC_StokTakipEntities();
            //var kullanici = db.Kullanicilar.FirstOrDefault(x => x.KullaniciAdi == username);
            //return new string[] { kullanici.Rol };

            //Loginde girilen Kullanci adını alarak Kullanıcılar tablosundaki KullaniciAdi alanı ile karşılaştırıp varsa eşleşen satırları Listeye atıyor. 
            //var kullaniciRolleri2 = db.KullaniciRolleri.Where(x => x.Kullanicilar.KullaniciAdi == username).Select(x => x.Roller.Rol).ToList();
            var kullaniciRolleri = db.KullaniciRolleri.Where(x => x.Kullanicilar.KullaniciAdi == username).ToList();
            

            //String bir dizi tanımladık boyutu.Kullanıcının sahip olduğu rollerin sayısı kadar yapıyoruz..
            string[] roller = new string[kullaniciRolleri.Count];

            if (kullaniciRolleri.Count > 0)
            {
                for (int i = 0; i < roller.Length; i++)
                {
                    foreach (var item in kullaniciRolleri)
                    {
                        roller[i] = item.Roller.Rol.Trim();
                        kullaniciRolleri.Remove(item);
                        break;
                    }
                }
                return roller;
            }
            return new string[] { "" };

        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}