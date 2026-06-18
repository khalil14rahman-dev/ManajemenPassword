using System;
using System.Collections.Generic;

namespace Project_KPL_ManajemenPassword
{
    public class RecoveryItem
    {
        public int IdQuestion { get; set; }        
        public string CustomQuestionText { get; set; } 
        public string Answer { get; set; }         
        public bool IsCustom { get; set; }      
    }

    public class PasswordRequestDto
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public List<RecoveryItem> RecoveryList { get; set; } = new List<RecoveryItem>();

        public bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(Password)) return false;
            if (Password.Length < 8) return false;

            if (RecoveryList == null || RecoveryList.Count == 0) return false;

            return true;
        }
    }
}