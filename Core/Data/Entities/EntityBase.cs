using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Data.Entities {
    public interface IEntityBase<T> {
        T Id { get; set; }
    }

    public abstract class EntityBase<T>: IEntityBase<T> {
        [Key]
        public virtual T Id { get; set; }
    }

    #region AUDITABLE ENTITY
    public interface IAuditableEntity {
        DateTime CreatedDate { get; set; }
        string CreatedBy { get; set; }
        DateTime UpdatedDate { get; set; }
        string UpdatedBy { get; set; }
    }

    public abstract class AuditableEntity<T>: EntityBase<T>, IAuditableEntity {
        [ScaffoldColumn(false)]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [MaxLength(256)]
        [ScaffoldColumn(false)]
        public string CreatedBy { get; set; } = "system";

        [ScaffoldColumn(false)]
        [DataType(DataType.DateTime)]
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        [MaxLength(256)]
        [ScaffoldColumn(false)]
        public string UpdatedBy { get; set; } = "system";
    }
    #endregion

    #region NSI ENTITY
    public interface INsiEntity {
        string Name { get; set; }
        string Code { get; set; }
    }

    public abstract class NsiEntity<T>: EntityBase<T>, INsiEntity {
        public string Name { get; set; }
        public string Code { get; set; }
    }
    #endregion
}
