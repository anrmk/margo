﻿using System;
using Core.Data.Enums;

namespace Core.Data.Dto {
    public class CompanySectionFieldDto {
        public Guid Id { get; set; }
        public FieldEnum Type { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public bool IsRequired { get; set; }
        public int Sort { get; set; }
        public bool IsHidden { get; set; }
    }
}