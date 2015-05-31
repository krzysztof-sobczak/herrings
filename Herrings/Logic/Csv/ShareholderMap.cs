using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herrings.Logic.Csv
{
    class ShareholderMap : CsvClassMap<Shareholder>
    {
        public override void CreateMap()
        {
            Map(m => m.Id).Name("Id").ConvertUsing(row => int.Parse(row.GetField(0))).Index(0);
            Map(m => m.Name).Name("Name").Index(1);
            Map(m => m.Share).Name("Share").ConvertUsing(
                row =>
                {
                    float value = 0;
                    float.TryParse(row.GetField(2), out value);
                    return value;
                }
            ).Index(2);
            Map(m => m.SpiedId).Name("SpyId").ConvertUsing(
                row => {
                    int value = 0;
                    int.TryParse(row.GetField(3), out value);
                    return value;
                }
            ).Index(3);
        }
    }
}
