﻿// ReSharper disable VirtualMemberCallInConstructor
namespace PLF_Football.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    using Microsoft.AspNetCore.Identity;
    using PLF_Football.Data.Common.Models;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
            this.Games = new HashSet<UserGame>();
            this.Messages = new HashSet<Message>();
        }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public int? ClubId { get; set; }

        [ForeignKey("ClubId")]
        public virtual Club FavoriteTeam { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

        public virtual ICollection<UserGame> Games { get; set; }

        public virtual ICollection<Message> Messages { get; set; }

        [NotMapped]
        public int TotalPoints => this.Games.Sum(x => x.Points);
    }
}
