using dal.Model;
using dto;
using System.Collections.Generic;

namespace dal
{
    public static class DalUtilsExtensions
    {
        public static IEnumerable<DTO> ConvertToDtoList<DTO>(this IEnumerable<IDalObject> queryDal, MoneyboardContext db)
            where DTO : IDtoObject, new()
        {
            var enumerator = queryDal.GetEnumerator();

            while (enumerator.MoveNext())
                yield return enumerator.Current.CreateDto<DTO>(db);
        }
    }
}