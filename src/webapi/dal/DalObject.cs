using dto;

namespace dal
{
    public interface IDalObject
    {
        IDtoObject LoadTo(IDtoObject dto, MoneyboardContext db);

        void UpdateFrom(IDtoObject dto, MoneyboardContext db);

        DTO CreateDto<DTO>(MoneyboardContext db) where DTO : IDtoObject, new();
    }
        
    public abstract class DalObject : IDalObject
    {
        public abstract IDtoObject LoadTo(IDtoObject dto, MoneyboardContext db);

        public abstract void UpdateFrom(IDtoObject dto, MoneyboardContext db);

        public DTO CreateDto<DTO>(MoneyboardContext db)
            where DTO : IDtoObject, new()
        {
            return (DTO)this.LoadTo(new DTO(), db);
        }
    }
}