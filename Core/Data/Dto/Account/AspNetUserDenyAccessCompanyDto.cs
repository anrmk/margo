﻿using System;

namespace Core.Data.Dto {
    public class AspNetUserDenyAccessCompanyDto {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        public Guid? CompanyId { get; set; }
        public CompanyDto Company { get; set; }
    }
}
