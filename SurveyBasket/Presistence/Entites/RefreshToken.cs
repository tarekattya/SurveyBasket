﻿namespace SurveyBasket.Presistence.Entites
{

    [Owned]
    public class RefreshToken
    {
        public string Token { get; set; } = string.Empty;

        public DateTime ExpireOn { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime? RevokedOn { get; set; }

        public bool IsExpired => DateTime.UtcNow >= ExpireOn;

        public bool IsActive => RevokedOn == null && !IsExpired;




    }
}
