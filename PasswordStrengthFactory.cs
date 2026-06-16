using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_KPL_ManajemenPassword
{
    public class StrengthResult
    {
        public string Status { get; set; }
        public Color Warna { get; set; }
        public int Score { get; set; }

        public StrengthResult(string status, Color warna, int score)
        {
            this.Status = status;
            this.Warna = warna;
            this.Score = score;
        }
    }

    public abstract class StrengthFactory
    {
        public abstract StrengthResult CreateStrengthResult(int score);
    }

    public class PasswordStrengthFactory : StrengthFactory
    {
        public override StrengthResult CreateStrengthResult(int score)
        {
            var strengthTable = new Dictionary<int, (string Status, Color Warna)>
            {
                { 0, ("Kekuatan: Sangat Lemah", Color.Red) },
                { 1, ("Kekuatan: Lemah", Color.Red) },
                { 2, ("Kekuatan: Sedang", Color.Orange) },
                { 3, ("Kekuatan: Kuat", Color.LightGreen) },
                { 4, ("Kekuatan: Sangat Kuat", Color.Green) }
            };

            if (strengthTable.TryGetValue(score, out var result))
            {
                return new StrengthResult(result.Status, result.Warna, score);
            }

            return new StrengthResult("Kekuatan: -", Color.Gray, 0);
        }
    }
}